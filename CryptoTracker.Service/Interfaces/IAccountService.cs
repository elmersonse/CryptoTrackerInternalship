using System.Security.Claims;
using System.Threading.Tasks;
using CryptoTracker.Domain.Response;
using CryptoTracker.Domain.ViewModels.Account;

namespace CryptoTracker.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);
        Task<BaseResponse<bool>> Check(RegisterViewModel model);
        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
        Task<BaseResponse<bool>> UpdateProfit(float profit, string username);

    }
}