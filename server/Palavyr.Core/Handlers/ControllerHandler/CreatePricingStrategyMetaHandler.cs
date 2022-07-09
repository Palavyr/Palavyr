// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using MediatR;
// using Palavyr.Core.Common.UniqueIdentifiers;
// using Palavyr.Core.Models.Configuration.Schemas;
// using Palavyr.Core.Models.Contracts;
// using Palavyr.Core.Resources;
// using Palavyr.Core.Resources.PricingStrategyResources;
// using Palavyr.Core.Services.PricingStrategyTableServices;
// using Palavyr.Core.Sessions;
//
// namespace Palavyr.Core.Handlers.ControllerHandler
// {
//     public class CreatePricingStrategyMetaHandler<T, TR, TCompiler>
//         : IRequestHandler<CreatePricingStrategyTableMetaRequest<T, TR, TCompiler>, CreatePricingStrategyTableMetaResponse<TR>>
//         where T : class, IPricingStrategyTable<T>, IEntity, ITable, new()
//         where TR : class, IPricingStrategyTableRowResource
//         where TCompiler : class, IPricingStrategyTableCompiler
//     {
//         private readonly IGuidUtils guidUtils;
//         private readonly IPricingStrategyTemplateCreator<T> templateCreator;
//         private readonly IAccountIdTransport accountIdTransport;
//
//         public CreatePricingStrategyMetaHandler(IGuidUtils guidUtils, IPricingStrategyTemplateCreator<T> templateCreator, IAccountIdTransport accountIdTransport)
//         {
//             this.guidUtils = guidUtils;
//             this.templateCreator = templateCreator;
//             this.accountIdTransport = accountIdTransport;
//         }
//
//         public Task<CreatePricingStrategyTableMetaResponse<TR>> Handle(CreatePricingStrategyTableRequest<T, TR, TCompiler> request, CancellationToken cancellationToken)
//         {
//             var tableId = Guid.NewGuid().ToString();
//             var tableTag = "Default-" + StaticGuidUtils.CreatePseudoRandomString(5);
//
//             var newTableMeta = DynamicTableMeta.CreateNew(
//                 tableTag,
//                 templateCreator.GetPrettyName(),
//                 templateCreator.GetTableType(),
//                 tableId,
//                 request.IntentId,
//                 accountIdTransport.AccountId,
//                 UnitIds.Currency);
//
//             return new CreatePricingStrategyTableResponse<TR>();
//         }
//     }
//
//     public class CreatePricingStrategyTableMetaResponse<TR> where TR : IPricingStrategyTableRowResource
//     {
//         public CreatePricingStrategyTableResponse(PricingStrategyTableMetaResource response) => Response = response;
//         public PricingStrategyTableMetaResource Response { get; set; }
//     }
//
//     public class CreatePricingStrategyTableMetaRequest<T, TR, TCompiler>
//         : IRequest<CreatePricingStrategyTableResponse<TR>>
//         where T : class, IPricingStrategyTable<T>, IEntity, new()
//         where TR : IPricingStrategyTableRowResource
//         where TCompiler : IPricingStrategyTableCompiler
//     {
//         
//     }
// }