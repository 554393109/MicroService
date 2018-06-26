namespace Utility
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
