#nullable enable
namespace Palavyr.Core.Sessions
{
    public interface IAccountIdTransport
    {
        public string AccountId { get; set; }
        public void Assign(string id);
    }
}