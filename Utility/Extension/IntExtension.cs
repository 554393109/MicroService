/************************************************************************
 * 文件标识：  10312800-F24A-46F6-8B00-AF43B3C9F0A0
 * 项目名称：  Utility.Extension  
 * 项目描述：  
 * 类 名 称：  IntExtension
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/07/01 16:24:57
 * 更新时间：  2018/07/01 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;

namespace Utility.Extension
{
    public static class IntExtension
    {
        public static string Join(this int[] array, string separator)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.Length > 0)
            {
                string[] strArray = new string[array.Length];
                for (int i = 0; i < array.Length; i++)
                    strArray[i] = array[i].ToString();

                return string.Join(separator, strArray);
            }

            return string.Empty;
        }

    }
}
