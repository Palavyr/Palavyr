using System;
using System.Collections.Generic;
using DashboardServer.Data;
using System.Linq;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.ReceiverTypes;
using Server.Domain;
using Server.Domain.Configuration.schema;


namespace Palavyr.API.Controllers
{

    // [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
    [Route("api/group")]
    [ApiController]
    public class GroupController : BaseController
    {
        public GroupController(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext, IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env) { }
        [HttpGet]
        public List<GroupMap> GetGroups([FromHeader] string accountId)
        {
            var groups = DashContext.Groups.Where(row => row.AccountId == accountId).ToList();
            return groups;
        }

        [HttpGet("{groupId}")]
        public GroupMap GetGroup([FromHeader] string accountId, string groupId)
        {
            var group = DashContext.Groups.Where(row => row.AccountId == accountId).Single(row => row.GroupId == groupId);
            return group;
        }
        
        [HttpPost]
        public List<GroupMap> CreateGroup([FromHeader] string accountId, [FromBody] Text text)
        {
            
            var groupId = Guid.NewGuid().ToString();
            var newGroup = GroupMap.CreateGroupMap(groupId, text.ParentGroup, text.GroupName, accountId);

            DashContext.Groups.Add(newGroup);
            DashContext.SaveChanges();

            return GetGroups(accountId);
        }

        [HttpDelete("{groupId}")]
        public List<GroupMap> DeleteGroup([FromHeader] string accountId, string groupId)
        {
            DashContext.Groups.Remove(DashContext.Groups.Where(row => row.AccountId == accountId).Single(row => row.GroupId == groupId));
            var areas = DashContext.Areas.Where(row => row.AccountId == accountId).Where(row => row.GroupId == groupId);
            foreach (var area in areas)
            {
                area.GroupId = null;
            }
            DashContext.Areas.UpdateRange(areas);
            DashContext.SaveChanges();
            return GetGroups(accountId);
        }

        [HttpPut("{groupId}")]
        public List<GroupMap> UpdateGroupName([FromHeader] string accountId, string groupId, [FromBody] Text text)
        {
            var currentGroup = DashContext.Groups.Where(row => row.AccountId == accountId).Single(row => row.GroupId == groupId);
            currentGroup.GroupName = text.GroupName;

            DashContext.SaveChanges();
            return GetGroups(accountId);
        }

        [HttpPut("area/{areaId}/{groupId}")]
        public List<GroupMap> UpdateAreaGroup([FromHeader] string accountId, string areaId, string groupId)
        {
            var area = DashContext.Areas.Where(row => row.AccountId == accountId).Single(row => row.AreaIdentifier == areaId);
            area.GroupId = groupId;
            DashContext.SaveChanges();
            return GetGroups(accountId);
        }

        [HttpDelete("area/{areaId}")]
        public List<GroupMap> DeleteAreaGroup([FromHeader] string accountId, string areaId)
        {
            var area = DashContext.Areas.Where(row => row.AccountId == accountId).Single(row => row.AreaIdentifier == areaId);
            area.GroupId = null;
            DashContext.SaveChanges();
            return GetGroups(accountId);
        }
        
    }    
}