using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.Domain.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Login is required")]
        [MaxLength(12, ErrorMessage = "Max login length is 12")]
        [MinLength(3, ErrorMessage = "Min login length is 3")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter password")]
        [MinLength(8, ErrorMessage = "Min password length is 8")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string PasswordConfirm { get; set; }
    }
}
