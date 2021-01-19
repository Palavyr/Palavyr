using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Palavyr.FileSystem.UIDUtils;

namespace Palavyr.Background
{
    public class CreatePalavyrSnapshot  //: ICreatePalavyrSnapshot
    {
        // // private readonly string DatabaseDirectory = $"C:\\{Utils.PalavyrData}\\AppData";
        // private readonly string UserDataDirectory = $"C:\\{Utils.PalavyrData}\\UserData";
        // // private readonly string Archives = $"C:\\{Utils.PalavyrData}\\Archives";
        //
        // private IAmazonS3 S3Client { get; }
        // private readonly ILogger<CreatePalavyrSnapshot> logger;        
        //
        // public CreatePalavyrSnapshot(IAmazonS3 s3Client, ILogger<CreatePalavyrSnapshot> logger)
        // {
        //     S3Client = s3Client;
        //     this.logger = logger;
        //     if (!Directory.Exists(Archives))
        //         Directory.CreateDirectory(Archives);
        // }
        //
        // public async Task CreateDatabaseAndUserDataSnapshot()
        // {
        //     var snapshotTimeStamp = DateTime.Now.ToString(TimeUtils.DateTimeFormat);
        //     // await SaveSnapshot(Utils.Databases, snapshotTimeStamp, DatabaseDirectory);
        //     await SaveSnapshot(Utils.UserData, snapshotTimeStamp, UserDataDirectory);
        // }
        //
        // private async Task SaveSnapshot(string prefix, string snapshotTimeStamp, string fromDirectory)
        // {
        //     var snapshotName = $"{prefix}.{snapshotTimeStamp}.zip";
        //     var zipName = $"C:\\{Utils.PalavyrData}\\Archives\\{snapshotName}";
        //     ZipFile.CreateFromDirectory(fromDirectory, zipName);
        //     await SaveZipToS3(zipName, snapshotTimeStamp, snapshotName);
        // }
        //
        // private static void DeleteLocalTempArchive(string archivePath)
        // {
        //     if (File.Exists(archivePath))
        //     {
        //         File.Delete(archivePath);
        //     }
        // }
        //
        // private async Task SaveZipToS3(string zipPath, string snapshotTimeStamp, string snapshotName)
        // {
        //     
        //     var fileKey = Path.Combine(Utils.SnapshotsDir, snapshotTimeStamp, snapshotName).Replace("\\", "/");
        //     var putRequest = new PutObjectRequest()
        //     {
        //         BucketName = Utils.ArchivesBucket,
        //         FilePath = zipPath,
        //         Key = fileKey
        //     };
        //     try
        //     {
        //         var response = await S3Client.PutObjectAsync(putRequest);
        //         logger.LogInformation($"Saved {zipPath} to {fileKey} in {Utils.ArchivesBucket}");
        //     }
        //     catch (Exception ex)
        //     {
        //         logger.LogInformation("Failed to write snapshot files: " + ex.Message);
        //         Console.WriteLine(ex);
        //         DeleteLocalTempArchive(zipPath);
        //     }
        // }
    }
}