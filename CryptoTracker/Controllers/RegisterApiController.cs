using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;
using CryptoTracker.Domain.Entity;
using CryptoTracker.Domain.ViewModels.Account;
using CryptoTracker.Service.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace CryptoTracker.Controllers
{
    [Route("api/[controller]")]
    public class RegisterApiController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUtilityService _utilityService;

        public RegisterApiController(IAccountService accountService, IUtilityService utilityService)
        {
            _accountService = accountService;
            _utilityService = utilityService;
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resp = await _accountService.Check(model);
                if (resp.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    var url = Url.Action(
                        "ConfirmEmail",
                        "RegisterApi",
                        new {name = model.Name, email = model.Email, password = model.Password},
                        protocol: HttpContext.Request.Scheme);
                    await _utilityService.SendEmail(model.Email,
                        $"Confirm your email by clicking the link below.\n <a href='{url}'>Click me!</a>");
                    return Ok("Check email for confirmation link");
                }
            }
            
            return Ok("Passwords don't match");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string name, string email, string password)
        {
            var response = await _accountService.Register(new RegisterViewModel
            {
                Email = email,
                Name = name,
                Password = password,
                PasswordConfirm = password
            });
            
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(response.Data));
            }

            return RedirectToAction("Index", "Home");
        }
    }
}