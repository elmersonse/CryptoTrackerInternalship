using CryptoTracker.DAL.Interfaces;
using CryptoTracker.Domain.Entity;
using CryptoTracker.Domain.Enum;
using CryptoTracker.Domain.Response;
using CryptoTracker.Domain.ViewModels.Deal;
using CryptoTracker.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoTracker.Domain.Utility;

namespace CryptoTracker.Service.Implementations
{
    public class DealService : IDealService
    {
        private readonly IBaseRepository<Deal> _dealRepository;

        public DealService(IBaseRepository<Deal> dealRepository)
        {
            _dealRepository = dealRepository;
        }

        public async Task<IBaseResponse<bool>> CreateDeal(DealViewModel model, int userId)
        {
            try
            {
                var deal = new Deal()
                {
                    Currency = (Currency)Convert.ToInt32(model.Currency),
                    Amount = model.Amount,
                    Rate = model.Rate,
                    DealType = (DealType)Convert.ToInt32(model.DealType),
                    Commentary = model.Commentary,
                    Date = DateTime.Today,
                    UserId = userId
                };
                await _dealRepository.Create(deal);
                
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[CreateDeal] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteDeal(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var deal = await _dealRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if(deal == null)
                {
                    baseResponse.Description = "Object not found";
                    baseResponse.StatusCode = Domain.Enum.StatusCode.ObjectNotFound;
                    baseResponse.Data = false;
                    return baseResponse;
                }
                await _dealRepository.Delete(deal);
                baseResponse.Data = true;
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteDeal] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Deal>> GetDeal(int id)
        {
            var baseResponse = new BaseResponse<Deal>();
            try
            {
                var deal = await _dealRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (deal == null)
                {
                    baseResponse.Description = "Object not found";
                    baseResponse.StatusCode = Domain.Enum.StatusCode.ObjectNotFound;
                    return baseResponse;
                }
                baseResponse.Data = deal;
                baseResponse.StatusCode = Domain.Enum.StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Deal>()
                {
                    Description = $"[GetDeal] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Deal>> EditDeal(int id, DealViewModel model, int userId)
        {
            var baseResponse = new BaseResponse<Deal>();
            try
            {
                var deal = await _dealRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (deal == null)
                {
                    baseResponse.StatusCode = Domain.Enum.StatusCode.ObjectNotFound;
                    return baseResponse;
                }
                deal.Amount = model.Amount;
                deal.Rate = model.Rate;
                deal.UserId = userId;
                deal.DealType = (DealType)Convert.ToInt32(model.DealType);
                deal.Currency = (Currency)Convert.ToInt32(model.Currency);
                deal.Commentary = model.Commentary;
                await _dealRepository.Update(deal);
                baseResponse.StatusCode = Domain.Enum.StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Deal>()
                {
                    Description = $"[EditDeal] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Deal>>> GetDeals()
        {
            var baseResponse = new BaseResponse<IEnumerable<Deal>>();
            try
            {
                var deals = await _dealRepository.GetAll().ToListAsync();
                if(deals.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = Domain.Enum.StatusCode.ObjectNotFound;
                    return baseResponse;
                }
                baseResponse.Data = deals;
                baseResponse.StatusCode = Domain.Enum.StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Deal>>()
                {
                    Description = $"[GetDeals] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Deal>>> GetDealsByCurrency(Currency currency)
        {
            var baseResponse = new BaseResponse<IEnumerable<Deal>>();
            try
            {
                var deals = await _dealRepository.GetAll().Where(x => x.Currency == currency).ToListAsync();
                if (deals.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = Domain.Enum.StatusCode.ObjectNotFound;
                    return baseResponse;
                }
                baseResponse.Data = deals;
                baseResponse.StatusCode = Domain.Enum.StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Deal>>()
                {
                    Description = $"[GetDealsByCurrency] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Deal>>> GetDealsByDealType(DealType type)
        {
            var baseResponse = new BaseResponse<IEnumerable<Deal>>();
            try
            {
                var deals = await _dealRepository.GetAll().Where(x => x.DealType == type).ToListAsync();
                if (deals.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = Domain.Enum.StatusCode.ObjectNotFound;
                    return baseResponse;
                }
                baseResponse.Data = deals;
                baseResponse.StatusCode = Domain.Enum.StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Deal>>()
                {
                    Description = $"[GetDealsByDealType] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<List<Deal>>> GetDealsByUserName(string name)
        {
            var baseResponse = new BaseResponse<List<Deal>>();
            try
            {
                var deals = await _dealRepository.GetAll().Include(i => i.User).Where(x => x.User.Name == name).ToListAsync();
                if (deals.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = Domain.Enum.StatusCode.ObjectNotFound;
                    return baseResponse;
                }
                baseResponse.Data = deals;
                baseResponse.StatusCode = Domain.Enum.StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Deal>>()
                {
                    Description = $"[GetDealsByUserId] : {ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<List<DealApi>> GetApi(string name)
        {
            var deals = await _dealRepository.GetAll().Include(i => i.User).Where(x => x.User.Name == name).ToListAsync();
            var dlc = new List<DealApi>();
            foreach (var d in deals)
            {
                dlc.Add(new DealApi
                {
                    Amount = d.Amount,
                    Commentary = d.Commentary,
                    Currency = d.Currency.ToString(),
                    FullName = d.Currency.GetDisplayName(),
                    DealType = d.DealType.ToString(),
                    Id = d.Id,
                    Rate = d.Rate,
                    UserId = d.UserId,
                    Date = d.Date.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()
                });
            }
            return dlc;
        }
    }
}
