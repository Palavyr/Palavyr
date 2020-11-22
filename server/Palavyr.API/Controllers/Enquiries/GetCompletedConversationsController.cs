using System;
using Microsoft.AspNetCore.Mvc;
using DashboardServer.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.Extensions.Logging;
using Palavyr.API.responseTypes;

namespace Palavyr.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class GetCompletedConversationsController : ControllerBase
    {
        private ILogger<GetCompletedConversationsController> logger;
        private ConvoContext convoContext;
        private AmazonS3Client s3Client;

        public GetCompletedConversationsController(
            ILogger<GetCompletedConversationsController> logger,
            ConvoContext convoContext,
            AmazonS3Client s3Client
            )
        {
            this.logger = logger;
            this.convoContext = convoContext;
            this.s3Client = s3Client;
        }

        [HttpGet("enquiries")]
        public async Task<IActionResult> Get([FromHeader] string accountId)
        {
            var Enquiries = new List<Enquiry>();

            var completedConvos = convoContext.CompletedConversations.Where(row => row.AccountId == accountId).ToList();
            if (completedConvos.Count() == 0)
            {
                return Ok(Enquiries);
            }

            foreach (var completedConvo in completedConvos)
            {
                try
                {
                    var completeEnquiry =
                        await Enquiry.MapEnquiryToResponse(logger, completedConvo, accountId, s3Client);
                    Enquiries.Add(completeEnquiry);
                }
                catch (Exception ex)
                {
                    logger.LogDebug($"Couldn't find the file: {completedConvo.ResponsePdfId}");
                    logger.LogDebug($"Message: {ex.Message}");
                }
            }

            logger.LogDebug("Returning completed conversations...");
            return Ok(Enquiries);
        }
    }
}