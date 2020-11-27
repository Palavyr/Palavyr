using Microsoft.AspNetCore.Http;

namespace Palavyr.API.RequestTypes
{
    public class FormReader
    {
        public string Name { get; set; }
        public IFormFile PDF { get; set; }
    }
}