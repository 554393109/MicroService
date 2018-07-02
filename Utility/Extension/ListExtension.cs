/************************************************************************
 * 文件标识：  A68E9028-C226-4B53-BA13-88F8BBA3145E
 * 项目名称：  Utility.Extension  
 * 项目描述：  
 * 类 名 称：  ListExtension
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/07/01 16:24:57
 * 更新时间：  2018/07/01 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility.Extension
{
    public static class ListExtension
    {
        public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var query = source.Select(selector);
            return query.ToList();
        }
    }
}
