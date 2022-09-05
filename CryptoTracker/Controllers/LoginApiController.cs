using System.Security.Claims;
using System.Threading.Tasks;
using CryptoTracker.Domain.ViewModels.Account;
using CryptoTracker.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CryptoTracker.Controllers
{
    [Route("api/[controller]")]
    public class LoginApiController : Controller
    {
        private readonly IAccountService _accountService;

        public LoginApiController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.Login(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));
                    return Ok("Done");

                }
                return Ok(response.Description);
            }

            return BadRequest(model);
        }
    }
}