/************************************************************************
 * 文件标识：  A28F045D-4C53-4FA6-8052-31665E01D2F8
 * 项目名称：  Utility.HttpClientUtils  
 * 项目描述：  
 * 类 名 称：  WechatCertClient
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
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Utility.HttpClientUtils
{
    /// <summary>
    /// 微信有证书策略Client
    /// 在当前类中保存该业务的HttpClient
    /// </summary>
    public sealed class WechatCertClient
        : Utility.HttpClientUtils.BaseClient
    {
        private string cert_filepath;
        private string cert_password;

        private static readonly object lockHelper_WechatCertClient = new object();
        private static volatile HttpClient httpClient_WechatCertClient = null;
        private const int Timeout = 10;

        public WechatCertClient(string cert_filepath, string cert_password)
        {
            this.cert_filepath = cert_filepath;
            this.cert_password = cert_password;

            this.Get_CertClientInstance();
            base.Client = httpClient_WechatCertClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cert_filepath"></param>
        /// <param name="cert_password"></param>
        /// <param name="timeout">超时时间 单位：秒</param>
        /// <param name="format"></param>
        public WechatCertClient(string cert_filepath, string cert_password, string format)
            : this(cert_filepath, cert_password)
        {
            base.Format = format;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cert_filepath"></param>
        /// <param name="cert_password"></param>
        /// <param name="timeout">超时时间 单位：秒</param>
        /// <param name="format"></param>
        /// <param name="charset"></param>
        public WechatCertClient(string cert_filepath, string cert_password, string format, Encoding charset)
            : this(cert_filepath, cert_password, format)
        {
            base.Charset = charset;
        }

        private void Get_CertClientInstance()
        {
            if (httpClient_WechatCertClient == null)
            {
                if (string.IsNullOrWhiteSpace(this.cert_filepath))
                    throw new DirectoryNotFoundException(string.Format("证书路径不存在"));

                if (!File.Exists(cert_filepath))
                    throw new FileNotFoundException(string.Format("证书路径【{0}】，文件不存在", cert_filepath));

                lock (lockHelper_WechatCertClient)
                {
                    if (httpClient_WechatCertClient == null)
                    {
                        var handler = new System.Net.Http.WinHttpHandler(); //new WebRequestHandler();
                        var certificate = new X509Certificate(cert_filepath, cert_password);
                        handler.ClientCertificates.Add(certificate);

                        httpClient_WechatCertClient = new HttpClient(handler);
                        httpClient_WechatCertClient.Timeout = new TimeSpan(0, 0, Timeout);
                        httpClient_WechatCertClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_WechatCertClient.DefaultRequestHeaders.Add("User-Agent", "MicroService");
                        httpClient_WechatCertClient.DefaultRequestHeaders.Add("DNT", "1");
                    }
                }
            }
        }
    }
}
