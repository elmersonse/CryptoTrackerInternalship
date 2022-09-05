using System;
using Microsoft.AspNetCore.Mvc;

namespace CryptoTracker.Controllers
{
    [Route("api/[controller]")]
    public class LayoutApiController : Controller
    {
        [HttpGet]
        public ActionResult GetUsername()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(User.Identity.Name);
            }

            return BadRequest();
        }
    }
}