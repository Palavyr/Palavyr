using System;
using System.IO;
using System.Text;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using DashboardServer.Data;
using EmailService.ResponseEmail;
using EmailService.VerificationRequest;
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
using Palavyr.Amazon.S3Services;
using Palavyr.API.CustomMiddleware;
using Palavyr.API.Response;
using Palavyr.API.Services.AccountServices;
using Palavyr.API.Services.AuthenticationServices;
using Palavyr.API.Services.DynamicTableService;
using Palavyr.API.Services.EntityServices;
using Palavyr.API.Services.StripeServices;
using Palavyr.API.Services.StripeServices.StripeWebhookHandlers;
using Palavyr.Background;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.BackupAndRestore.UserData;
using Palavyr.Common.FileSystem;
using Palavyr.Common.FileSystem.FormPaths;
using Stripe;

namespace Palavyr.API
{
    public class Startup
    {
        private const string ConfigurationDbStringKey = "DashContextPostgres";
        private const string AccountDbStringKey = "AccountsContextPostgres";
        private const string ConvoDbStringKey = "ConvoContextPostgres";
        private const string AccessKeySection = "AWS:AccessKey";
        private const string SecretKeySection = "AWS:SecretKey";
        private const string StripeKeySection = "Stripe:SecretKey";
        
        private const string WebhookKeySection = "Stripe:WebhookKey";
        
        
        private int stripeRetriesCount = 3;
        public Startup(IWebHostEnvironment Env, IConfiguration configuration)
        {
            env = Env;
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; set; }
        private ILogger<Startup> logger { get; set; }
        private IWebHostEnvironment env { get; set; }

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
                .AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthenticationHandler>(
                    AuthenticationSchemeNames.ApiKeyScheme,
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
                            builder.WithOrigins("*");
                        }
                        else
                        {
                            builder.WithOrigins(
                                "http://staging.palavyr.com",
                                "http://www.staging.palavyr.com",
                                "http://palavyr.com",
                                "http://www.palavyr.com",
                                "http://staging.widget.palavyr.com",
                                "http://widget.palavyr.com",
                                "https://staging.palavyr.com",
                                "https://www.staging.palavyr.com",
                                "https://palavyr.com",
                                "https://www.palavyr.com",
                                "https://staging.widget.palavyr.com",
                                "https://widget.palavyr.com",
                                "https://stripe.com"
                            );
                        }
                    });
            });

            services.AddControllers();
            services.AddDbContext<AccountsContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString(AccountDbStringKey)));
            services.AddDbContext<ConvoContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString(ConvoDbStringKey)));
            services.AddDbContext<DashContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString(ConfigurationDbStringKey)));

            // Stripe
            StripeConfiguration.ApiKey = Configuration.GetSection(StripeKeySection).Value;
            StripeConfiguration.MaxNetworkRetries = stripeRetriesCount;
            
            // AWS Services
            var accessKey = Configuration.GetSection(AccessKeySection).Value;
            var secretKey = Configuration.GetSection(SecretKeySection).Value;
            var awsOptions = Configuration.GetAWSOptions();
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
            services.AddTransient<IRemoveOldS3Archives, RemoveOldS3Archives>();
            services.AddTransient<IRemoveStaleSessions, RemoveStaleSessions>();
            services.AddTransient<IValidateAttachments, ValidateAttachments>();
            
            services.AddTransient<IJwtAuthenticationService, JwtAuthenticationService>();
            services.AddTransient<IAccountSetupService, AccountSetupService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IEmailVerificationService, EmailVerificationService>();
            services.AddTransient<IStripeWebhookAuthService, StripeWebhookAuthService>();
            services.AddTransient<IStripeEventWebhookService, StripeEventWebhookService>();
            services.AddTransient<IStripeCustomerService, StripeCustomerService>();
            services.AddTransient<IStripeSubscriptionService, StripeSubscriptionService>();
            services.AddTransient<IStripeProductService, StripeProductService>();
            services.AddTransient<IProcessStripeCheckoutSessionCompletedHandler, ProcessStripeCheckoutSessionCompletedHandler>();
            services.AddTransient<IProcessStripeInvoicePaidHandler, ProcessStripeInvoicePaidHandler>();
            services.AddTransient<IProcessStripeInvoicePaymentFailedHandler, ProcessStripeInvoicePaymentFailedHandler>();
            services.AddTransient<ICompileDynamicTables, CompileDynamicTables>();
            services.AddSingleton<ISesEmail, SesEmail>();
            services.AddTransient<ISenderVerification, SenderVerification>();
            services.AddTransient<IPdfResponseGenerator, PdfResponseGenerator>();
            services.AddTransient<IAccountDataService, AccountDataService>();
            services.AddTransient<IAreaDataService, AreaDataService>();
            services.AddTransient<IS3Saver, S3Saver>();
            services.AddTransient<IPostgresBackup, PostgresBackup>();
            services.AddTransient<IUserDataBackup, UserDataBackup>();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory
        )
        {
            logger = loggerFactory.CreateLogger<Startup>();
            
            var appDataPath = ResolveAppDataPath();
            if (string.IsNullOrEmpty(Configuration["WebRootPath"]))
            {
                Configuration["WebRootPath"] = Environment.CurrentDirectory;
            }

            var bucket = Configuration["Backups"];

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<SetHeaders>(); // MUST come after UseAuthentication to ensure we are setting these headers on authenticated requests
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            if (env.IsProduction())
            {
                Console.WriteLine("Current think its production Okay?");
                var option = new BackgroundJobServerOptions {WorkerCount = 1};
                app.UseHangfireServer(option);
                app.UseHangfireDashboard();
                logger.LogInformation("Preparing to archive the project");
                try
                {
                    recurringJobManager
                        .AddOrUpdate(
                            "Backup database",
                            () => serviceProvider.GetService<ICreatePalavyrSnapshot>()
                                .CreateAndTransferCompleteBackup(),
                            Cron.Daily
                        );
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
                    logger.LogCritical("ERROR " + ex.Message);
                }
            }
        }

        private string ResolveAppDataPath()
        {
            string appDataPath;
            var osVersion = Environment.OSVersion;
            if (osVersion.Platform != PlatformID.Unix)
            {
                appDataPath = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), MagicPathStrings.DataFolder);
            }
            else
            {
                var home = Environment.GetEnvironmentVariable("HOME");
                if (home == null)
                {
                    logger.LogDebug($"STARTUP-9: HOME VARIABLE NOT SET");
                }

                appDataPath = Path.Combine(home, MagicPathStrings.DataFolder);
            }
            
            DiskUtils.CreateDir(appDataPath);
            return appDataPath;
        }
    }
}