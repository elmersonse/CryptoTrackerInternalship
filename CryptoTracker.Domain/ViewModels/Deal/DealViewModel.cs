using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.Domain.ViewModels.Deal
{
    public class DealViewModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Select currency")]
        public string Currency { get; set; }
        
        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Enter amount")]
        public float Amount { get; set; }
        
        [Display(Name = "Rate")]
        [Required(ErrorMessage = "Enter rate")]
        public float Rate { get; set; }
        
        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Select deal type")]
        public string DealType { get; set; }
        
        public string Commentary { get; set; }
        
        //public int UserId { get; set; }
    }
}
