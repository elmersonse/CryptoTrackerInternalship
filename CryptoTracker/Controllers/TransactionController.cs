using System.Threading.Tasks;
using CryptoTracker.Domain.ViewModels.Transaction;
using CryptoTracker.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CryptoTracker.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IUtilityService _utilityService;

        public TransactionController(ITransactionService transactionService, IUtilityService utilityService)
        {
            _transactionService = transactionService;
            _utilityService = utilityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions(string name)
        {
            ViewBag.Currencies = new SelectList(_utilityService.GetCurrencies().Data, "Key", "Value");
            ViewBag.TransactionTypes = new SelectList(_utilityService.GetTransactionTypes().Data, "Key", "Value");
            var response = await _transactionService.GetTransactionsByUserName(name);
            if(response.StatusCode == Domain.Enum.StatusCode.OK || response.StatusCode == Domain.Enum.StatusCode.ObjectNotFound)
            {
                return View(response.Data);
            }
            return RedirectToAction("Error", "Home");
        }
    }
}