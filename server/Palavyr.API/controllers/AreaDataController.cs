using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.API.ReceiverTypes;
using Server.Domain.Configuration.schema;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using EmailService;
using Microsoft.EntityFrameworkCore;

///https://docs.aws.amazon.com/sdkfornet/v3/apidocs/index.html
/// The verification status of an email address is "Pending" until the email address owner clicks the link within the verification email that Amazon SES sent to that address. If the email address owner clicks the link within 24 hours, the verification status of the email address changes to "Success". If the link is not clicked within 24 hours, the verification status changes to "Failed." In that case, if you still want to verify the email address, you must restart the verification process from the beginning.

namespace Palavyr.API.Controllers
{
    [Route("api/areas")]
    [ApiController]
    public class AreaDataController : BaseController
    {
        private static ILogger<AreaDataController> _logger;
        private IAmazonSimpleEmailService _client { get; set; }

        private const string Pending = "Pending";
        private const string Success = "Success";
        private const string Failed = "Failed";

        public AreaDataController(
            IAmazonSimpleEmailService client,
            ILogger<AreaDataController> logger,
            AccountsContext accountContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public IQueryable<Area> GetAllAreas([FromHeader] string accountId)
        {
            var area = DashContext.Set<Area>().Where(row => row.AccountId == accountId);
            return area;
        }

        [HttpGet("{areaId}")]
        public async Task<Area> GetAreaById([FromHeader] string accountId, string areaId)
        {
            // check if email is verified, if it is not,
            // look up from AWS if email is under verifiection and
            // set the appropriate  (unmapped property) property

            var data = DashContext.Areas.Where(row => row.AccountId == accountId)
                .Single(row => row.AreaIdentifier == areaId);

            // if area email not verified, check verification status and attach to response
            if (data.EmailIsVerified)
            {
                data.AwaitingVerification = false;
                return data;
            }

            if (string.IsNullOrWhiteSpace(data.AreaSpecificEmail))
            {
                // TODO: rethink this -- on account setup and new area creation, we should automatically have 
                // set the area emails to the default email. This is a safeguard measure to ensure we don't 
                // try to query a null or empty email to aws
                var account = await AccountContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
                if (account == null) throw new NullReferenceException("Account doesnt exist: Area Data Controller");
                data.AreaSpecificEmail = account.EmailAddress;
                // await DashContext.SaveChangesAsync();
            }
            var identityRequest = new GetIdentityVerificationAttributesRequest()
            {
                Identities = new List<string>() {data.AreaSpecificEmail}
            };
            GetIdentityVerificationAttributesResponse response;
            try
            {
                response = await _client.GetIdentityVerificationAttributesAsync(identityRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve AWS identities: {ex.Message}");
            }
            var found = response.VerificationAttributes.TryGetValue(data.AreaSpecificEmail, out var status);
            if (found)
            {
                switch (status.VerificationStatus.Value)
                {
                    case (Pending):
                        data.AwaitingVerification = true;
                        data.EmailIsVerified = false;
                        break;

                    case (Failed):
                        data.AwaitingVerification = false;
                        data.EmailIsVerified = false;
                        break;

                    case (Success):
                        data.AwaitingVerification = false;
                        data.EmailIsVerified = true;
                        break;
                }
            }
            else
            {
                data.AwaitingVerification = false;
                data.EmailIsVerified = false;
            }
            await DashContext.SaveChangesAsync();
            return data;
        }

        [HttpPost("create")]
        public Area CreateArea([FromHeader] string accountId, [FromBody] Text text)
        {
            var account = AccountContext.Accounts.SingleOrDefault(row => row.AccountId == accountId);
            if (account == null) throw new Exception();
            var defaultEmail = account.EmailAddress;
            var isVerified = account.DefaultEmailIsVerified;

            var defaultAreaTemplate = Area.CreateNewArea(text.AreaName, accountId, defaultEmail, isVerified);
            var result = DashContext.Areas.Add(defaultAreaTemplate);
            DashContext.SaveChanges();
            return result.Entity;
        }

        [HttpPut("update/{areaId}")]
        public StatusCodeResult UpdateArea([FromHeader] string accountId, [FromBody] Text text, string areaId)
        {
            var newAreaName = text.AreaName;
            var newAreaDisplayTitle = text.AreaDisplayTitle;

            var curArea = DashContext.Areas.Where(row => row.AccountId == accountId)
                .Single(row => row.AreaIdentifier == areaId);

            if (text.AreaName != null)
                curArea.AreaName = newAreaName;
            if (text.AreaDisplayTitle != null)
                curArea.AreaDisplayTitle = newAreaDisplayTitle;
            DashContext.SaveChanges();
            return new OkResult();
        }

        [HttpDelete("delete/{areaId}")]
        public StatusCodeResult DeleteArea([FromHeader] string accountId, string areaId)
        {
            DashContext.Areas.RemoveRange(DashContext.Areas.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.ConversationNodes.RemoveRange(DashContext.ConversationNodes.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.DynamicTableMetas.RemoveRange(DashContext.DynamicTableMetas.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.FileNameMaps.RemoveRange(DashContext.FileNameMaps.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.SelectOneFlats.RemoveRange(DashContext.SelectOneFlats.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.StaticFees.RemoveRange(DashContext.StaticFees.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.StaticTablesMetas.RemoveRange(DashContext.StaticTablesMetas.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.StaticTablesRows.RemoveRange(DashContext.StaticTablesRows.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));

            try
            {
                DiskUtils.DeleteAreaFolder(accountId, areaId);
                DashContext.SaveChanges();
            }
            catch
            {
                _logger.LogCritical($"Area Data NOT Deleted.");
                _logger.LogCritical($"Unable to delete the area folder for {accountId} under areaId {areaId}.");
            }

            return new OkResult();
        }
    }
}