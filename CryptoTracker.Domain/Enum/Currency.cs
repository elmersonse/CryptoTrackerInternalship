using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.Domain.Enum
{
    public enum Currency
    {
        [Display(Name = "Bitcoin")]
        BTC = 0,
        [Display(Name = "Ethereum")]
        ETH = 1,
        [Display(Name = "Tether")]
        USDT = 2,
        [Display(Name = "Ripple")]
        XRP = 3,
        [Display(Name = "BNB")]
        BNB = 4
    }
}
