using System;
using System.IO;
using System.Reflection;
using Amazon.S3;
using Amazon.SimpleEmail;
using DashboardServer.Data;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Palavyr.API.CustomMiddleware;
using Palavyr.API.pathUtils;
using Palavyr.Background;
using Palavyr.Common.FileSystem;

namespace Palavyr.API
{
    public class Startup
    {
        public Startup(IWebHostEnvironment Env)
        {
            env = Env;
        }
        private IConfiguration Configuration { get; set; }
        private ILogger<Startup> _logger { get; set; }
        private IWebHostEnvironment env { get; set; } 

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = $"appsettings.{env.EnvironmentName.ToLower()}.json";
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(appSettings, false)
                .AddJsonFile("appsettings.json", true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .Build();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .SetIsOriginAllowed(_ => true)
                            .WithMethods("DELETE", "POST", "GET", "OPTIONS", "PUT")
                            .WithHeaders(
                                "action",
                                "Server",
                                "sessionId",
                                "Content-Type",
                                "Access-Control-Allow-Origin",
                                "Access-Control-Allow-Headers",
                                "Access-Control-Allow-Methods",
                                "Authorization",
                                "X-Requested-With"
                            );

                        if (env.IsStaging() || env.IsProduction())
                        {
                            builder.WithOrigins(
                                "http://palavyr.com",
                                "http://www.palavyr.com",
                                "https://palavyr.com",
                                "https://www.palavyr.com"
                            );
                        } 
                        else
                        {
                            builder.WithOrigins(
                                "http://localhost/",
                                "https://localhost/",
                                "http://localhost",
                                "https://localhost",
                                "http://localhost:5000/",
                                "https://localhost:5001/",
                                "http://localhost:5000",
                                "https://localhost:5001"
                            );
                        }
                    });
            });

            services.AddControllers();

            services.AddDbContext<AccountsContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("AccountsContextPostgres")));
            services.AddDbContext<ConvoContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("ConvoContextPostgres")));
            services.AddDbContext<DashContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("DashContextPostgres")));

            // AWS Services
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonSimpleEmailService>();
            services.AddAWSService<IAmazonS3>();

            Console.WriteLine($"STARTUP-2: Platform = {Environment.OSVersion.Platform.ToString()}");
            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
                if (env.IsStaging() || env.IsProduction())
                {
                    services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });
                }
            }
            else
            {
                Console.WriteLine($"STARTUP-4: Platform = {Environment.OSVersion.Platform.ToString()}");
                if (env.IsStaging() || env.IsProduction())
                {
                    Console.WriteLine($"STARTUP-5: env = {env}");
                }

            }

            services.AddHangfire(config =>
                config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseMemoryStorage());
            services.AddHangfireServer();

            // services.AddScoped<ICreatePalavyrSnapshot, CreatePalavyrSnapshot>();
            services.AddScoped<IRemoveOldS3Archives, RemoveOldS3Archives>();
            services.AddScoped<IRemoveStaleSessions, RemoveStaleSessions>();
            services.AddScoped<IValidateAttachments, ValidateAttachments>();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory
        )
        {
            _logger = loggerFactory.CreateLogger<Startup>();
            var option = new BackgroundJobServerOptions {WorkerCount = 1};
            app.UseHangfireServer(option);
            app.UseHangfireDashboard();

            var appDataPath = resolveAppDataPath();
            if (string.IsNullOrEmpty(Configuration["WebRootPath"]))
                Configuration["WebRootPath"] = Environment.CurrentDirectory;

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection(); // when we enable ssl
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(appDataPath),
                RequestPath = new PathString("")
            });
            app.UseRouting();
            app.UseCors();
            app.UseMiddleware<AuthenticateByLoginOrSession>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseHangfireDashboard();
            
            if (env.IsProduction())
            {
                app.UseHangfireDashboard();
                _logger.LogInformation("Preparing to archive teh project");
                try
                {
                    recurringJobManager
                        .AddOrUpdate(
                            "Create S3 Snapshot",
                            () => serviceProvider.GetService<ICreatePalavyrSnapshot>()
                                .CreateDatabaseAndUserDataSnapshot(),
                            Cron.Daily);
                    recurringJobManager
                        .AddOrUpdate(
                            "Keep only the last 50 snapshots",
                            () => serviceProvider.GetService<IRemoveOldS3Archives>().RemoveS3Objects(),
                            Cron.Daily
                        );
                    recurringJobManager
                        .AddOrUpdate(
                            "Clean Expired Sessions",
                            () => serviceProvider.GetService<IRemoveStaleSessions>().CleanSessionDB(),
                            Cron.Hourly
                        );
                    recurringJobManager
                        .AddOrUpdate(
                            "Validate All Attachment DB Entries",
                            () => serviceProvider.GetService<IValidateAttachments>().ValidateAllAttachments(),
                            Cron.Weekly
                        );
                    recurringJobManager
                        .AddOrUpdate(
                            "Validate All Files",
                            () => serviceProvider.GetService<IValidateAttachments>().ValidateAllFiles(),
                            Cron.Weekly
                        );
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("ERROR OMGOMGOMG WHY?: " + ex.Message);
                }
            }

            
        }
        private string resolveAppDataPath()
        {
            string appDataPath;
            var osVersion = Environment.OSVersion;
            if (osVersion.Platform != PlatformID.Unix)
            {
                _logger.LogDebug("STARTUP-6: We are running on windows.");
                appDataPath = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), MagicPathStrings.DataFolder);
            }
            else
            {
                var home = Environment.GetEnvironmentVariable("HOME");
                _logger.LogDebug($"STARTUP-8: HOME env variable = {home}");
                if (home == null)
                {
                    _logger.LogDebug($"STARTUP-9: HOME VARIABLE NOT SET");
                }

                appDataPath = Path.Combine(home, MagicPathStrings.DataFolder);
            }
            _logger.LogDebug($"STARTUP-10: APPDATAPATH: {appDataPath}");
            
            DiskUtils.CreateDir(appDataPath);
            
            return appDataPath;
        }
    }
}