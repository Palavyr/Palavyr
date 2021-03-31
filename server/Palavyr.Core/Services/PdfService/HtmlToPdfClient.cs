using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.FileSystemTools.LocalServices;

namespace Palavyr.Core.Services.PdfService
{
    public class HtmlToPdfClient : IHtmlToPdfClient
    {
        private readonly IConfiguration configuration; // TODO: Get the pdf server ip from the configuration. Should be a separate linux server.
        private static readonly HttpClient Client = new HttpClient();

        public HtmlToPdfClient(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public async Task<string> GeneratePdfFromHtml(string htmlString, string localWriteToPath, string identifier)
        {
            var values = new Dictionary<string, string>
            {
                {LocalServices.html, htmlString.Trim()},
                {LocalServices.path, localWriteToPath},
                {LocalServices.id, identifier}
            };

            var content = new FormUrlEncodedContent(values);
            var response = await Client.PostAsync(LocalServices.PdfServiceUrl, content); // TODO: Get url from config
            var fileName = await response.Content.ReadAsStringAsync();
            return fileName;
        }
        
        // private string CreatePreviewUrlLink(string accountId, string fileId)
        // {
        //     // var host = Request.Host.Value; //TODO:  Get this from config (request.host)
        //     logger.LogDebug("Attempting to create new file URI for preview.");
        //     var builder =
        //         new UriBuilder()
        //         {
        //             // TODO: take these values from the configuration
        //             Scheme = "https", Host = "localhost", Port = 5001,
        //             Path = Path.Combine(accountId, MagicPathStrings.PreviewPDF, fileId)
        //         };
        //     logger.LogDebug($"URI used for the preview: {builder.Uri.ToString()}");
        //     return builder.Uri.ToString();
        // }

    }
}