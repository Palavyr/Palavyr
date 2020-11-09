// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net.Http;
// using System.Threading.Tasks;
// using Amazon.SimpleEmail;
// using Amazon.SimpleEmail.Model;
// using DashboardServer.Data;
// using EmailService.verification;
// using Google.Apis.Util;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Configuration;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using Palavyr.API.receiverTypes;
// using Palavyr.API.response;
// using Newtonsoft.Json;
// using Stripe;
// using Stripe.Checkout;
//
//
// namespace Palavyr.API.Controllers.Payments
// {
//     public class ConfirmationRequest
//     {
//         public string confirmationId { get; set; }
//         public string SessionId { get; set; }
//     }
//     
//     [Route("api/payments")]
//     [ApiController]
//     public class PaymentConfirmation : BaseController
//     {
//         private static ILogger<PaymentConfirmation> _logger;
//
//         public PaymentConfirmation(
//             ILogger<PaymentConfirmation> logger,
//             AccountsContext accountContext,
//             ConvoContext convoContext,
//             DashContext dashContext,
//             IWebHostEnvironment env
//         ) : base(accountContext, convoContext, dashContext, env)
//         {
//             _logger = logger;
//         }
//
//         [HttpPost("confirmation")]
//         public IActionResult Confirmation([FromHeader] string accountId, ConfirmationRequest request)
//         {
//             Console.WriteLine(request);
//
//             var sessionId = request.SessionId;
//             var session = AccountContext.Sessions.SingleOrDefault(row => row.SessionId == sessionId);
//             if (session == null) throw new Exception("Session token not found!");
//             
//             var accountId = 
//             
//             return Ok();
//         }
//         
//     }
// }