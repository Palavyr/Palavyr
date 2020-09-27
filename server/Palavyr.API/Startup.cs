using System;
using System.IO;
using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Amazon.SimpleEmail;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DashboardServer.Data;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Palavyr.API.CustomMiddleware;
using Palavyr.API.pathUtils;
using Palavyr.Background;
using Palavyr.Common.FileSystem;
using static Microsoft.Extensions.Hosting.Environments;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Palavyr.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }
        private readonly ILogger<Startup> _logger;

        public Startup(ILoggerFactory loggerFactory)
        {
            // Configuration = configuration;
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            
            Console.WriteLine($"Current env: {env}");
            var appsettings = $"appsettings.{env}.migrator.json";
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile(appsettings, false)
                .Build();

            
            // ReSharper disable once HeapView.ClosureAllocation
            // var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            _logger.LogDebug($"Startup-1: ENV VARIABLE ASPNETCORE_ENVIRONMENT = {env}");
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .SetIsOriginAllowed(_ => true)
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
                            )
                            .WithMethods("DELETE", "POST", "GET", "OPTIONS", "PUT");

                        if (env == Staging || env == Production)
                        {
                            builder.WithOrigins(
                                "http://palavyr.com",
                                "http://www.palavyr.com",
                                "https://palavyr.com",
                                "https://www.palavyr.com"
                            );
                        }

                        if (env == Development)
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

            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
                _logger.LogDebug($"STARTUP-2: Platform = {Environment.OSVersion.Platform.ToString()}");
                if (env == Staging || env == Production)
                {
                    _logger.LogDebug($"STARTUP-3: env = {env}");
                    services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });
                }
            }
            else
            {
                _logger.LogDebug($"STARTUP-4: Platform = {Environment.OSVersion.Platform.ToString()}");
                if (env == Staging || env == Production)
                {
                    _logger.LogDebug($"STARTUP-5: env = {env}");
                    services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));
                }
            }

            services.AddHangfire(config =>
                config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseMemoryStorage());
            services.AddHangfireServer();

            services.AddSingleton<ICreatePalavyrSnapshot, CreatePalavyrSnapshot>();
            services.AddSingleton<IRemoveOldS3Archives, RemoveOldS3Archives>();
            services.AddSingleton<IRemoveStaleSessions, RemoveStaleSessions>();
            services.AddSingleton<IValidateAttachments, ValidateAttachments>();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider
        )
        {
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
                _logger.LogDebug("STARTUP-7: We are running on LINUX.");
                var home = Environment.GetEnvironmentVariable("HOME");
                _logger.LogDebug($"STARTUP: HOME env variable = {home}");
                if (home == null)
                {
                    _logger.LogDebug($"STARTUP-8: HOME VARIABLE NOT SET");
                }

                appDataPath = Path.Combine(home, MagicPathStrings.DataFolder);
            }
            _logger.LogDebug($"STARTUP-9: APPDATAPATH: {appDataPath}");
            
            DiskUtils.CreateDir(appDataPath);
            
            return appDataPath;
        }
    }
}