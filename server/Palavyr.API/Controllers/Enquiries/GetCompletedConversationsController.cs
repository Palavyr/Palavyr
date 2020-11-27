using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.responseTypes;
using Palavyr.API.Utils;

namespace Palavyr.API.Controllers.Enquiries
{
    [Route("api")]
    [ApiController]
    public class GetCompletedConversationsController : ControllerBase
    {
        private ILogger<GetCompletedConversationsController> logger;
        private ConvoContext convoContext;
        private IAmazonS3 s3Client;

        public GetCompletedConversationsController(
            ILogger<GetCompletedConversationsController> logger,
            ConvoContext convoContext,
            IAmazonS3 s3Client
        )
        {
            this.logger = logger;
            this.convoContext = convoContext;
            this.s3Client = s3Client;
        }

        [HttpGet("enquiries")]
        public async Task<Enquiry[]> Get([FromHeader] string accountId)
        {
            var enquiries = new List<Enquiry>();

            var completedConvos = convoContext
                .CompletedConversations
                .Where(row => row.AccountId == accountId)
                .ToArray();

            if (completedConvos.Count() == 0)
            {
                return enquiries.ToArray();
            }

            foreach (var completedConvo in completedConvos)
            {
                try
                {
                    var completeEnquiry =
                        await EnquiryUtils.MapEnquiryToResponse(completedConvo, accountId, s3Client, logger);
                    enquiries.Add(completeEnquiry);
                }
                catch (Exception ex)
                {
                    logger.LogDebug($"Couldn't find the file: {completedConvo.ResponsePdfId}");
                    logger.LogDebug($"Message: {ex.Message}");
                }
            }

            logger.LogDebug("Returning completed conversations...");
            return enquiries.ToArray();
        }
    }
}