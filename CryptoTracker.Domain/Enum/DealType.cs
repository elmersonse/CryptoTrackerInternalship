using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.Domain.Enum
{
    public enum DealType
    {
        [Display(Name = "Long Buy")]
        Buy = 0,
        [Display(Name = "Sell")]
        Sell = 2,
        [Display(Name = "Short Buy")]
        ShortBuy = 1,
    }
}
