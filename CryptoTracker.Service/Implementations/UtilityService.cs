using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CryptoTracker.DAL.Interfaces;
using CryptoTracker.DAL.Repositories;
using CryptoTracker.Domain.Entity;
using CryptoTracker.Domain.Enum;
using CryptoTracker.Domain.Response;
using CryptoTracker.Domain.Utility;
using CryptoTracker.Domain.ViewModels.Home;
using CryptoTracker.Domain.ViewModels.Transaction;
using CryptoTracker.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;


namespace CryptoTracker.Service.Implementations
{
    public class UtilityService : IUtilityService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Deal> _dealRepository;
        private readonly IBaseRepository<Transaction> _transactionRepository;

        public UtilityService(IBaseRepository<User> userRepository, IBaseRepository<Deal> dealRepository, IBaseRepository<Transaction> transactionRepository)
        {
            _userRepository = userRepository;
            _dealRepository = dealRepository;
            _transactionRepository = transactionRepository;
        }

        public BaseResponse<Dictionary<int, string>> GetCurrencies()
        {
            try
            {
                var currencies = ((Currency[]) Enum.GetValues(typeof(Currency)))
                    .ToDictionary(k => (int) k, t => t.GetDisplayName());

                return new BaseResponse<Dictionary<int, string>>()
                {
                    Data = currencies,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<int, string>>()
                {
                    Description = $"[GetCurrencies] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public BaseResponse<Dictionary<int, string>> GetDealTypes()
        {
            try
            {
                var currencies = ((DealType[]) Enum.GetValues(typeof(DealType)))
                    .ToDictionary(k => (int) k, t => t.GetDisplayName());

                return new BaseResponse<Dictionary<int, string>>()
                {
                    Data = currencies,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<int, string>>()
                {
                    Description = $"[GetDealTypes] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }public BaseResponse<Dictionary<int, string>> GetTransactionTypes()
        {
            try
            {
                var currencies = ((TransactionType[]) Enum.GetValues(typeof(TransactionType)))
                    .ToDictionary(k => (int) k, t => t.GetDisplayName());

                return new BaseResponse<Dictionary<int, string>>()
                {
                    Data = currencies,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<int, string>>()
                {
                    Description = $"[GetTransactionTypes] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<int>> GetUserIdByName(string name)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == name);
                if (user.Id == 0)
                {
                    return new BaseResponse<int>()
                    {
                        Description = "User not found",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }
                
                return new BaseResponse<int>()
                {
                    Data = user.Id,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>()
                {
                    Description = $"[GetUserIdByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<List<HomeViewModel>>> GetIndexValues(string name)
        {
            try
            {
                int id = GetUserIdByName(name).Result.Data;
                var deals = await _dealRepository.GetAll().Where(d => d.UserId == id).ToListAsync();
                //var trans = await _transactionRepository.GetAll().Where(d => d.UserId == id).ToListAsync();

                var res = new List<HomeViewModel>();
                var names = GetCurrencies().Data.Values.ToList();
                Currency[] currs = (Currency[])Enum.GetValues(typeof(Currency));
                for (int i = 0; i < names.Count; i++)
                {
                    var tdeals = deals.Where(d => d.Currency == currs[i]);
                    //var ttrans = trans.Where(t => t.Currency == currs[i]);
                    var sum = tdeals.Where(d => d.DealType != DealType.Sell).Sum(d => d.Amount) -
                              tdeals.Where(d => d.DealType == DealType.Sell).Sum(d => d.Amount);
                              // +
                              // ttrans.Where(t => t.TransactionType == TransactionType.Get).Sum(t => t.Amount) -
                              // ttrans.Where(t => t.TransactionType == TransactionType.Send).Sum(t => t.Amount);
                    var sumUsd = tdeals.Where(d => d.DealType != DealType.Sell).Sum(d => d.Amount * d.Rate) -
                                 tdeals.Where(d => d.DealType == DealType.Sell).Sum(d => d.Amount * d.Rate);
                    res.Add(new HomeViewModel
                    {
                        Name = names[i],
                        ShortName = currs[i].ToString(),
                        Value = sum,
                        BoughtUsd = sumUsd
                    });
                }

                return new BaseResponse<List<HomeViewModel>>()
                {
                    Data = res,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<HomeViewModel>>()
                {
                    Description = $"[GetIndexValues] {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task SendEmail(string email, string message)
        {
            var mes = new MimeMessage();
            
            mes.From.Add(new MailboxAddress("CryptoTracker", "crypto.tracker@inbox.ru"));
            mes.To.Add(new MailboxAddress("", email));
            mes.Subject = "E-mail verification";
            mes.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465, true);
                
                await client.AuthenticateAsync("crypto.tracker@inbox.ru", "msjVXfajRYdtdiMWXJ0X");
                await client.SendAsync(mes);
                await client.DisconnectAsync(true);
            }
        }
    }
}