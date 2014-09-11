using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmonMonitor.Utils
{
    public static class Util
    {
        public static DateTime FromApiTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }

        public static Decimal ConvertToDecimal(string val)
        {
            CultureInfo ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            ci.NumberFormat.NumberGroupSeparator = "";
            ci.NumberFormat.NumberDecimalSeparator = ".";
            decimal outval = 0.0m;
            decimal.TryParse(val, NumberStyles.Any,ci,out outval);
            return outval;
        }

    }
}
