using System;

namespace CryptoTracker.Domain.ViewModels.Deal
{
    public class DealApi
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public string FullName { get; set; }
        public float Amount { get; set; }
        public float Rate { get; set; }
        public string DealType { get; set; }
        public string Date { get; set; }
        public string Commentary { get; set; }
        public int UserId { get; set; }
    }
}