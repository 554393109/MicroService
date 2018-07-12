/************************************************************************
 * 文件标识：  99DEB9D8-2A71-4D7D-AF03-458E3CE35EC0
 * 项目名称：  Utility.Extension  
 * 项目描述：  
 * 类 名 称：  ObjectExtensions
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/07/01 16:24:57
 * 更新时间：  2018/07/01 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Utility.Extension
{
    public static class ObjectExtensions
    {
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

        #region JSON相关

        /// <summary>
        /// 将对象转换为 JSON 字符串。
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="formatting">格式化 None = 0, Indented = 1</param>
        /// <returns></returns>
        public static string ToJson(this object obj, int formatting = 0)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, formatting: (Newtonsoft.Json.Formatting)formatting);
            }
            catch (InvalidOperationException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 将对象转换为 JSON 字符串。
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="settings">序列化设置 待封装</param>
        /// <returns></returns>
        public static string ToJson(this object obj, Newtonsoft.Json.JsonSerializerSettings settings)
        {
            try
            {
                settings = settings ?? new Newtonsoft.Json.JsonSerializerSettings() {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,               // 忽略null
                    DateFormatString = "yyyy-MM-dd HH:mm:ss.fff"                // 格式化DateTime
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, settings: settings);
            }
            catch (InvalidOperationException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 将给定对象转换为指定类型。
        /// 基于Newtonsoft.Json
        /// </summary>
        /// <typeparam name="T">obj 将转换成的类型。</typeparam>
        /// <param name="obj">序列化的 JSON 字符串。</param>
        /// <returns>已转换为目标类型的对象。</returns>
        public static T ConvertToType<T>(this object obj)
        {
            try
            {
                return JSON.Deserialize<T>(JSON.Serialize(obj));
            }
            catch (InvalidOperationException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
        }

        #endregion JSON相关


        public static string ValueOrEmpty(this object obj)
        {
            return obj?.ToString() ?? string.Empty
        }

        public static Hashtable ToHashtable(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return JSON.ConvertToType<Hashtable>(obj);
        }

        /// <summary>
        /// JSON.Serialize(obj)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JSON.Serialize(obj);
        }

        /// <summary>
        /// Serialize(dynamic obj, string root)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="root">根节点</param>
        /// <returns></returns>
        public static string ToXml(this object obj, string root = "xml")
        {
            return XML.Serialize(obj, root);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="needEncode"></param>
        /// <returns></returns>
        public static string ToQueryString(this object obj, bool needEncode = true)
        {
            var builder = new StringBuilder();
            IDictionary<string, string> parameters_temp;

            #region 转换为IDictionary<string, string>

            if (obj is IDictionary<string, string>)
            {
                parameters_temp = obj as IDictionary<string, string>;
            }
            if (obj is IDictionary)
            {
                var parameters = obj as IDictionary;
                parameters_temp = new Dictionary<string, string>();
                foreach (DictionaryEntry item in parameters)
                    parameters_temp.Add(item.Key?.ToString(), item.Value?.ToString() ?? string.Empty);
            }
            else
            {
                parameters_temp = JSON.ConvertToType<Dictionary<string, string>>(obj);
            }

            #endregion 转换为IDictionary<string, string>

            #region 拼接QueryString

            var enumerator = parameters_temp.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string key = enumerator.Current.Key;
                string val = enumerator.Current.Value;

                if (key.IsNullOrWhiteSpace())
                    continue;

                if (needEncode)
                    builder.Append("&").Append(key).Append("=").Append(Uri.EscapeDataString(val));
                else
                    builder.Append("&").Append(key).Append("=").Append(val);
            }

            #endregion 拼接QueryString

            return builder.ToString().TrimStart('&');
        }
    }
}
