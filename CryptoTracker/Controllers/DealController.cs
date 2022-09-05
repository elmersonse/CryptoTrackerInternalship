using CryptoTracker.DAL.Interfaces;
using CryptoTracker.Domain.Entity;
using CryptoTracker.Domain.Response;
using CryptoTracker.Domain.Enum;
using CryptoTracker.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CryptoTracker.Domain.ViewModels.Deal;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CryptoTracker.Controllers
{
    public class DealController : Controller
    {
        private readonly IDealService _dealService;
        private readonly IUtilityService _utilityService;

        public DealController(IDealService dealService, IUtilityService utilityService)
        {
            _dealService = dealService;
            _utilityService = utilityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDeals(string name)
        {
            ViewBag.Currencies = new SelectList(_utilityService.GetCurrencies().Data, "Key", "Value");
            ViewBag.DealTypes = new SelectList(_utilityService.GetDealTypes().Data, "Key", "Value");
            var response = await _dealService.GetDealsByUserName(name);
            if (response.StatusCode == Domain.Enum.StatusCode.OK || response.StatusCode == Domain.Enum.StatusCode.ObjectNotFound)
            {
                return View(response.Data);
            }
        
            return RedirectToAction("Error", "Home");
        }
    }
}
