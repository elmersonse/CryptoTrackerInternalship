using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTracker.Domain.Entity;
using CryptoTracker.Domain.Enum;
using CryptoTracker.Domain.Response;
using CryptoTracker.Domain.ViewModels.Transaction;

namespace CryptoTracker.Service.Interfaces
{
    public interface ITransactionService
    {
        Task<IBaseResponse<bool>> CreateTransaction(TransactionViewModel getTransactionViewModel, int userId);
        Task<IBaseResponse<bool>> DeleteTransaction(int id);
        Task<IBaseResponse<Transaction>> GetTransaction(int id);
        Task<IBaseResponse<Transaction>> EditTransaction(int id, TransactionViewModel model, int userId);
        Task<IBaseResponse<IEnumerable<Transaction>>> GetTransactions();
        Task<IBaseResponse<IEnumerable<Transaction>>> GetTransactionsByCurrency(Currency currency);
        Task<IBaseResponse<IEnumerable<Transaction>>> GetTransactionsByUserName(string name);
        Task<List<TransactionApi>> GetApi(string name);
    }
}