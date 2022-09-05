using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTracker.Domain.ViewModels.Transaction;
using CryptoTracker.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoTracker.Controllers
{
    [Route("api/[controller]")]
    public class TransactionApiController : Controller
    {
        private readonly IUtilityService _utilityService;
        private readonly ITransactionService _transactionService;

        public TransactionApiController(IUtilityService utilityService, ITransactionService transactionService)
        {
            _utilityService = utilityService;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            if (User.Identity.IsAuthenticated)
            {
                var resp = await _transactionService.GetApi(User.Identity?.Name);
                return Ok(resp);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<TransactionViewModel>> CreateTransaction([FromBody] TransactionViewModel trans)
        {
            if (trans == null)
            {
                return BadRequest();
            }

            var id = await _utilityService.GetUserIdByName(User.Identity?.Name);
            var res = await _transactionService.CreateTransaction(trans, id.Data);
            if (res.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(trans);
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult<TransactionViewModel>> UpdateTransaction([FromBody] TransactionViewModel trans)
        {
            if (trans == null)
            {
                return BadRequest();
            }

            var id = await _utilityService.GetUserIdByName(User.Identity?.Name);
            var res = await _transactionService.EditTransaction(trans.Id, trans, id.Data);
            if (res.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(trans);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TransactionViewModel>> DeleteTransaction(int id)
        {
            var res = await _transactionService.DeleteTransaction(id);
            if (res.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok();
            }

            return BadRequest();
            
        }
    }
}