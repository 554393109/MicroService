/************************************************************************
 * 文件标识：  9DF17A48-11B6-4551-A3CB-5CA259B3AF98
 * 项目名称：  Utility.HttpClientUtils  
 * 项目描述：  
 * 类 名 称：  ClientContext
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/07/01 16:24:57
 * 更新时间：  2018/07/01 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System.Collections.Generic;

namespace Utility.HttpClientUtils
{
    public class ClientContext
    {
        private Utility.HttpClientUtils.IClient client;

        public ClientContext(IClient _client)
        {
            this.client = _client;
        }

        public string Post(string url, object content)
        {
            return this.client.Post(url, content);
        }

        public string Post(string url, object content, IDictionary<string, FileItem> fileParams)
        {
            return this.client.Post(url, content, fileParams);
        }

        public void PostAsync(string url, object content)
        {
            this.client.PostAsync(url, content);
        }
    }
}
