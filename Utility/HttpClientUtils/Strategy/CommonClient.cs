/************************************************************************
 * 文件标识：  680CF86F-99C7-4612-84A0-38DA7C13D2F6
 * 项目名称：  Utility.HttpClientUtils  
 * 项目描述：  
 * 类 名 称：  CommonClient
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
    /// Common无证书策略Client
    /// 在当前类中保存该业务的HttpClient
    /// </summary>
    public sealed class CommonClient
        : Utility.HttpClientUtils.BaseClient
    {
        private static readonly object lockHelper_CommonClient = new object();
        private static volatile HttpClient httpClient_CommonClient = null;
        private const int Timeout = 15;

        public CommonClient()
        {
            this.Get_ClientInstance();
            base.Client = httpClient_CommonClient;
        }

        public CommonClient(string format)
            : this()
        {
            base.Format = format;
        }

        public CommonClient(string format, Encoding charset)
            : this(format)
        {
            base.Charset = charset;
        }

        private void Get_ClientInstance()
        {
            if (httpClient_CommonClient == null)
            {
                lock (lockHelper_CommonClient)
                {
                    if (httpClient_CommonClient == null)
                    {
                        httpClient_CommonClient = new HttpClient();
                        httpClient_CommonClient.Timeout = new TimeSpan(0, 0, seconds: Timeout);
                        httpClient_CommonClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_CommonClient.DefaultRequestHeaders.Add("User-Agent", "MicroService");
                        httpClient_CommonClient.DefaultRequestHeaders.Add("DNT", "1");
                    }
                }
            }
        }
    }
}
