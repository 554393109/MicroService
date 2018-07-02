/************************************************************************
 * 文件标识：  41F79E20-26E4-41B8-A57D-A4D1854CFE1F
 * 项目名称：  Utility.Extension  
 * 项目描述：  
 * 类 名 称：  DecimalExtension
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
    public static class DecimalExtension
    {
        public static decimal Digit(this decimal value, int lenght)
        {
            lenght = lenght < 0 ? 0 : lenght;
            lenght = lenght > 28 ? 28 : lenght;

            string format = string.Empty;

            if (lenght > 0)
                //format = "0." + "0".PadRight(lenght, '0');
                format = string.Format("F{0}", lenght);
            return Convert.ToDecimal(value.ToString(format));
        }

        public static Nullable<decimal> Digit(this Nullable<decimal> value, int lenght)
        {
            if (value.HasValue)
            {
                lenght = lenght < 0 ? 0 : lenght;
                lenght = lenght > 28 ? 28 : lenght;

                string format = string.Empty;

                if (lenght > 0)
                    //format = "0." + "0".PadRight(lenght, '0');
                    format = string.Format("F{0}", lenght);
                return Convert.ToDecimal(value.Value.ToString(format));
            }
            return null;
        }
    }
}
