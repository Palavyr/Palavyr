using System;
using System.IO;
using System.Text;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using DashboardServer.Data;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Palavyr.API.Controllers;
using Palavyr.API.controllers.accounts.newAccount;
using Palavyr.API.CustomMiddleware;
using Palavyr.Background;
using Palavyr.Common.FileSystem.FormPaths;
using Stripe;

namespace Palavyr.API
{
    public class Startup
    {
        public Startup(IWebHostEnvironment Env, IConfiguration configuration)
        {
            env = Env;
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; set; }
        private ILogger<Startup> _logger { get; set; }
        private IWebHostEnvironment env { get; set; }

        private const string _configurationDbStringKey = "DashContextPostgres";
        private const string _accountDbStringKey = "AccountsContextPostgres";
        private const string _convoDbStringKey = "ConvoContextPostgres";
        private const string _accessKeySection = "AWS:AccessKey";
        private const string _secretKeySection = "AWS:SecretKey";
        private const string _StripeKeySection = "Stripe:SecretKey";
        private const string _webhookKeySection = "Stripe:WebhookKey";

        public void ConfigureServices(IServiceCollection services)
        {
            var key = Configuration["JWTSecretKey"] ?? throw new ArgumentNullException("Configuration[\"JWTSecretKey\"]");

            services.AddLogging(loggingBuilder => { loggingBuilder.AddSeq(); });
            services.AddHttpContextAccessor();

            services
                .AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = true;
                    opt.SaveToken = true;

                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // ValidateLifetime = false,
                        // ValidIssuer = Configuration["JwtToken:Issuer"],
                        // ValidAudience = Configuration["JwtToken:Issuer"],
                    };
                })
                .AddScheme<WidgetAuthSchemeOptions, WidgetAuthenticationHandler>(
                    AuthenticationSchemeNames.WidgetScheme,
                    op => { });
               
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

                        if (env.IsDevelopment())
                        {
                            builder.WithOrigins(
                                "http://localhost/",
                                "https://localhost/",
                                "http://localhost",
                                "https://localhost",
                                "http://localhost:5000/",
                                "https://localhost:5001/",
                                "http://localhost:5000",
                                "https://localhost:5001",
                                "http://localhost:3600",
                                "https://localhost:3500",
                                "https://stipe.com"
                            );
                        }
                        else
                        {
                            builder.WithOrigins(
                                "http://staging.palavyr.com",
                                "http://www.staging.palavyr.com",
                                "https://staging.palavyr.com",
                                "https://www.staging.palavyr.com",
                                "http://palavyr.com",
                                "http://www.palavyr.com",
                                "https://palavyr.com",
                                "https://www.palavyr.com",
                                "https://staging.widget.palavyr.com",
                                "https://widget.palavyr.com",
                                "http://staging.widget.palavyr.com",
                                "http://widget.palavyr.com"
                            );
                        }
                    });
            });

            services.AddControllers();
            var value = Configuration.GetConnectionString(_accountDbStringKey);
            services.AddDbContext<AccountsContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString(_accountDbStringKey)));
            services.AddDbContext<ConvoContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString(_convoDbStringKey)));
            services.AddDbContext<DashContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString(_configurationDbStringKey)));

            // AWS Services
            var accessKey = Configuration.GetSection(_accessKeySection).Value;
            var secretKey = Configuration.GetSection(_secretKeySection).Value;
            var awsOptions = Configuration.GetAWSOptions();
            StripeConfiguration.ApiKey = "sk_test_51HOtDQAnPqY603aZg1LhzHge6qQ7AEYcGPQhhCqMc5gXwfyr6XTEJJvJisBtzhFChIeOnytjCkhHK2ZmEgIuWyup00loOlq4W1";
            // StripeConfiguration.ApiKey = Configuration.GetSection(_StripeKeySection).Value);
            
            
            awsOptions.Credentials = new BasicAWSCredentials(accessKey, secretKey);
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonSimpleEmailService>();
            services.AddAWSService<IAmazonS3>();

            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
                if (!env.IsDevelopment())
                {
                    services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });
                }
            }
            
            services.AddHangfire(config =>
                config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseMemoryStorage());
            services.AddHangfireServer();
            // services.AddScoped<ICreatePalavyrSnapshot, CreatePalavyrSnapshot>();
            services.AddTransient<IRemoveOldS3Archives, RemoveOldS3Archives>();
            services.AddTransient<IRemoveStaleSessions, RemoveStaleSessions>();
            services.AddTransient<IValidateAttachments, ValidateAttachments>();
            
            services.AddTransient<IJwtAuthenticationService, JwtAuthenticationService>();
            services.AddTransient<IAccountSetupService, AccountSetupService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IEmailVerificationService, EmailVerificationService>();
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
            _logger.LogDebug("Starting Configure method in startup.cs");
            var appDataPath = resolveAppDataPath();
            if (string.IsNullOrEmpty(Configuration["WebRootPath"]))
                Configuration["WebRootPath"] = Environment.CurrentDirectory;

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<SetHeaders>(); // MUST come after UseAuthentication to ensure we are setting these headers on authenticated requests
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            if (env.IsProduction())
            {
                var option = new BackgroundJobServerOptions {WorkerCount = 1};
                app.UseHangfireServer(option);
                app.UseHangfireDashboard();
                _logger.LogInformation("Preparing to archive teh project");
                try
                {
                    // recurringJobManager
                    //     .AddOrUpdate(
                    //         "Create S3 Snapshot",
                    //         () => serviceProvider.GetService<ICreatePalavyrSnapshot>()
                    //             .CreateDatabaseAndUserDataSnapshot(),
                    //         Cron.Daily);
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