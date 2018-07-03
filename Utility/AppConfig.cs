using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Utility
{
    public class AppConfig
    {
        ///// <summary>
        ///// 配置文件是否存在
        ///// </summary>
        //private static bool IsExistFile
        //{
        //    get {
        //        return File.Exists(ApplicationInfo.ConfigPath);
        //    }
        //}

        //private const string ConfigKeyPrefix = "AppConfig-{0}";
        //private const int ConfigTimeoutSec = 600;

        //private AppConfig() { }

        //public static bool Create(Dictionary<string, string> keyValues)
        //{
        //    bool succeed = false;

        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        //    sb.AppendLine("<settings>");

        //    try
        //    {
        //        foreach (KeyValuePair<string, string> keyValue in keyValues)
        //        {
        //            sb.AppendLine(String.Format("<add key=\"{0}\" value=\"{1}\" /> ", keyValue.Key, keyValue.Value));
        //        }
        //        sb.AppendLine("</settings>");

        //        using (StreamWriter writre = new StreamWriter(ApplicationInfo.ConfigPath, false, Encoding.UTF8))
        //        {
        //            writre.Write(sb.ToString());
        //        }

        //        succeed = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        TextLogHelper.WriterExceptionLog(ex);
        //        succeed = false;
        //    }

        //    return succeed;
        //}

        //public static bool Create(Hashtable keyValues)
        //{
        //    bool succeed = false;

        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        //    sb.AppendLine("<settings>");

        //    try
        //    {
        //        foreach (DictionaryEntry keyValue in keyValues)
        //        {
        //            sb.AppendLine(String.Format("<add key=\"{0}\" value=\"{1}\" /> ", keyValue.Key, keyValue.Value));
        //        }
        //        sb.AppendLine("</settings>");

        //        using (StreamWriter writre = new StreamWriter(ApplicationInfo.ConfigPath, false, Encoding.UTF8))
        //        {
        //            writre.Write(sb.ToString());
        //        }

        //        succeed = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        TextLogHelper.WriterExceptionLog(ex);
        //        succeed = false;
        //    }

        //    return succeed;
        //}

        //public static bool Save(Dictionary<string, string> keyValues)
        //{
        //    bool succeed = false;
        //    if (!IsExistFile)
        //    {
        //        succeed = Create(keyValues);
        //    }
        //    else
        //    {
        //        try
        //        {
        //            XmlDocument doc = XmlDoc;
        //            foreach (KeyValuePair<string, string> item in keyValues)
        //            {
        //                XmlNode node = doc.SelectSingleNode(String.Format("//settings/add[@key='{0}']", item.Key));
        //                if (node == null)
        //                {
        //                    XmlNode root = doc.SelectSingleNode("/settings");
        //                    XmlElement elem = doc.CreateElement("add");

        //                    XmlAttribute attr = doc.CreateAttribute("key");
        //                    attr.Value = item.Key;
        //                    elem.Attributes.SetNamedItem(attr);
        //                    node = root.AppendChild(elem);
        //                    attr = doc.CreateAttribute("value");
        //                    attr.Value = item.Value;
        //                    elem.Attributes.SetNamedItem(attr);
        //                    node = root.AppendChild(elem);
        //                }
        //                else
        //                {
        //                    node.Attributes["value"].Value = item.Value;
        //                }
        //            }
        //            doc.Save(ApplicationInfo.ConfigPath);
        //            succeed = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            TextLogHelper.WriterExceptionLog(ex);
        //            succeed = false;
        //        }
        //    }

        //    return succeed;
        //}

        //public static bool SetValue(string key, string newValue)
        //{
        //    if (!IsExistFile)
        //    {
        //        return false;
        //    }
        //    bool succeed = false;
        //    try
        //    {
        //        XmlDocument doc = XmlDoc;
        //        XmlNode node = doc.SelectSingleNode(String.Format("//settings/add[@key='{0}']", key));
        //        if (node == null)
        //        {
        //            XmlNode root = doc.SelectSingleNode("/settings");
        //            XmlElement elem = doc.CreateElement("add");

        //            XmlAttribute attr = doc.CreateAttribute("key");
        //            attr.Value = key;
        //            elem.Attributes.SetNamedItem(attr);
        //            node = root.AppendChild(elem);
        //            attr = doc.CreateAttribute("value");
        //            attr.Value = newValue;
        //            elem.Attributes.SetNamedItem(attr);
        //            node = root.AppendChild(elem);
        //        }
        //        else
        //        {
        //            node.Attributes["value"].Value = newValue;
        //        }
        //        doc.Save(ApplicationInfo.ConfigPath);
        //        succeed = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        TextLogHelper.WriterExceptionLog(ex);
        //        succeed = false;
        //    }

        //    return succeed;
        //}

        //public static string GetValue(string key)
        //{
        //    if (!IsExistFile)
        //    {
        //        //return null;
        //        return string.Empty;
        //    }

        //    XmlNode node = GetNode(key);
        //    if (node == null)
        //        return string.Empty;

        //    return node.Attributes["value"].Value;
        //}

        ///// <summary>
        ///// 写入缓存
        ///// 缓存时长：10分钟
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string GetValue_Cache(string key)
        //{
        //    string value = GetValue(key);

        //    if (!string.IsNullOrWhiteSpace(value))
        //        CacheHelper.Insert(string.Format(ConfigKeyPrefix, key), value, ConfigTimeoutSec);

        //    return value;
        //}

        //public static XmlNode GetNode(string key)
        //{
        //    if (!IsExistFile)
        //    {
        //        return null;
        //    }
        //    return XmlDoc.SelectSingleNode(string.Format("//settings/add[@key='{0}']", key));
        //}

        //public static bool ExistNode(string key)
        //{
        //    if (!IsExistFile)
        //    {
        //        return false;
        //    }
        //    XmlNode node = XmlDoc.SelectSingleNode(String.Format("//settings/add[@key='{0}']", key));
        //    return node == null ? false : true;
        //}

        ///// <summary>
        ///// 全部
        ///// </summary>
        //public static Dictionary<string, string> KeyValues
        //{
        //    get {
        //        if (!IsExistFile)
        //        {
        //            return null;
        //        }
        //        Dictionary<string, string> dic = new Dictionary<string, string>();
        //        XmlNodeList nodeList = XmlDoc.GetElementsByTagName("add");
        //        foreach (XmlNode node in nodeList)
        //        {
        //            dic.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
        //        }
        //        return dic;
        //    }
        //}

        //public static XmlDocument XmlDoc
        //{
        //    get {
        //        if (!IsExistFile)
        //        {
        //            return null;
        //        }
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(ApplicationInfo.ConfigPath);
        //        return doc;
        //    }
        //}
    }
}
