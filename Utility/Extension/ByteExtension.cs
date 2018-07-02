/************************************************************************
 * 文件标识：  AF25746C-1220-45F8-8F00-77B108BCA5B7
 * 项目名称：  Utility.Extension  
 * 项目描述：  
 * 类 名 称：  ByteExtension
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/07/01 16:24:57
 * 更新时间：  2018/07/01 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;
using System.Text;

namespace Utility.Extension
{
    public static class ByteExtension
    {
        public static string ByteToHexStr(this byte[] source, string format = "X2")
        {
            StringBuilder returnStr = new StringBuilder();
            if (source != null)
            {
                if (!"X2".Equals(format, StringComparison.OrdinalIgnoreCase))
                    format = "X2";

                for (int i = 0; i < source.Length; i++)
                {
                    returnStr.Append(source[i].ToString(format));              //小写
                }
            }
            return returnStr.ToString();
        }
    }
}
