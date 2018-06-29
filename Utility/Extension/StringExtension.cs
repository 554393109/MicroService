using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utility.Extension
{
    public static class StringExtension
    {
        public static string Left(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            if (length < 1)
            {
                return string.Empty;
            }
            value = value.Length > length ? value.Substring(0, length) : value;
            return value;
        }

        public static string Right(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            if (length < 1)
            {
                return string.Empty;
            }
            value = value.Length > length ? value.Substring(value.Length - length, length) : value;
            return value;
        }

        public static string IsNullOrEmpty(string value, string defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            return value;
        }

        public static string ToFormat(this string value, params object[] args)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (string.IsNullOrWhiteSpace(value))
                return value;

            return string.Format(value, args);
        }





        public static string Join(this string[] array, string separator)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.Length > 0)
                return string.Join(separator, array);

            return string.Empty;
        }

        public static string Join<TSource>(this IEnumerable<TSource> source, Func<TSource, string> keySelector, string separator)
        {
            string result = string.Empty;
            if (source != null && source.Count() > 0)
            {
                string[] value = source.Select(keySelector).ToArray();
                result = string.Join(separator, value);
            }
            return result;
        }

        public static bool Contains(this string[] array, string value, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.Count() > 0)
            {
                foreach (string item in array)
                {
                    if (item.Equals(value, comparisonType))
                        return true;
                }
            }
            return false;
        }

        public static long GetBytesLength(this string value, string format = "UTF8")
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            if (string.IsNullOrWhiteSpace(format))
                format = "UTF8";

            return Encoding.GetEncoding(format).GetBytes(value).Length;
        }

        public static int ToInt32(this string value)
        {
            return Convert.ToInt32(value);
        }



        /// <summary>
        /// 截取字符串
        /// 减3位 拼省略号【...】
        /// </summary>
        /// <param name="str_original"></param>
        /// <param name="len">最大长度</param>
        /// <returns></returns>
        public static string CutString(this string str_original, int len)
        {
            if (!string.IsNullOrWhiteSpace(str_original) && str_original.Length > len)
                return str_original.Remove(len - 3) + "...";
            else
                return str_original;
        }


        /// <summary>
        /// String转Unicode
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StringToUnicode(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            byte[] bytes = Encoding.Unicode.GetBytes(source);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode转String
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string UnicodeToString(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        /// <summary>
        /// <para>将 URL 中的参数名称/值编码为合法的格式。</para>
        /// <para>可以解决类似这样的问题：假设参数名为 tvshow, 参数值为 Tom&Jerry，如果不编码，可能得到的网址： http://a.com/?tvshow=Tom&Jerry&year=1965 编码后则为：http://a.com/?tvshow=Tom%26Jerry&year=1965 </para>
        /// <para>实践中经常导致问题的字符有：'&', '?', '=' 等</para>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string UrlEscape(this string source)
        {
            if (source == null)
                return null;

            return Uri.EscapeDataString(source);
        }

        public static string UrlUnescape(this string source)
        {
            if (source == null)
                return null;

            return Uri.UnescapeDataString(source);
        }

        public static byte[] HexStrToByte(this string source)
        {
            source = source.Replace(" ", "");
            if ((source.Length % 2) != 0)
                source += " ";
            byte[] returnBytes = new byte[source.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(source.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        //        /// <summary>
        //        /// 封装System.Web.HttpUtility.UrlEncode
        //        /// </summary>
        //        /// <param name="url"></param>
        //        /// <returns></returns>
        //#if DEBUG
        //        [Obsolete("此方法已过期，建议使用Utility.StringExtension.UrlEscape()方法")]
        //#endif
        //        public static string UrlEncode(this string url)
        //        {
        //            if (url == null)
        //                return string.Empty;

        //            return System.Web.HttpUtility.UrlEncode(url);
        //        }


        ///// <summary>
        ///// 封装System.Web.HttpUtility.UrlDecode
        ///// </summary>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //public static string UrlDecode(this string url)
        //{
        //    if (url == null)
        //        return string.Empty;

        //    return System.Web.HttpUtility.UrlDecode(url);
        //}

        public static string TrimSpace(this string source)
        {
            if (source == null)
                return null;

            return source.Replace(" ", string.Empty);
        }
    }
}
