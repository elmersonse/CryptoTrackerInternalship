using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoTracker.DAL.Interfaces;
using CryptoTracker.Domain.Entity;
using CryptoTracker.Domain.Enum;
using CryptoTracker.Domain.Response;
using CryptoTracker.Domain.Utility;
using CryptoTracker.Domain.ViewModels.Transaction;
using CryptoTracker.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptoTracker.Service.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly IBaseRepository<Transaction> _transactionRepository;

        public TransactionService(IBaseRepository<Transaction> transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IBaseResponse<bool>> CreateTransaction(TransactionViewModel transactionViewModel, int userId)
        {
            try
            {
                var transaction = new Transaction()
                {
                    Currency = (Currency)Convert.ToInt32(transactionViewModel.Currency),
                    Wallet = transactionViewModel.Wallet,
                    Amount = transactionViewModel.Amount,
                    Commission = transactionViewModel.Commission,
                    TransactionType = (TransactionType)Convert.ToInt32(transactionViewModel.TransactionType),
                    Commentary = transactionViewModel.Commentary,
                    Date = DateTime.Today,
                    UserId = userId
                };
                await _transactionRepository.Create(transaction);
                
                return new BaseResponse<bool>
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[CreateTransaction] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteTransaction(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var transaction = await _transactionRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (transaction == null)
                {
                    baseResponse.Description = "Object not found";
                    baseResponse.StatusCode = Domain.Enum.StatusCode.ObjectNotFound;
                    baseResponse.Data = false;
                    return baseResponse;
                }
                await _transactionRepository.Delete(transaction);
                baseResponse.Data = true;
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteTransaction] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Transaction>> GetTransaction(int id)
        {
            var baseResponse = new BaseResponse<Transaction>();
            try
            {
                var transaction = await _transactionRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (transaction == null)
                {
                    baseResponse.Description = "Object not found";
                    baseResponse.StatusCode = Domain.Enum.StatusCode.ObjectNotFound;
                    return baseResponse;
                }
                baseResponse.Data = transaction;
                baseResponse.StatusCode = Domain.Enum.StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Transaction>()
                {
                    Description = $"[GetTransaction] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Transaction>> EditTransaction(int id, TransactionViewModel model, int userId)
        {
            try
            {
                var tran = await _transactionRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (tran == null)
                {
                    return new BaseResponse<Transaction>()
                    {
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }

                tran.Amount = model.Amount;
                tran.Currency = (Currency)Convert.ToInt32(model.Currency);
                tran.Wallet = model.Wallet ?? "unknown";
                tran.Commentary = model.Commentary;
                tran.UserId = userId;
                tran.TransactionType = (TransactionType) Convert.ToInt32(model.TransactionType);
                tran.Commission = model.Commission;
                await _transactionRepository.Update(tran);
                return new BaseResponse<Transaction>()
                {
                    Data = tran,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Transaction>()
                {
                    Description = $"[EditTransaction] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Transaction>>> GetTransactions()
        {
            var baseResponse = new BaseResponse<IEnumerable<Transaction>>();
            try
            {
                var transactions = await _transactionRepository.GetAll().ToListAsync();
                if (transactions.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = Domain.Enum.StatusCode.ObjectNotFound;
                    return baseResponse;
                }
                baseResponse.Data = transactions;
                baseResponse.StatusCode = Domain.Enum.StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Transaction>>()
                {
                    Description = $"[GetTransactions] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Transaction>>> GetTransactionsByCurrency(Currency currency)
        {
            var baseResponse = new BaseResponse<IEnumerable<Transaction>>();
            try
            {
                var transactions = await _transactionRepository.GetAll().Where(x => x.Currency == currency).ToListAsync();
                if (transactions.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = Domain.Enum.StatusCode.ObjectNotFound;
                    return baseResponse;
                }
                baseResponse.Data = transactions;
                baseResponse.StatusCode = Domain.Enum.StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Transaction>>()
                {
                    Description = $"[GetTransactionsByCurrency] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Transaction>>> GetTransactionsByUserName(string name)
        {
            var baseResponse = new BaseResponse<IEnumerable<Transaction>>();
            try
            {
                var transactions = await _transactionRepository.GetAll()
                    .Include(x => x.User)
                    .Where(x => x.User.Name == name)
                    .ToListAsync();
                if (transactions.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = Domain.Enum.StatusCode.ObjectNotFound;
                    return baseResponse;
                }
                baseResponse.Data = transactions;
                baseResponse.StatusCode = Domain.Enum.StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Transaction>>()
                {
                    Description = $"[GetTransactionsByUserId] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<List<TransactionApi>> GetApi(string name)
        {
            var trans = await _transactionRepository.GetAll().Include(i => i.User).Where(x => x.User.Name == name).ToListAsync();
            var tr = new List<TransactionApi>();
            foreach (var t in trans)
            {
                tr.Add(new TransactionApi
                {
                    Amount = t.Amount,
                    Commentary = t.Commentary,
                    Commission = t.Commission,
                    Currency = t.Currency.ToString(),
                    Wallet = t.Wallet,
                    FullName = t.Currency.GetDisplayName(),
                    Id = t.Id,
                    UserId = t.UserId,
                    Date = t.Date.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString(),
                    TransactionType = t.TransactionType.ToString()
                });
            }

            return tr;
        }
    }
}