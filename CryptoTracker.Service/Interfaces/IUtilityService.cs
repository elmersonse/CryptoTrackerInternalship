using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTracker.Domain.Response;
using CryptoTracker.Domain.ViewModels.Home;

namespace CryptoTracker.Service.Interfaces
{
    public interface IUtilityService
    {
        BaseResponse<Dictionary<int, string>> GetCurrencies();
        BaseResponse<Dictionary<int, string>> GetDealTypes();
        Task<BaseResponse<int>> GetUserIdByName(string name);
        BaseResponse<Dictionary<int, string>> GetTransactionTypes();
        Task<BaseResponse<List<HomeViewModel>>> GetIndexValues(string name);
        Task SendEmail(string email, string message);
    }
}