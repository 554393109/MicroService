/************************************************************************
 * 文件标识：  3fcc9651-b38e-49d4-9cc0-b9610835ef81
 * 项目名称：  Utility.Extension  
 * 项目描述：  
 * 类 名 称：  HashtableExtension
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2017/12/27 16:06:59
 * 更新时间：  2017/12/27 16:06:59
************************************************************************
 * Copyright @ 尹自强 2017. All rights reserved.
************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Extension
{
    public static class HashtableExtension
    {
        /// <summary>
        /// 对Hashtable里的value进行Uri.EscapeDataString，
        /// 返回一个新的Hashtable。
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static Hashtable UrlEscape(this Hashtable hash)
        {
            if (hash == null)
                throw new ArgumentNullException("hash");

            Hashtable param_new = new Hashtable();
            foreach (string key in hash.Keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                if (null == hash[key] || string.IsNullOrWhiteSpace(hash[key].ToString()))
                    param_new[key] = hash[key];
                else
                    param_new[key] = Uri.EscapeDataString(hash[key].ToString());
            }

            return param_new;
        }

        /// <summary>
        /// 对Hashtable里的value进行Uri.UnescapeDataString，
        /// 返回一个新的Hashtable。
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static Hashtable UrlUnescape(this Hashtable hash)
        {
            if (hash == null)
                throw new ArgumentNullException("hash");

            Hashtable param_new = new Hashtable();
            foreach (string key in hash.Keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                if (null == hash[key] || string.IsNullOrWhiteSpace(hash[key].ToString()))
                    param_new[key] = hash[key];
                else
                    param_new[key] = Uri.UnescapeDataString(hash[key].ToString());
            }

            return param_new;
        }

        public static SortedDictionary<string, string> ToSortedDictionary(this Hashtable hash)
        {
            if (hash == null)
                throw new ArgumentNullException("hash");

            var sdic = new SortedDictionary<string, string>();
            foreach (string iterm in hash.Keys)
            {
                sdic[iterm] = hash[iterm] != null
                    ? hash[iterm].ToString()
                    : string.Empty;
            }
            return sdic;
        }

        public static Dictionary<string, string> ToDictionary(this Hashtable hash)
        {
            if (hash == null)
                throw new ArgumentNullException("hash");

            var dic = new Dictionary<string, string>();
            foreach (string key in hash.Keys)
            {
                if (hash[key] != null)
                    dic.Add(key, hash[key].ToString());
            }
            return dic;
        }

        public static Dictionary<K, V> ToDictionary<K, V>(this Hashtable hash)
        {
            if (hash == null)
                throw new ArgumentNullException("hash");

            return hash
              .Cast<DictionaryEntry>()
              .ToDictionary(kvp => (K)kvp.Key, kvp => (V)kvp.Value);
        }


        /// <summary>
        /// 遍历并返回URL参数格式,a=1&b=2
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static string ToUrlParams(this Hashtable hash, bool needEncode = true)
        {
            if (hash == null || hash.Count == 0)
                return string.Empty;

            var dic = hash.ToDictionary<string, object>();
            var enumerator = new SortedDictionary<string, object>(dic).GetEnumerator();
            StringBuilder builder = new StringBuilder();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, object> current = enumerator.Current;
                string key = current.Key;
                current = enumerator.Current;
                string val = current.Value?.ToString();
                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(val))
                {
                    if (needEncode)
                        builder.Append(key).Append("=").Append(Uri.EscapeDataString(val)).Append("&");
                    else
                        builder.Append(key).Append("=").Append(val).Append("&");
                }
            }

            return builder.ToString().TrimEnd('&');
        }

        public static Hashtable RemoveEmpty(this Hashtable hash)
        {
            if (hash == null)
                throw new ArgumentNullException("hash");

            var param_new = new Hashtable();
            foreach (string key in hash.Keys)
            {
                if (key.IsNullOrWhiteSpace() || hash[key].IsNullOrWhiteSpace())
                    continue;

                param_new[key] = hash[key].ToString();
            }

            return param_new;
        }

        /// <summary>
        /// 判断该键值是否存在或为空或为NULL
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this Hashtable hash, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");

            try
            {
                if (hash == null)
                    return true;

                if (hash.ContainsKey(key))
                    return string.IsNullOrWhiteSpace(hash[key]?.ToString());

                return true;
            }
            catch { }
            return true;
        }

        /// <summary>
        /// 取出该键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T>(this Hashtable hash, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");

            if (hash == null)
                throw new ArgumentNullException("hash");
            try
            {
                if (hash.ContainsKey(key))
                    return (T)hash[key];
            }
            catch
            {
            }
            return default(T);
        }
    }
}
