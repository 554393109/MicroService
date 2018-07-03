/************************************************************************
 * 文件标识：  6147DE98-BD8C-46ED-A5EA-70E85B426755
 * 项目名称：  Utility.HttpClientUtils  
 * 项目描述：  
 * 类 名 称：  WechatClient
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
    /// 微信无证书策略Client
    /// 在当前类中保存该业务的HttpClient
    /// </summary>
    public sealed class WechatClient
        : Utility.HttpClientUtils.BaseClient
    {
        private static readonly object lockHelper_WechatClient = new object();
        private static volatile HttpClient httpClient_WechatClient = null;
        private const int Timeout = 10;

        public WechatClient()
        {
            this.Get_ClientInstance();
            base.Client = httpClient_WechatClient;
        }

        public WechatClient(string format)
            : this()
        {
            base.Format = format;
        }

        public WechatClient(string format, Encoding charset)
            : this(format)
        {
            base.Charset = charset;
        }

        private void Get_ClientInstance()
        {
            if (httpClient_WechatClient == null)
            {
                lock (lockHelper_WechatClient)
                {
                    if (httpClient_WechatClient == null)
                    {
                        httpClient_WechatClient = new HttpClient();
                        httpClient_WechatClient.Timeout = new TimeSpan(0, 0, Timeout);
                        httpClient_WechatClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_WechatClient.DefaultRequestHeaders.Add("User-Agent", "MicroService");
                        httpClient_WechatClient.DefaultRequestHeaders.Add("DNT", "1");
                    }
                }
            }
        }
    }
}
