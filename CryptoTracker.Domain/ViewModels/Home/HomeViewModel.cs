using System.Collections.Generic;

namespace CryptoTracker.Domain.ViewModels.Home
{
    public class HomeViewModel
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public float Value { get; set; }
        public float BoughtUsd { get; set; }

        public HomeViewModel()
        {
            
        }
    }
}