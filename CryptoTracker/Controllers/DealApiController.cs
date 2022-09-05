using System.Collections;
using System.Threading.Tasks;
using CryptoTracker.Domain.Entity;
using CryptoTracker.Domain.ViewModels.Deal;
using CryptoTracker.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoTracker.Controllers
{
    [Route("api/[controller]")]
    public class DealApiController : Controller
    {
        private readonly IUtilityService _utilityService;
        private readonly IDealService _dealService;

        public DealApiController(IDealService dealService, IUtilityService utilityService)
        {
            _utilityService = utilityService;
            _dealService = dealService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDeals()
        {
            if (User.Identity.IsAuthenticated)
            {
                var response = await _dealService.GetApi(User.Identity.Name);
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<DealViewModel>> CreateDeal([FromBody] DealViewModel deal)
        {
            if (deal == null)
            {
                return BadRequest();
            }
            var id = await _utilityService.GetUserIdByName(User.Identity?.Name);
            var res = await _dealService.CreateDeal(deal, id.Data);
            if (res.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(deal);
            }
            
            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult<DealViewModel>> UpdateDeal([FromBody] DealViewModel deal)
        {
            if (deal == null)
            {
                return BadRequest();
            }

            var id = await _utilityService.GetUserIdByName(User.Identity?.Name);
            var res = await _dealService.EditDeal(deal.Id, deal, id.Data);
            if (res.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(deal);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DealViewModel>> DeleteDeal(int id)
        {
            var res = await _dealService.DeleteDeal(id);
            if (res.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}