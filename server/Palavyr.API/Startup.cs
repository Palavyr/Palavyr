using System;
using System.IO;
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
using Palavyr.Background;
using Palavyr.Common.FileSystem;
using static Microsoft.Extensions.Hosting.Environments;

namespace Palavyr.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // ReSharper disable once HeapView.ClosureAllocation
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

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


            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonSimpleEmailService>();
            services.AddAWSService<IAmazonS3>();
            services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });

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

            var appDataPath = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), MagicPathStrings.DataFolder);
            if (string.IsNullOrEmpty(Configuration["WebRootPath"]))
                Configuration["WebRootPath"] = Environment.CurrentDirectory;

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
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
    }
}