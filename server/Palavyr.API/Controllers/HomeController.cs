using Microsoft.AspNetCore.Mvc;

namespace Palavyr.API.Controllers
{
    public class HomeController : PalavyrBaseController
    {
        public HomeController()
        {
        }

        [HttpGet()]
        public string Get()
        {
            return "Hello from the Palavyr Api!";
        }
    }
}