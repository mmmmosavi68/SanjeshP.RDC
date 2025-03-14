using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SanjeshP.RDC.Web.Controllers
{
    [AllowAnonymous]
    public class SharedErrorController : Controller
    {
        [Route("SharedError/NotFound")]
        public IActionResult NotFound()
        {
            return View();
        }

        [Route("SharedError/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("SharedError/ServerError")]
        public IActionResult ServerError()
        {
            return View();
        }
    }
}
