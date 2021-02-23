using Microsoft.AspNetCore.Http;

namespace Palavyr.Domain.Resources.Requests
{
    public class FormReader
    {
        public string Name { get; set; }
        public IFormFile PDF { get; set; }
    }
}