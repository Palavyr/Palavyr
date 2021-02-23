using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Common.GlobalConstants;
using Palavyr.Common.Utils;
using Palavyr.Data;
using Palavyr.Domain;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Services.AmazonServices;

namespace Palavyr.API.Controllers.Enquiries
{
    [Route("api")]
    [ApiController]
    public class GetCompletedConversationsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private ILogger<GetCompletedConversationsController> logger;
        private ConvoContext convoContext;
        private IAmazonS3 s3Client;

        public GetCompletedConversationsController(
            IConfiguration configuration,
            ILogger<GetCompletedConversationsController> logger,
            ConvoContext convoContext,
            IAmazonS3 s3Client
        )
        {
            this.configuration = configuration;
            this.logger = logger;
            this.convoContext = convoContext;
            this.s3Client = s3Client;
        }

        [HttpGet("enquiries")]
        public async Task<Enquiry[]> Get([FromHeader] string accountId)
        {
            var previewBucket = configuration.GetSection(ConfigSections.PreviewSection).Value;

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
                        await EnquiryUtils.MapEnquiryToResponse(completedConvo, accountId, s3Client, logger, previewBucket);
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