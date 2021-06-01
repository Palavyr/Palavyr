using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.StripeServices;

namespace Palavyr.API.Controllers.Accounts
{
    public class EnsureDbIsValidController : PalavyrBaseController
    {
        private readonly IAccountRepository accountRepository;
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<DeleteAccountController> logger;
        private StripeCustomerService stripeCustomerService;

        public EnsureDbIsValidController(
            IAccountRepository accountRepository,
            IConfigurationRepository configurationRepository,
            ILogger<DeleteAccountController> logger,
            StripeCustomerService stripeCustomerService
        )
        {
            this.accountRepository = accountRepository;
            this.configurationRepository = configurationRepository;
            this.logger = logger;
            this.stripeCustomerService = stripeCustomerService;
        }

        [HttpPost("configure-conversations/ensure-db-valid")]
        public async Task<NoContentResult> Ensure(
            [FromHeader]
            string accountId,
            CancellationToken cancellationToken)
        {
            var preferences = await configurationRepository.GetWidgetPreferences(accountId);
            var account = await accountRepository.GetAccount(accountId, cancellationToken);

            if (string.IsNullOrWhiteSpace(account.StripeCustomerId) && account.Active)
            {
                Thread.Sleep(5000);

                var existingCustomer = await stripeCustomerService.GetCustomerByEmailAddress(account.EmailAddress, cancellationToken);
                if (existingCustomer.Count() == 0)
                {
                    var newCustomer = await stripeCustomerService.CreateNewStripeCustomer(account.EmailAddress, cancellationToken);
                    account.StripeCustomerId = newCustomer.Id;
                    await accountRepository.CommitChangesAsync();
                }
            }

            if (string.IsNullOrWhiteSpace(preferences.ChatHeader))
            {
                preferences.ChatHeader = "";
            }

            if (string.IsNullOrWhiteSpace(preferences.LandingHeader))
            {
                preferences.LandingHeader = "";
            }

            if (string.IsNullOrWhiteSpace(preferences.ButtonColor))
            {
                preferences.ButtonColor = "#F2F2F2";
            }

            if (string.IsNullOrWhiteSpace(preferences.ButtonFontColor))
            {
                preferences.ButtonFontColor = "#1F1F1F";
            }

            if (string.IsNullOrWhiteSpace(preferences.ChatBubbleColor))
                preferences.ChatBubbleColor = "#E1E1E1";

            if (string.IsNullOrWhiteSpace(preferences.ChatFontColor))
                preferences.ChatFontColor = "#35CCE6";

            if (string.IsNullOrWhiteSpace(preferences.OptionsHeaderColor))
                preferences.OptionsHeaderColor = "#35CCE6";

            if (string.IsNullOrWhiteSpace(preferences.OptionsHeaderFontColor))
                preferences.OptionsHeaderFontColor = "#35CCE6";

            if (string.IsNullOrWhiteSpace(preferences.ListFontColor))
                preferences.ListFontColor = "#35CCE6";

            if (string.IsNullOrWhiteSpace(preferences.HeaderFontColor))
                preferences.HeaderFontColor = "#35CCE6";

            if (string.IsNullOrWhiteSpace(preferences.SelectListColor))
                preferences.SelectListColor = "#E1E1E1";

            if (string.IsNullOrWhiteSpace(preferences.HeaderColor))
                preferences.HeaderColor = "#35CCE6";

            if (string.IsNullOrWhiteSpace(preferences.FontFamily))
                preferences.FontFamily = "Architects Daughter";

            var conversationNodes = await configurationRepository.GetAllConversationNodes();
            var dynamicNodeTypes = new[] {"SelectOneFlat", "PercentOfThreshold", "BasicThreshold"};
            foreach (var node in conversationNodes)
            {
                if (node.IsDynamicTableNode == null)
                {
                    if (dynamicNodeTypes.Contains(node.NodeType.Split("-").First()))
                    {
                        node.IsDynamicTableNode = true;
                    }
                }

                if (node.ResolveOrder == null)
                {
                    node.ResolveOrder = 0;
                }

                if (node.NodeComponentType == null)
                {
                    if (node.NodeType.StartsWith(nameof(SelectOneFlat)))
                    {
                        node.NodeComponentType = DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue; // should check meta, I know currently this has never been set as paths. 
                    }

                    else if (node.NodeType.StartsWith(nameof(PercentOfThreshold)))
                    {
                        node.NodeComponentType = DefaultNodeTypeOptions.NodeComponentTypes.TakeCurrency;
                    }

                    else if (node.NodeType.StartsWith(nameof(BasicThreshold)))
                    {
                        node.NodeComponentType = DefaultNodeTypeOptions.NodeComponentTypes.TakeCurrency;
                    }

                    else
                    {
                        node.NodeComponentType = node.NodeType;
                    }
                }
            }

            await configurationRepository.CommitChangesAsync();
            return NoContent();
        }
    }
}