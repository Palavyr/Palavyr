﻿// using System.Threading;
// using System.Threading.Tasks;
// using MediatR;
// using Microsoft.AspNetCore.Mvc;
// using Palavyr.Core.Handlers.ControllerHandler;
// using Palavyr.Core.Resources;
//
// namespace Palavyr.API.Controllers.Response.Tables.Dynamic
// {
//     public class CreatePricingStrategyTableController : PalavyrBaseController
//     {
//         private readonly IMediator mediator;
//         public const string Route = "tables/dynamic/{intentId}";
//
//
//         public CreatePricingStrategyTableController(IMediator mediator)
//         {
//             this.mediator = mediator;
//         }
//
//         [HttpPost(Route)]
//         public async Task<PricingStrategyTableMetaResource> Create(
//             [FromRoute]
//             string intentId,
//             CancellationToken cancellationToken)
//         {
//             // This should be part of the pricing Strategy
//             var response = await mediator.Send(new CreatePricingStrategyTableRequest<>()<(intentId), cancellationToken);
//             return response.Response;
//         }
//     }
// }