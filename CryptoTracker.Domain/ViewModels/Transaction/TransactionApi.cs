namespace CryptoTracker.Domain.ViewModels.Transaction
{
    public class TransactionApi
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public string FullName { get; set; }
        public float Amount { get; set; }
        public string Wallet { get; set; }
        public float Commission { get; set; }
        public string TransactionType { get; set; }
        public string Date { get; set; }
        public string Commentary { get; set; }
        public int UserId { get; set; }
    }
}