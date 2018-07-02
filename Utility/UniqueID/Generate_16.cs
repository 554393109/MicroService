/************************************************************************
 * 文件标识：  DE11847D-F29B-4355-AFC8-972247C63952
 * 项目名称：  Utility.UniqueID  
 * 项目描述：  
 * 类 名 称：  Generate_16
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/07/01 16:24:57
 * 更新时间：  2018/07/01 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;

namespace Utility.UniqueID
{
    public class Generate_16
    {
        private static readonly object lock_Generate_16 = new object();								//加锁对象
        private const long twepoch = 687888001020L;                                                 //唯一时间随机量
        private const long machineIdBits = 4L; 														//机器码字节数
        private const long datacenterIdBits = 0L;													//数据中心字节数
        private const long maxMachineId = -1L ^ -1L << (int)machineIdBits; 							//最大机器ID
        private const long maxDatacenterId = -1L ^ (-1L << (int)datacenterIdBits);                  //最大数据ID
        private const long sequenceBits = 8L;                                                       //计数器字节数，12个字节用来保存计数码        
        private const long machineIdShift = sequenceBits; 											//机器码数据左移位数，就是后面计数器占用的位数
        private const long datacenterIdShift = sequenceBits + machineIdBits;
        private const long timestampLeftShift = sequenceBits + machineIdBits + datacenterIdBits; 	//时间戳左移动位数就是机器码+计数器总字节数+数据字节数
        private const long sequenceMask = -1L ^ -1L << (int)sequenceBits;                           //一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成

        private static long machineId = 0L;															//机器ID
        private static long datacenterId = 0L;														//数据中心ID
        private static long sequence = 0L;                                                          //计数从零开始
        private static long lastTimestamp = -1L;													//最后时间戳        

        /// <summary>
        /// 默认机器生成（暂不使用）
        /// </summary>
        private Generate_16()
        {
            UniqueIDs(0L, -1);
        }

        /// <summary>
        /// 使用不同机器生成ID（暂不使用）
        /// </summary>
        /// <param name="machineId"></param>
        private Generate_16(long machineId)
        {
            UniqueIDs(machineId, -1);
        }

        /// <summary>
        /// 此构造需要开放数据中心（暂不使用）
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="datacenterId"></param>
        private Generate_16(long machineId, long datacenterId)
        {
            UniqueIDs(machineId, datacenterId);
        }

        private void UniqueIDs(long machineId, long datacenterId)
        {
            if (machineId >= 0)
            {
                if (machineId > maxMachineId)
                {
                    throw new Exception("机器码ID非法");
                }
                Generate_16.machineId = machineId;
            }
            if (datacenterId >= 0)
            {
                if (datacenterId > maxDatacenterId)
                {
                    throw new Exception("数据中心ID非法");
                }
                Generate_16.datacenterId = datacenterId;
            }
        }




        /// <summary>
        /// 根据Twitter的snowflake算法生成唯一ID
        /// snowflake算法 64 位
        /// 0 --- 0000000000 0000000000 0000000000 0000000000 0 --- 00000 ---00000 ---000000000000
        /// 第一位为未使用（实际上也可作为long的符号位），接下来的41位为毫秒级时间，
        /// 然后5位datacenter标识位，5位机器ID（并不算标识符，实际是为线程标识），
        /// 然后12位该毫秒内的当前毫秒内的计数，加起来刚好64位，为一个Long型。
        /// 其中datacenter标识位起始是机器位，机器ID其实是线程标识，可以同一一个10位来表示不同机器
        /// </summary>
        /// <returns></returns>
        public static long Generate()
        {
            lock (lock_Generate_16)
            {
                long timestamp = Get_Timestamp();
                if (Generate_16.lastTimestamp == timestamp)
                { //同一微妙中生成ID
                    sequence = (sequence + 1) & sequenceMask; //用&运算计算该微秒内产生的计数是否已经到达上限
                    if (sequence == 0)
                    {
                        //一微妙内产生的ID计数已达上限，等待下一微妙
                        timestamp = Get_NextTimestamp(Generate_16.lastTimestamp);
                    }
                }
                else
                {
                    //不同微秒生成ID
                    sequence = 0L;
                }
                if (timestamp < lastTimestamp)
                {
                    throw new Exception("时间戳比上一次生成ID时时间戳还小，故异常");
                }
                Generate_16.lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
                long Id = ((timestamp - twepoch) << (int)timestampLeftShift)
                    | (datacenterId << (int)datacenterIdShift)
                    | (machineId << (int)machineIdShift)
                    | sequence;
                return Id;
            }
        }

        /// <summary>
        /// 生成单号
        /// </summary>
        /// <returns></returns>
        public static string GetBillNo(string px = null)
        {
            return string.Format("{0}{1:yyyyMMddHHmmss}{2}", px, DateTime.Now, Utility.UniqueID.Generate_16.Generate());
        }

        /// <summary>
        /// 商户号
        /// </summary>
        /// <returns></returns>
        public static string GetQrMch()
        {
            return string.Format("f{0}", Utility.UniqueID.Generate_16.Generate());
        }



        /// <summary>
        /// 获取下一微秒时间戳
        /// </summary>
        /// <param name="Timestamp_last"></param>
        /// <returns></returns>
        private static long Get_NextTimestamp(long Timestamp_last)
        {
            long timestamp = Get_Timestamp();
            while (timestamp <= Timestamp_last)
            {
                timestamp = Get_Timestamp();
            }
            return timestamp;
        }

        /// <summary>
        /// 生成当前时间戳
        /// </summary>
        /// <returns>毫秒</returns>
        private static long Get_Timestamp()
        {
            return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
        }
    }
}
