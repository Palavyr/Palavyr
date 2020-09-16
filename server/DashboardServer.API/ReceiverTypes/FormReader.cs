using Microsoft.AspNetCore.Http;

namespace DashboardServer.API.ReceiverTypes
{
    public class FormReader
    {
        public string Name { get; set; }
        public IFormFile PDF { get; set; }
    }
}