/************************************************************************
 * 文件标识：  05F6E402-9DE1-4995-9D83-0C6FEFA68D24
 * 项目名称：  Utility.HttpClientUtils  
 * 项目描述：  
 * 类 名 称：  IClient
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
    public interface IClient
    {
        string Post(string url, object content);

        string Post(string url, object content, IDictionary<string, FileItem> fileParams);

        //Task PostAsync(string requestUri, object content);
        void PostAsync(string url, object content);

        //TResult PostAsync<TResult>(string url, object content);
    }
}
