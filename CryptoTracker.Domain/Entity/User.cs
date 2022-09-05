using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoTracker.Domain.Enum;

namespace CryptoTracker.Domain.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public float Profit { get; set; }
        
        public List<Deal> Deals { get; set; }
        public List<Transaction> Transactions { get; set; }

    }
}
