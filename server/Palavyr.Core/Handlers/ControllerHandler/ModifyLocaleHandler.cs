using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyLocaleHandler : IRequestHandler<ModifyLocaleRequest, ModifyLocaleResponse>
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly ILogger<ModifyLocaleHandler> logger;
        private readonly ILocaleDefinitions localeDefinitions;

        public ModifyLocaleHandler(
            IEntityStore<Account> accountStore,
            ILogger<ModifyLocaleHandler> logger,
            ILocaleDefinitions localeDefinitions)
        {
            this.accountStore = accountStore;
            this.logger = logger;
            this.localeDefinitions = localeDefinitions;
        }

        public async Task<ModifyLocaleResponse> Handle(ModifyLocaleRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            var newLocale = localeDefinitions.Parse(request.LocaleId);

            account.Locale = newLocale.Name;
            
            var culture = new CultureInfo(newLocale.Name);

            var localeMeta = new LocaleResource
            {
                CurrentLocale = culture.ConvertToResource(),
                LocaleMap = culture.CreateLocaleMap()
            };

            return new ModifyLocaleResponse(localeMeta);
        }
    }

    public class ModifyLocaleResponse
    {
        public ModifyLocaleResponse(LocaleResource response) => Response = response;
        public LocaleResource Response { get; set; }
    }

    public class LocaleResource
    {
        public Resources.LocaleResource CurrentLocale { get; set; }
        public Resources.LocaleResource[] LocaleMap { get; set; }
    }

    public class ModifyLocaleRequest : IRequest<ModifyLocaleResponse>
    {
        public string LocaleId { get; set; }
    }
}