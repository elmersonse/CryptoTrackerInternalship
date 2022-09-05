using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CryptoTracker.DAL.Interfaces;
using CryptoTracker.Domain.Entity;
using CryptoTracker.Domain.Enum;
using CryptoTracker.Domain.Helper;
using CryptoTracker.Domain.Response;
using CryptoTracker.Domain.ViewModels.Account;
using CryptoTracker.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptoTracker.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseRepository<User> _userRepository;

        public AccountService(IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<bool>> Check(RegisterViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == model.Name);
                if (user != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "User with same login already exists",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }

                user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email);
                if (user != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "User with same email already exists",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[Check] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }

            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK
            };
        }

        public async Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model)
        {
            try
            {
                PasswordHashHelper hash = new PasswordHashHelper(model.Password);
                var user = new User()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = Convert.ToBase64String(hash.ToArray()),
                    Role = Role.User
                };

                await _userRepository.Create(user);
                var result = Authenticate(user);
                
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK,
                    Description = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = $"[Register] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == model.Name);
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "User not found"
                    };
                }

                byte[] passwordBytes = Convert.FromBase64String(user.Password);
                PasswordHashHelper hash = new PasswordHashHelper(passwordBytes);
                if (!hash.Verify(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Wrong password",
                        StatusCode = StatusCode.InternalServerError
                    };
                }

                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = $"[Login] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };
            return new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        public async Task<BaseResponse<bool>> UpdateProfit(float profit, string username)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Name == username);
            if (user == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.ObjectNotFound
                };
            }

            user.Profit = profit;
            await _userRepository.Update(user);
            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.OK,
                Data = true
            };
        }
    }
}