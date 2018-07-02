using System;
using Newtonsoft.Json;

namespace Utility
{
    public static class JSON
    {
        /// <summary>
        /// 将指定的 JSON 字符串转换为 T 类型的对象。
        /// </summary>
        /// <typeparam name="T">所生成对象的类型。</typeparam>
        /// <param name="input">要进行反序列化的 JSON 字符串。</param>
        /// <returns>反序列化的对象。</returns>
        public static T Deserialize<T>(string input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch (ArgumentNullException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
        }




        /// <summary>
        /// 将对象转换为 JSON 字符串。
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="formatting">格式化 None = 0, Indented = 1</param>
        /// <returns></returns>
        public static string Serialize(object obj, int formatting = 0)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, formatting: (Newtonsoft.Json.Formatting)formatting);
            }
            catch (InvalidOperationException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 将对象转换为 JSON 字符串。
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="settings">序列化设置 待封装</param>
        /// <returns></returns>
        public static string Serialize(object obj, Newtonsoft.Json.JsonSerializerSettings settings)
        {
            try
            {
                settings = settings ?? new Newtonsoft.Json.JsonSerializerSettings() {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,               // 忽略null
                    DateFormatString = "yyyy-MM-dd HH:mm:ss.fff"                // 格式化DateTime
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, settings: settings);
            }
            catch (InvalidOperationException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 将给定对象转换为指定类型。
        /// 基于Newtonsoft.Json
        /// </summary>
        /// <typeparam name="T">obj 将转换成的类型。</typeparam>
        /// <param name="obj">序列化的 JSON 字符串。</param>
        /// <returns>已转换为目标类型的对象。</returns>
        public static T ConvertToType<T>(object obj)
        {
            try
            {
                return JSON.Deserialize<T>(JSON.Serialize(obj));
            }
            catch (InvalidOperationException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (ArgumentException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
        }
    }
}

