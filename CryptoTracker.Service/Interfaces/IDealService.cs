using CryptoTracker.Domain.Entity;
using CryptoTracker.Domain.Enum;
using CryptoTracker.Domain.Response;
using CryptoTracker.Domain.ViewModels.Deal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.Service.Interfaces
{
    public interface IDealService
    {
        Task<IBaseResponse<bool>> CreateDeal(DealViewModel model, int userId);
        Task<IBaseResponse<bool>> DeleteDeal(int id);
        Task<IBaseResponse<Deal>> GetDeal(int id);
        Task<IBaseResponse<Deal>> EditDeal(int id, DealViewModel model, int userId);
        Task<IBaseResponse<IEnumerable<Deal>>> GetDeals();
        Task<IBaseResponse<IEnumerable<Deal>>> GetDealsByCurrency(Currency currency);
        Task<IBaseResponse<IEnumerable<Deal>>> GetDealsByDealType(DealType type);
        Task<IBaseResponse<List<Deal>>> GetDealsByUserName(string name);
        Task<List<DealApi>> GetApi(string name);
    }
}
