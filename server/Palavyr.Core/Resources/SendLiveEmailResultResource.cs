using Palavyr.Core.Mappers;

#nullable enable
namespace Palavyr.Core.Resources.Responses
{
    public class SendLiveEmailResultResource
    {
        public string NextNodeId { get; set; }
        public bool Result { get; set; }
        public FileAssetResource? FileAsset { get; set; }
        
        public static SendLiveEmailResultResource CreateSuccess(string nextNodeId, FileAssetResource? fileAsset)
        {
            return new SendLiveEmailResultResource
            {
                NextNodeId = nextNodeId,
                Result = true,
                FileAsset = fileAsset
            };
        }

        public static SendLiveEmailResultResource CreateFailure(string nextNodeId)
        {
            return new SendLiveEmailResultResource
            {
                NextNodeId = nextNodeId,
                Result = false
            };
        }
    }
}