#nullable enable
namespace Palavyr.Core.Sessions
{
    public interface IAccountIdTransport
    {
        string AccountId { get; set; }
        void Assign(string id);
        bool IsSet();
    }
}