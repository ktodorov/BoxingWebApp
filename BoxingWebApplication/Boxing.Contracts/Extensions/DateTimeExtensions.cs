using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsValidExpirationDate(this DateTime date)
        {
            if ((date.ToLocalTime() - DateTime.Now).Minutes < 60 &&
                (date.ToLocalTime() - DateTime.Now).Minutes > 0)
            {
                return true;
            }

            return false;
        }

        public static DateTime CreateExpirationDate()
        {
            return DateTime.Now.AddMinutes(60);
        }
    }
}
