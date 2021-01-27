using Microsoft.Extensions.DependencyInjection;
using Palavyr.Background;

namespace Palavyr.API.Registration.Services
{
    public static class BackgroundServices
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ICreatePalavyrSnapshot, CreatePalavyrSnapshot>();
            services.AddTransient<IRemoveOldS3Archives, RemoveOldS3Archives>();
            services.AddTransient<IRemoveStaleSessions, RemoveStaleSessions>();
            services.AddTransient<IValidateAttachments, ValidateAttachments>();
        }
    }
}