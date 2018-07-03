/************************************************************************
 * 文件标识：  45082E34-557F-4832-A2C0-E23F113B269D
 * 项目名称：  Utility.HttpClientUtils  
 * 项目描述：  
 * 类 名 称：  SandClient
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
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace Utility.HttpClientUtils
{
    /// <summary>
    /// 杉德上传文件无证书策略Client
    /// 在当前类中保存该业务的HttpClient
    /// </summary>
    public sealed class SandClient
        : Utility.HttpClientUtils.BaseClient
    {
        private static readonly object lockHelper_CommonClient = new object();
        private static volatile HttpClient httpClient_SANDClient = null;
        private const int Timeout = 15;

        public SandClient()
        {
            this.Get_ClientInstance();
            base.Client = httpClient_SANDClient;
        }

        public SandClient(string format)
            : this()
        {
            base.Format = format;
        }

        public SandClient(string format, Encoding charset)
            : this(format)
        {
            base.Charset = charset;
        }

        private void Get_ClientInstance()
        {
            if (httpClient_SANDClient == null)
            {
                lock (lockHelper_CommonClient)
                {
                    if (httpClient_SANDClient == null)
                    {
                        httpClient_SANDClient = new HttpClient();
                        httpClient_SANDClient.Timeout = new TimeSpan(0, 0, Timeout);
                        httpClient_SANDClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_SANDClient.DefaultRequestHeaders.Add("User-Agent", "MicroService");
                        httpClient_SANDClient.DefaultRequestHeaders.Add("DNT", "1");
                    }
                }
            }
        }

        public override string Post(string url, object content)
        {
            try
            {
                string result = null;
                string str_content = this.GetContent(content);
                this.Verify(url, str_content);
                this.SetServicePoint(url);
                using (HttpContent httpContent = new StringContent(str_content, this.Charset, "application/json"))
                {
                    HttpResponseMessage response = httpClient_SANDClient.PostAsync(url, httpContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        //LogHelper.Error(string.Format("HttpClientUtils BaseClient Post StatusCode：【{0}】，ReasonPhrase：【{1}】", response.StatusCode, response.ReasonPhrase));
                    }

                    httpContent.Dispose();
                }
                return result;
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                //LogHelper.Error(ex);
                System.Threading.Thread.ResetAbort();
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex.GetType().FullName);
                //LogHelper.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 构造postData
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string GetContent(dynamic content)
        {
            string _content = string.Empty;

            if (content is string)
                return Convert.ToString(content);

            if (content is IDictionary)
            {
                switch (this.Format)
                {
                    case "xml":
                    _content = WebCommon.BuildXml(content);
                    break;

                    case "hash":
                    default:
                    _content = WebCommon.BuildHash(content, false);
                    break;
                }
            }

            return _content;
        }

        private void Verify(string url, object content)
        {
            if (httpClient_SANDClient == null)
                throw new Exception("连接对象为空");

            if (string.IsNullOrWhiteSpace(url))
                throw new Exception("请求地址为空");

            if (content == null)
                throw new Exception("请求内容为空");

            //if (content.GetType() != typeof(string))
            if (!(content is string))
                throw new Exception("请求内容类型错误");
        }

        private void SetServicePoint(string url)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            var servicePoint = System.Net.ServicePointManager.FindServicePoint(new Uri(url));
            if (servicePoint != null)
                servicePoint.ConnectionLeaseTimeout = (1000 * 60 * 5);          //★★修复目标方因蓝绿部署导致的请求失败 默认5分钟回收★★

            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            }
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
