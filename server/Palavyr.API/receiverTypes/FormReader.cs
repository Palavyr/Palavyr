using Microsoft.AspNetCore.Http;

namespace Palavyr.API.ReceiverTypes
{
    public class FormReader
    {
        public string Name { get; set; }
        public IFormFile PDF { get; set; }
    }
}