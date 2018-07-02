/************************************************************************
 * 文件标识：  12DAC6C8-B598-4E53-88A5-1EFB693F7CCA
 * 项目名称：  Utility.HttpClientUtils  
 * 项目描述：  
 * 类 名 称：  IClientStrategy
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/07/01 16:24:57
 * 更新时间：  2018/07/01 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System.Text;

namespace Utility.HttpClientUtils
{
    public interface IClientStrategy
        : Utility.HttpClientUtils.IClient
    {
        ///// <summary>
        ///// 请求超时
        ///// 单位：秒
        ///// </summary>
        //int Timeout { get; set; }

        /// <summary>
        /// 内容编码
        /// 默认UTF-8
        /// </summary>
        Encoding Charset { get; set; }

        /// <summary>
        /// 参数格式
        /// 默认hash
        /// </summary>
        string Format { get; set; }


        //HttpClient Client { get; set; }
    }
}
