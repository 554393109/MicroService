using Utility.Extension;
using Newtonsoft.Json;
using System;

namespace Utility
{
    /// <summary>
    /// 基于Json.Net的XML序列化操作类
    /// YZQ
    /// 2016.12
    /// </summary>
    public static class XML
    {
        /// <summary>
        /// 已有xml根节点
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(dynamic obj)
        {
            return XML.Serialize(obj, null);
        }

        /// <summary>
        /// 根节点需要定义root
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public static string Serialize(dynamic obj, string root)
        {
            return XML.Serialize(obj, root, (int)System.Xml.Linq.SaveOptions.DisableFormatting);
        }

        public static string Serialize(dynamic obj, string root, int formatting)
        {
            string str_json = JsonConvert.SerializeObject(obj, Formatting.None);
            System.Xml.Linq.XNode node;

            if (root.IsNullOrWhiteSpace())
                node = JsonConvert.DeserializeXNode(str_json);
            else
                node = JsonConvert.DeserializeXNode(str_json, root);

            string str_xml = node.ToString((System.Xml.Linq.SaveOptions)formatting);

            //str_xml = str_xml.Replace("&lt;![", "<![").Replace("]]&gt;", "]]>");        //Bug Fix by YZQ；此法已弃用，改用匿名对象中new Dictionary<string, string>() { { "#cdata-section", "value" } },
            return str_xml;
        }

        public static T Deserialize<T>(string xml)
        {
            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument() { XmlResolver = null };
                doc.LoadXml(xml);
                string json = JsonConvert.SerializeXmlNode(doc, Formatting.None);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
        }
    }
}

