/************************************************************************
 * 文件标识：  41788caa-8d4c-4e9d-a1c7-ceab30e893ec
 * 项目名称：  Utility.Extension  
 * 项目描述：  
 * 类 名 称：  ObjectExtensions
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/1/30 16:24:57
 * 更新时间：  2018/1/30 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Utility.Extension
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Hashtable UrlEscape(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            Hashtable param = JSON.ConvertToType<Hashtable>(obj);
            Hashtable param_new = new Hashtable();
            foreach (string key in param.Keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                if (null == param[key] || string.IsNullOrWhiteSpace(param[key].ToString()))
                    param_new[key] = param[key];
                else
                    param_new[key] = Uri.EscapeDataString(param[key].ToString());
            }

            return param_new;
        }

        #region 类型判断

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(this object obj)
        {
            bool isCorrect = false;
            string ip = string.Empty;

            if (obj != null)
                ip = obj.ToString();

            isCorrect = Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
            return isCorrect;
        }

        /// <summary>
        /// 是否为Url
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsURL(this object obj)
        {
            bool isCorrect = false;
            string url = string.Empty;

            if (obj != null)
                url = obj.ToString();

            isCorrect = Regex.IsMatch(url, @"^((http|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?");
            return isCorrect;
        }

        /// <summary>
        /// 判断是否Int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsInt(this object obj)
        {
            bool isCorrect = false;
            int val = 0;

            if (obj != null)
                isCorrect = int.TryParse(obj.ToString(), out val);

            return isCorrect;
        }

        /// <summary>
        /// 判断是否Long
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsLong(this object obj)
        {
            bool isCorrect = false;
            long val = 0;

            if (obj != null)
                isCorrect = long.TryParse(obj.ToString(), out val);

            return isCorrect;
        }

        /// <summary>
        /// 判断是否Decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsDecimal(this object obj)
        {
            bool isCorrect = false;
            decimal val = 0;

            if (obj != null)
                isCorrect = decimal.TryParse(obj.ToString(), out val);

            return isCorrect;
        }

        /// <summary>
        /// 判断是否DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsDateTime(this object obj)
        {
            bool isCorrect = false;
            DateTime val;

            if (obj != null)
                isCorrect = DateTime.TryParse(obj.ToString(), out val);

            return isCorrect;
        }

        public static bool IsNull(this object obj)
        {
            return (obj == null);
        }

        /// <summary>
        /// 判断是否Null或string.Empty
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this object obj)
        {
            bool isCorrect = false;

            if (obj == null || string.IsNullOrWhiteSpace(obj.ToString()))
                isCorrect = true;

            return isCorrect;
        }

        #endregion 类型判断
    }
}
