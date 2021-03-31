using System.Threading.Tasks;

namespace Palavyr.BackupAndRestore
{
    public interface ICreatePalavyrSnapshot
    {
        Task CreateAndTransferCompleteBackup();
    }
}