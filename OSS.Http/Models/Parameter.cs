#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：Http请求 == 参数实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*****************************************************************************/

#endregion

using System;

namespace OSS.Http.Models
{
    /// <summary>
    /// 
    /// </summary>
    public struct Parameter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public Parameter(string name, object value,ParameterType type)
        {
            Name = name;
            Value = value;
            Type = type;
            Domain = string.Empty;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="domain"></param>
        /// <param name="type"></param>
        public Parameter(string name, object value,string domain, ParameterType type)
        {
            Name = name;
            Value = value;
            Type = type;
            Domain = domain;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 参数值
        /// </summary>
        public object Value;
        
        /// <summary>
        /// 参数类型
        /// </summary>
        public ParameterType Type;

        /// <summary>
        ///  cookie的域名   -- cookie 类型时需要
        /// </summary>
        public string Domain;

        /// <summary>
        /// 重写ToString返回   name=value编码后的格式
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return string.Format("{0}={1}", Name.UrlEncode(), Value.UrlEncode());
        }
    }

    internal static class UrlExtension
    {
        /// <summary>
        /// Url编码处理
        /// </summary>
        public static string UrlEncode(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return Uri.EscapeDataString(input);
        }

        /// <summary>
        /// Url编码处理
        /// </summary>
        public static string UrlEncode(this object input)
        {
            if (input == null)
                return string.Empty;

            return Uri.EscapeDataString(input.ToString());
        }
    }

   
}
