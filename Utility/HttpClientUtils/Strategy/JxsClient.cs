/************************************************************************
 * 文件标识：  D151BA7F-5A20-46FE-B574-5CCCF67B7E96
 * 项目名称：  Utility.HttpClientUtils  
 * 项目描述：  
 * 类 名 称：  JxsClient
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
    /// 经销商策略Client
    /// 在当前类中保存该业务的HttpClient
    /// </summary>
    public sealed class JxsClient
        : Utility.HttpClientUtils.BaseClient
    {
        private static readonly object lockHelper_JxsClient = new object();
        private static volatile HttpClient httpClient_JxsClient = null;
        private const int Timeout = 15;

        public JxsClient()
        {
            this.Get_ClientInstance();
            base.Client = httpClient_JxsClient;
        }

        public JxsClient(string format)
            : this()
        {
            base.Format = format;
        }

        public JxsClient(string format, Encoding charset)
            : this(format)
        {
            base.Charset = charset;
        }

        private void Get_ClientInstance()
        {
            if (httpClient_JxsClient == null)
            {
                lock (lockHelper_JxsClient)
                {
                    if (httpClient_JxsClient == null)
                    {
                        httpClient_JxsClient = new HttpClient();
                        httpClient_JxsClient.Timeout = new TimeSpan(0, 0, Timeout);
                        httpClient_JxsClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_JxsClient.DefaultRequestHeaders.Add("User-Agent", "Cysoft.UnifiedPay");
                        httpClient_JxsClient.DefaultRequestHeaders.Add("DNT", "1");
                    }
                }
            }
        }
    }
}
