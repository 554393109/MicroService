using System;

namespace Utility.Extension
{
    public static class DecimalExtension
    {
        public static decimal Digit(this decimal value, int lenght)
        {
            lenght = lenght < 0 ? 0 : lenght;
            lenght = lenght > 28 ? 28 : lenght;

            string format = string.Empty;

            if (lenght > 0)
                //format = "0." + "0".PadRight(lenght, '0');
                format = string.Format("F{0}", lenght);
            return Convert.ToDecimal(value.ToString(format));
        }

        public static Nullable<decimal> Digit(this Nullable<decimal> value, int lenght)
        {
            if (value.HasValue)
            {
                lenght = lenght < 0 ? 0 : lenght;
                lenght = lenght > 28 ? 28 : lenght;

                string format = string.Empty;

                if (lenght > 0)
                    //format = "0." + "0".PadRight(lenght, '0');
                    format = string.Format("F{0}", lenght);
                return Convert.ToDecimal(value.Value.ToString(format));
            }
            return null;
        }
    }
}
