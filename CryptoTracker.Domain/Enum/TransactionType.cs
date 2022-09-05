using System.ComponentModel.DataAnnotations;

namespace CryptoTracker.Domain.Enum
{
    public enum TransactionType
    {
        [Display(Name = "Get Transaction")]
        Get = 0,
        [Display(Name = "Send Transaction")]
        Send = 1,
    }
}