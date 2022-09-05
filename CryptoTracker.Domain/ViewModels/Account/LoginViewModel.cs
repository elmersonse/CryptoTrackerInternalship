using System.ComponentModel.DataAnnotations;

namespace CryptoTracker.Domain.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter login")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}