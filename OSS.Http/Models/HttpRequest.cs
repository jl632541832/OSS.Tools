
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OSS.Http.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class OsHttpRequest
    {
        public OsHttpRequest()
        {
            Parameters = new List<Parameter>();
            FileParameterList = new List<FileParameter>();
        }

        /// <summary>
        /// 请求地址信息
        /// </summary>
        public string AddressUrl { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public HttpMothed HttpMothed { get; set; }

        ///// <summary>
        /////  是否允许自动重定向
        ///// 默认为true  最大可重定向次数为 5
        ///// </summary>
        //public bool AllowAutoRedirect { get; set; } = true;

        ///// <summary>
        /////   是否使用压缩
        ///// </summary>
        //public bool AutoDecompres { get; set; }

        ///// <summary>
        /////   是否使用cookie 
        ///// </summary>
        //public bool UseCookies { get; set; }

        /// <summary>
        ///   头部属性自定义设置
        /// </summary>
        public Action<HttpRequestHeaders> HeaderSetting { get; set; }

        /// <summary>
        ///  HttpClientHandler 自定义属性设置
        /// </summary>
        public Action<HttpClientHandler> HandlerSetting { get; set; }

        #region   请求的内容参数

        /// <summary>
        /// 文件参数列表
        /// </summary>
        public List<FileParameter> FileParameterList { get; set; }

        /// <summary>
        /// 是否存在文件
        /// </summary>
        public bool HasFile
        {
            get { return FileParameterList.Any(); }
        }

        /// <summary>
        /// 非文件参数列表
        /// </summary>
        public List<Parameter> Parameters { get; set; }

        #endregion

        /// <summary>
        /// 自定义内容实体
        /// eg:当上传文件时，无法自定义内容
        /// </summary>
        public string CustomBody { get; set; }

        /// <summary>
        ///    超时时间（毫秒）
        /// </summary>
        public int TimeOutMilSeconds { get; set; }
    }
}
