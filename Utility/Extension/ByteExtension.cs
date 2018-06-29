using System;
using System.Text;

namespace Utility.Extension
{
    public static class ByteExtension
    {
        public static string ByteToHexStr(this byte[] source, string format = "X2")
        {
            StringBuilder returnStr = new StringBuilder();
            if (source != null)
            {
                if (!"X2".Equals(format, StringComparison.OrdinalIgnoreCase))
                    format = "X2";

                for (int i = 0; i < source.Length; i++)
                {
                    returnStr.Append(source[i].ToString(format));              //小写
                }
            }
            return returnStr.ToString();
        }
    }
}
