using System.Threading.Tasks;
using CryptoTracker.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoTracker.Controllers
{
    
    [Route("api/[controller]")]
    public class MainPageController : Controller
    {
        private readonly IUtilityService _utilityService;
        private readonly IAccountService _accountService;

        public MainPageController(IUtilityService utilityService, IAccountService accountService)
        {
            _utilityService = utilityService;
            _accountService = accountService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetIndex()
        {
            if (User.Identity.IsAuthenticated)
            {
                var result = await _utilityService.GetIndexValues(User.Identity.Name);
                return Ok(result.Data);
            }

            return Ok();
        }
    
        [HttpPost]
        public async Task<ActionResult> SetProfit([FromBody]float profit)
        {
            var res = await _accountService.UpdateProfit(profit, User.Identity.Name);
            if (res.StatusCode != Domain.Enum.StatusCode.OK)
            {
                return BadRequest();
            }

            return Ok(res.Data);
        }
    }
}