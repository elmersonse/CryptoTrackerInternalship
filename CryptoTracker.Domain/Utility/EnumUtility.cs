using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace CryptoTracker.Domain.Utility
{
    public static class EnumUtility
    {
        public static string GetDisplayName(this System.Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                ?.GetName() ?? "Unknown";
        }
    }
}