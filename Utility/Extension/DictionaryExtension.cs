/************************************************************************
 * 文件标识：  e8aa73a5-2e38-42ec-910d-7494e06ae610
 * 项目名称：  Utility.Extension  
 * 项目描述：  
 * 类 名 称：  DictionaryExtension
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/2/7 11:34:06
 * 更新时间：  2018/2/7 11:34:06
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System.Collections;
using System.Collections.Generic;

namespace Utility.Extension
{
    public static class DictionaryExtension
    {
        public static Hashtable ToHashtable(this IDictionary dic)
        {
            var hash = new Hashtable();

            if (dic != null && dic.Count > 0)
            {
                foreach (string key in dic.Keys)
                {
                    if (key != null)
                        hash.Add(key, dic[key]);
                }
            }

            return hash;
        }

        public static SortedDictionary<string, string> ToSortedDictionary(this Dictionary<string, object> dic)
        {
            var sdic = new SortedDictionary<string, string>();

            foreach (string iterm in dic.Keys)
            {
                sdic[iterm] = dic[iterm] != null ? dic[iterm].ToString() : string.Empty;
            }
            return sdic;
        }

    }
}
