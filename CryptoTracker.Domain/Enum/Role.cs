using System.ComponentModel.DataAnnotations;

namespace CryptoTracker.Domain.Enum
{
    public enum Role
    {
        [Display(Name = "Admin")]
        Admin = 0,
        [Display(Name = "User")]
        User = 1
    }
}