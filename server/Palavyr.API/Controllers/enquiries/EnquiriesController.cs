using Microsoft.AspNetCore.Mvc;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.Extensions.Logging;
using Palavyr.API.responseTypes;

namespace Palavyr.API.Controllers
{

    [Route("api/enquiries")]
    [ApiController]
    public class EnquiriesController : BaseController
    {

        private IAmazonS3 S3Client { get; set; }
        private static ILogger<EnquiriesController> _logger;

        public EnquiriesController(
            ILogger<EnquiriesController> logger,
            IAmazonS3 s3Client, 
            AccountsContext accountContext, 
            ConvoContext convoContext,
            DashContext dashContext, 
            IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env)
        {
            S3Client = s3Client;
            _logger = logger;
        }
        
        
        [HttpGet]
        public async Task<IActionResult> GetCompletedConversations([FromHeader] string accountId)
        {
            var Enquiries = new List<Enquiry>();
            
            var completedConvos = ConvoContext.CompletedConversations.ToList();
            if (completedConvos.Count() == 0)
            {
                return Ok(Enquiries);
            }
            
            foreach (var completedConvo in completedConvos)
            {
                var mapped = await Enquiry.MapEnquiryToResponse(_logger, completedConvo, accountId, S3Client);
                Enquiries.Add(mapped);
            }

            _logger.LogDebug("Returning completed conversations...");
            return Ok(Enquiries);
        }
        
        [HttpPut("update/{conversationId}")]
        public IActionResult UpdateCompletedConversation(string conversationId)
        {
            var convo = ConvoContext.CompletedConversations.Single(row => row.ConversationId == conversationId);
            convo.Seen = !convo.Seen;
            ConvoContext.SaveChanges();
            return Ok();
        }
    
    }
}