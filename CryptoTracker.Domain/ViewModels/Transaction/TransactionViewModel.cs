using System.ComponentModel.DataAnnotations;
using CryptoTracker.Domain.Enum;
using CryptoTracker.Domain.Utility;

namespace CryptoTracker.Domain.ViewModels.Transaction
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Currency")]
        public string Currency { get; set; }
        
        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Enter amount")]
        [RegularExpression(@"\d+", ErrorMessage = "Enter valid amount")]
        public float Amount { get; set; }
        
        public string Wallet { get; set; }
        public float Commission { get; set; }
        
        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }
        
        public string Commentary { get; set; }
        //public int UserId { get; set; }

        public TransactionViewModel()
        {
        }

        public TransactionViewModel(Entity.Transaction transaction)
        {
            Id = transaction.Id;
            Currency = transaction.Currency.GetDisplayName();
            Amount = transaction.Amount;
            Wallet = transaction.Wallet;
            Commission = transaction.Commission;
            TransactionType = transaction.TransactionType.GetDisplayName();
            //UserId = transaction.UserId;
        }
    }
}