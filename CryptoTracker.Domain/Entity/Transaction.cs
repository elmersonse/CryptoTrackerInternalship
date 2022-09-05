using System;
using CryptoTracker.Domain.Enum;

namespace CryptoTracker.Domain.Entity
{
    public class Transaction
    {
        public int Id { get; set; }
        public Currency Currency { get; set; }
        public float Amount { get; set; }
        public string Wallet { get; set; }
        public float Commission { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime Date { get; set; }
        public string Commentary { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}