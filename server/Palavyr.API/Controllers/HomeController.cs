using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Palavyr.API.Controllers
{
    public class HomeController : PalavyrBaseController
    {
        public HomeController()
        {
        }

        [AllowAnonymous]
        [HttpGet()]
        public string Get()
        {
            return "Hello from the Palavyr Api!";
        }
    }
}