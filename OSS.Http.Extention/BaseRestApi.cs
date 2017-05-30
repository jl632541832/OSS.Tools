#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSS.Http.Extention - 通用App接口请求基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2017-5-25
*       
*****************************************************************************/

#endregion

using System;
using System.Net.Http;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs;
using OSS.Http.Mos;

namespace OSS.Http.Extention
{
    /// <summary>
    /// 通用App接口请求基类
    /// </summary>
    /// <typeparam name="RestType"></typeparam>
    public class BaseRestApi<RestType>
        where RestType:class, new()
    {

        #region  接口配置信息

        /// <summary>
        ///   默认配置信息，如果实例中的配置为空会使用当前配置信息
        /// </summary>
        public static AppConfig DefaultConfig { get; set; }

        private readonly AppConfig _config;

        /// <summary>
        /// 微信接口配置
        /// </summary>
        public AppConfig ApiConfig => _config ?? DefaultConfig;

        /// <summary>
        /// 微信api接口地址
        /// </summary>
        protected const string m_ApiUrl = "https://api.weixin.qq.com";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config"></param>
        public BaseRestApi(AppConfig config=null)
        {
            if (config == null && DefaultConfig == null)
                throw new ArgumentNullException(nameof(config),
                    "构造函数中的config 和 全局DefaultConfig 配置信息同时为空，请通过构造函数赋值，或者在程序入口处给 DefaultConfig 赋值！");
            _config = config;
        }

        #endregion
        
        #region  单例实体

        private static object _lockObj=new object();

        private static RestType _instance;
        /// <summary>
        ///   接口请求实例  
        ///  当 DefaultConfig 设值之后，可以直接通过当前对象调用
        /// </summary>
        public static RestType Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (_lockObj)
                {
                    if (_instance == null)
                        _instance = new RestType();
                }
                return _instance;
            }

        }
        
        #endregion
        
        /// <summary>
        ///   当前模块名称
        ///     方便日志追踪
        /// </summary>
        protected static string ModuleName { get; set; } = ModuleNames.Default;

        /// <summary>
        /// 处理远程请求方法，并返回需要的实体
        /// </summary>
        /// <typeparam name="T">需要返回的实体类型</typeparam>
        /// <param name="request">远程请求组件的request基本信息</param>
        /// <param name="funcFormat">获取实体格式化方法</param>
        /// <returns>实体类型</returns>
        public virtual async Task<T> RestCommon<T>(OsHttpRequest request,
            Func<HttpResponseMessage, Task<T>> funcFormat = null)
            where T : ResultMo, new()
        {
            return await request.RestCommon(funcFormat, ModuleName);
        }
        
    }
}
