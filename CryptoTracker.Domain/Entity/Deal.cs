using CryptoTracker.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.Domain.Entity
{
    public class Deal
    {
        public int Id { get; set; }
        public Currency Currency { get; set; }
        public float Amount { get; set; }
        public float Rate { get; set; }
        public DealType DealType { get; set; }
        public DateTime Date { get; set; }
        public string Commentary { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
