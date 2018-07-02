/************************************************************************
 * 文件标识：  80A5860B-5179-4826-B4D4-D1146BAA4D29
 * 项目名称：  Utility.Consul.Model
 * 项目描述：  
 * 类 名 称：  ServiceEntity
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/07/01 16:24:57
 * 更新时间：  2018/07/01 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

namespace Utility.Consul.Model
{
    public class ServiceEntity
    {
        /// <summary>
        /// 服务IP
        /// </summary>
        public string IP { get; set; } = string.Empty;

        /// <summary>
        /// 服务Port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 服务ID 格式{Guid:N}
        /// 若为空，每次服务启动时使用新ID进行注册
        /// </summary>
        public string ServiceID { get; set; } = string.Empty;

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// 服务中心IP
        /// </summary>
        public string ConsulIP { get; set; } = string.Empty;

        /// <summary>
        /// 服务中心Port
        /// </summary>
        public int ConsulPort { get; set; }
    }
}
