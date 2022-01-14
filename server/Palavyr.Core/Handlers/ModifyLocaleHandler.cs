using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.Core.Handlers
{
    public class ModifyLocaleHandler : IRequestHandler<ModifyLocaleRequest, ModifyLocaleResponse>
    {
        private readonly IAccountRepository accountRepository;
        private readonly ILogger<ModifyLocaleHandler> logger;
        private readonly LocaleDefinitions localeDefinitions;

        public ModifyLocaleHandler(
            IAccountRepository accountRepository,
            ILogger<ModifyLocaleHandler> logger,
            LocaleDefinitions localeDefinitions)
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
            this.localeDefinitions = localeDefinitions;
        }

        public async Task<ModifyLocaleResponse> Handle(ModifyLocaleRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            var newLocale = localeDefinitions.Parse(request.LocaleId);

            account.Locale = newLocale.Name;
            await accountRepository.CommitChangesAsync();
            var culture = new CultureInfo(newLocale.Name);

            var localeMeta = new LocaleDetails
            {
                CurrentLocale = culture.ConvertToResource(),
                LocaleMap = culture.CreateLocaleMap()
            };

            return new ModifyLocaleResponse(localeMeta);
        }
    }

    public class ModifyLocaleResponse
    {
        public ModifyLocaleResponse(LocaleDetails response) => Response = response;
        public LocaleDetails Response { get; set; }
    }

    public class LocaleDetails
    {
        public LocaleResource CurrentLocale { get; set; }
        public LocaleResource[] LocaleMap { get; set; }
    }

    public class ModifyLocaleRequest : IRequest<ModifyLocaleResponse>
    {
        public string LocaleId { get; set; }
    }
}