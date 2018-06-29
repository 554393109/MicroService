/************************************************************************
 * 文件标识：  41788caa-8d4c-4e9d-a1c7-ceab30e893ec
 * 项目名称：  Utility.Extension  
 * 项目描述：  
 * 类 名 称：  DateTimeExtensions
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/04/20 16:24:57
 * 更新时间：  2018/04/20 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;

namespace Utility.Extension
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Unix epoch start date (lower boundary)
        /// </summary>
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Unix Millennium problem date (upper boundary)
        /// </summary>
        private static readonly DateTime _epochLimit = new DateTime(2038, 1, 19, 3, 14, 7, 0, DateTimeKind.Utc);

        /// <summary>
        /// 本地时间 -> 时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            if (dateTime == null)
                throw new ArgumentNullException("dateTime");

            TimeSpan diff = dateTime - _epoch;
            return Convert.ToInt64(Math.Floor(diff.TotalSeconds));
        }

        /// <summary>
        /// 本地时间.ToUniversalTime() -> 时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixUTCTimestamp(this DateTime dateTime)
        {
            if (dateTime == null)
                throw new ArgumentNullException("dateTime");

            // ArgumentAssert.IsInRange(dateTime, _epoch, _epochLimit, Messages.Exception_ArgumentDateTimeOutOfRangeUnixTimestamp);
            TimeSpan diff = dateTime.ToUniversalTime() - _epoch;
            return Convert.ToInt64(Math.Floor(diff.TotalSeconds));
        }



        /// <summary>
        /// 时间戳 -> 本地时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimestamp(this Nullable<long> timestamp)
        {
            if (!timestamp.HasValue)
                throw new ArgumentNullException("timestamp");

            return (_epoch + TimeSpan.FromSeconds((double)timestamp)).ToLocalTime();
        }

        /// <summary>
        /// Convert Unix timestamp integer value to DateTime object.
        /// </summary>
        /// <param name="timestamp">signed integer value, specifies Unix timestamp</param>
        /// <returns>DateTime object (represented in local time) based on specified Unix timestamp value</returns>
        /// <exception cref="ArgumentOutOfRangeException">Unix timestamp value can'img_type be converted to DateTime due out of signed int range</exception>
        public static DateTime FromUnixUTCTimestamp(this Nullable<long> timestamp)
        {
            if (!timestamp.HasValue)
                throw new ArgumentNullException("timestamp");

            // ArgumentAssert.IsGreaterOrEqual(timestamp, 0, Messages.Exception_ArgumentUnixTimestampOutOfRange);
            return (_epoch + TimeSpan.FromSeconds((double)timestamp)).ToUniversalTime();
        }
    }
}
