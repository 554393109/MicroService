/************************************************************************
 * 文件标识：  D888616F-6E3E-41BE-9087-053DD76311CA
 * 项目名称：  Utility.HttpClientUtils  
 * 项目描述：  
 * 类 名 称：  WechatClient3
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/07/01 16:24:57
 * 更新时间：  2018/07/01 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Utility.HttpClientUtils
{
    /// <summary>
    /// 微信有无证书策略Client
    /// </summary>
    public sealed class WechatClient3
    {
        #region 私有变量

        /// <summary>
        /// 单位：秒
        /// </summary>
        private int timeout = 15;
        private Encoding charset = Encoding.UTF8;
        private string format = "query";
        private static object lockHelper = new object();
        private static volatile HttpClient httpClient = null;

        #endregion 私有变量

        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        public WechatClient3()
        {
            this.GetClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_charset">内容编码</param>
        public WechatClient3(Encoding _charset)
            : this()
        {
            this.charset = _charset;
        }

        #endregion 构造函数

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="format">传参类型</param>
        /// <returns></returns>
        public string Post(string url, IDictionary<string, string> parameters, string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                this.format = "query";
            else
                this.format = format;

            string postData;

            if (this.format.Equals("xml", StringComparison.OrdinalIgnoreCase))
                postData = this.BuildXml(parameters);
            else
                postData = this.BuildQuery(parameters);


            return this.Post(url, postData, null, null);
        }

        public string Post(string url, IDictionary<string, string> parameters, string format, string cert, string password)
        {
            if (string.IsNullOrWhiteSpace(format))
                this.format = "query";
            else
                this.format = format;

            string postData;

            if (this.format.Equals("xml", StringComparison.OrdinalIgnoreCase))
                postData = this.BuildXml(parameters);
            else
                postData = this.BuildQuery(parameters);


            return this.Post(url, postData, cert, password);
        }

        public string Post(string url, string postData, string cert, string password)
        {
            try
            {
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                using (HttpContent httpContent = new StringContent(postData, this.charset, "application/x-www-form-urlencoded"))
                {
                    System.Net.ServicePointManager.Expect100Continue = false;
                    HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        return result;
                    }
                    else
                    {
                        //LogHelper.Error(string.Format("HttpHelper Post StatusCode：【{0}】，ReasonPhrase：【{1}】", response.StatusCode, response.ReasonPhrase));
                        return null;
                    }
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                //LogHelper.Error(ex);
                System.Threading.Thread.ResetAbort();
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
        }

        public string Get(string url)
        {
            try
            {
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                using (HttpResponseMessage response = httpClient.GetAsync(url).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string respText = response.Content.ReadAsStringAsync().Result;
                        return respText;
                    }
                    else
                    {
                        //LogHelper.Error(string.Format("HttpHelper Get StatusCode：【{0}】，ReasonPhrase：【{1}】", response.StatusCode, response.ReasonPhrase));
                        return null;
                    }
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                //LogHelper.Error(ex);
                System.Threading.Thread.ResetAbort();
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
        }

        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }


        public string BuildQuery(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();

            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;

                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        public string BuildXml(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            postData.Append("<xml>");
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;

                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(value))
                {
                    postData.Append("<").Append(name).Append(">");
                    //postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    postData.Append(value);
                    postData.Append("</").Append(name).Append(">");
                }
            }
            postData.Append("</xml>");
            return postData.ToString();
        }

        private void GetClient()
        {
            if (httpClient == null)
            {
                lock (lockHelper)
                {
                    if (httpClient == null)
                    {
                        httpClient = new HttpClient();
                        httpClient.Timeout = new TimeSpan(0, 0, this.timeout);
                        httpClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                    }
                }
            }
        }
    }
}
