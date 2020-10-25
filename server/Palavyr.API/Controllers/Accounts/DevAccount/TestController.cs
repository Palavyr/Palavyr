using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    [Route("api/test/")]
    [ApiController]
    public class TestController : BaseController
    {
        
        public TestController(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext, IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env) { }


        [HttpPost]
        public void GetTestData([FromBody] List<ConversationNode> node)
        {
                 
        }

        [Authorize]
        [HttpGet]
        public string GetTest()
        {
            var rows = DashContext.Areas.ToList();
            foreach (var row in rows)
            {
                Console.WriteLine(row.ToString());
            }
            return "This is a test";
        }

        [HttpGet("{id}")]
        public void TestEndpoint(string id)
        {
            Console.WriteLine("This is a silly test.");
            Console.WriteLine(id);
        }
    }
}