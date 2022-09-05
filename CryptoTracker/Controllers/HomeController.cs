using CryptoTracker.DAL.Interfaces;
using CryptoTracker.DAL.Repositories;
using CryptoTracker.Domain.Entity;
using CryptoTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CryptoTracker.Domain.Helper;
using CryptoTracker.Domain.ViewModels;
using CryptoTracker.Service.Interfaces;

namespace CryptoTracker.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IDealService _dealService;
        private readonly IUtilityService _utilityService;

        public HomeController(IUtilityService utilityService, IDealService dealService)
        {
            _utilityService = utilityService;
            _dealService = dealService;
        }
        
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var result = await _utilityService.GetIndexValues(User.Identity.Name);
                return View(result.Data);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
