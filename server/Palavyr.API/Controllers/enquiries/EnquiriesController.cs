using System;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Server.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.Extensions.Logging;
using Palavyr.API.responseTypes;

namespace Palavyr.API.Controllers
{

    // [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
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
        public async Task<List<Enquiry>> GetCompletedConversations([FromHeader] string accountId)
        {
            var completedConvos = ConvoContext.CompletedConversations.ToList();
            _logger.LogInformation("0. Completed Convos: " + completedConvos[0].ToString());
            
            var Enquiries = new List<Enquiry>();
            foreach (var completedConvo in completedConvos)
            {
                var mapped = await Enquiry.MapEnquiryToResponse(_logger, completedConvo, accountId, S3Client);
                Enquiries.Add(mapped);
            }

            _logger.LogInformation("Mapped...");
            return Enquiries;
        }
        
        [HttpPut("update/{conversationId}")]
        public StatusCodeResult UpdateCompletedConversation(string conversationId)
        {
            var convo = ConvoContext.CompletedConversations.Single(row => row.ConversationId == conversationId);
            convo.Seen = !convo.Seen;
            ConvoContext.SaveChanges();
            return new OkResult();
        }
    
    }
}