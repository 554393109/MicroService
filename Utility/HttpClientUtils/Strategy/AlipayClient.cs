/************************************************************************
 * 文件标识：  EE28D8B1-E411-41A6-A72E-27F48F52079A
 * 项目名称：  Utility.HttpClientUtils  
 * 项目描述：  
 * 类 名 称：  AlipayClient
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/07/01 16:24:57
 * 更新时间：  2018/07/01 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;
using System.Net.Http;
using System.Text;

namespace Utility.HttpClientUtils
{
    /// <summary>
    /// Alipay无证书策略Client
    /// 在当前类中保存该业务的HttpClient
    /// </summary>
    public sealed class AlipayClient
        : Utility.HttpClientUtils.BaseClient
    {
        private static readonly object lockHelper_AlipayClient = new object();
        private static volatile HttpClient httpClient_AlipayClient = null;
        private const int Timeout = 10;

        public AlipayClient()
        {
            this.Get_ClientInstance();
            base.Client = httpClient_AlipayClient;
        }

        public AlipayClient(string format)
            : this()
        {
            base.Format = format;
        }

        public AlipayClient(string format, Encoding charset)
            : this(format)
        {
            base.Charset = charset;
        }

        private void Get_ClientInstance()
        {
            if (httpClient_AlipayClient == null)
            {
                lock (lockHelper_AlipayClient)
                {
                    if (httpClient_AlipayClient == null)
                    {
                        httpClient_AlipayClient = new HttpClient();
                        httpClient_AlipayClient.Timeout = new TimeSpan(0, 0, Timeout);
                        httpClient_AlipayClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_AlipayClient.DefaultRequestHeaders.Add("User-Agent", "MicroService");
                        httpClient_AlipayClient.DefaultRequestHeaders.Add("DNT", "1");
                    }
                }
            }
        }
    }
}
