#nullable enable
namespace Palavyr.Core.Sessions
{
    public interface IHoldAnAccountId
    {
        public string AccountId { get; set; }
        public void Assign(string accountId);
    }
}