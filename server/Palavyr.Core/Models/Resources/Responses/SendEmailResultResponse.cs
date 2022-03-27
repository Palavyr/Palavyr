using Palavyr.Core.Mappers;

#nullable enable
namespace Palavyr.Core.Models.Resources.Responses
{
    public class SendEmailResultResponse
    {
        public string NextNodeId { get; set; }
        public bool Result { get; set; }
        public FileAssetResource? FileAsset { get; set; }
        
        public static SendEmailResultResponse CreateSuccess(string nextNodeId, FileAssetResource? fileAsset)
        {
            return new SendEmailResultResponse
            {
                NextNodeId = nextNodeId,
                Result = true,
                FileAsset = fileAsset
            };
        }

        public static SendEmailResultResponse CreateFailure(string nextNodeId)
        {
            return new SendEmailResultResponse
            {
                NextNodeId = nextNodeId,
                Result = false
            };
        }
    }
}