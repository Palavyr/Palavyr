using System.Threading.Tasks;

namespace Palavyr.Background
{
    public interface ICreatePalavyrSnapshot
    {
        Task CreateAndTransferCompleteBackup();
    }
}