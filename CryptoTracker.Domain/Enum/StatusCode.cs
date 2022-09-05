using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.Domain.Enum
{
    public enum StatusCode
    {
        ObjectNotFound = 0,
        OK = 200,
        InternalServerError = 500
    }
}
