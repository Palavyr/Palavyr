using System;
using DashboardServer.Data;
using EmailService.ResponseEmail;
using EmailService.Verification;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Palavyr.Amazon.S3Services;
using Palavyr.API.Response;
using Palavyr.API.Services.AccountServices;
using Palavyr.API.Services.AuthenticationServices;
using Palavyr.API.Services.DynamicTableService;
using Palavyr.API.Services.EntityServices;
using Palavyr.API.Services.StripeServices;
using Palavyr.API.Services.StripeServices.StripeWebhookHandlers;
using Palavyr.Background;
using Palavyr.BackupAndRestore;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.BackupAndRestore.UserData;
using Palavyr.Common.Constants;

namespace Palavyr.API.Registration.ServiceCollection
{
    public static class ServiceRegistry
    {
        public static void RegisterBackgroundServices(IServiceCollection services)
        {
            services.AddTransient<ICreatePalavyrSnapshot, CreatePalavyrSnapshot>();
            services.AddTransient<IRemoveOldS3Archives, RemoveOldS3Archives>();
            services.AddTransient<IRemoveStaleSessions, RemoveStaleSessions>();
            services.AddTransient<IValidateAttachments, ValidateAttachments>();
        }

        public static void RegisterGeneralServices(IServiceCollection services)
        {
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
            services.AddTransient<IRequestEmailVerification, RequestEmailVerification>();
            services.AddTransient<IPdfResponseGenerator, PdfResponseGenerator>();
            services.AddTransient<IAccountDataService, AccountDataService>();
            services.AddTransient<IAreaDataService, AreaDataService>();
            services.AddTransient<IS3Saver, S3Saver>();
            services.AddTransient<IPostgresBackup, PostgresBackup>();
            services.AddTransient<IUserDataBackup, UserDataBackup>();
            services.AddTransient<IUpdateDatabaseLatest, UpdateDatabaseLatest>();
        }

        public static void RegisterHangfire(IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddHangfire(
                config =>
                    config
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseMemoryStorage());

            if (env.IsProduction()) // only use hangfire server live in production
            {
                services.AddHangfireServer(
                    opt => { opt.WorkerCount = 1; });
            }
        }

        public static void RegisterIISConfiguration(IServiceCollection services, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment()) // only need IIS configured in production and staging
            {
                if (Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });
                }
            }
        }

        public static void RegisterDatabaseContexts(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AccountsContext>(
                opt =>
                    opt.UseNpgsql(configuration.GetConnectionString(ConfigSections.AccountDbStringKey)));
            services.AddDbContext<ConvoContext>(
                opt =>
                    opt.UseNpgsql(configuration.GetConnectionString(ConfigSections.ConvoDbStringKey)));
            services.AddDbContext<DashContext>(
                opt =>
                    opt.UseNpgsql(configuration.GetConnectionString(ConfigSections.ConfigurationDbStringKey)));
        }
    }
}