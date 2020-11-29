﻿using System;
using System.Collections.Generic;
using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Accounts.Develop
{
    [Route("api")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private DashContext dashContext;

        public TestController(
            DashContext dashContext)
        {
            this.dashContext = dashContext;
        }


        [HttpPost("test")]
        public void GetTestData([FromBody] List<ConversationNode> node)
        {
        }

        [Authorize]
        [HttpGet("test")]
        public string GetTest()
        {
            var rows = dashContext.Areas.ToList();
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