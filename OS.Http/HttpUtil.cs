using System;
using OS.Http.Connect;
using OS.Http.Interface;
using OS.Http.Models;

namespace OS.Http
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpUtil
    {
        private static IHttp httpsync = new RestHttp();

        /// <summary>
        /// 同步的请求方式
        /// </summary>
        /// <param name="request">请求的参数</param>
        /// <returns>自定义的Response结果</returns>
        public static OsHttpResponse ExecuteSync(this OsHttpRequest request)
        {
            return httpsync.ExecuteSync(request);
        }


        /// <summary>
        /// 异步的请求方式
        /// </summary>
        /// <param name="request">请求的参数</param>
        /// <param name="callback"></param>
        /// <returns>自定义的Response结果</returns>
        public static void ExecuteAsync(this OsHttpRequest request, Action<OsHttpResponse> callback)
        {
            httpsync.ExecuteAsync(request, callback);
        }


        ///// <summary>
        ///// 处理远程请求方法，并返回需要的实体
        ///// </summary>
        ///// <typeparam name="T">需要返回的实体类型</typeparam>
        ///// <param name="request">远程请求组件的request基本信息</param>
        ///// <param name="funcFormat">获取实体格式化方法</param>
        ///// <param name="errorButResponseFormat">如果</param>
        ///// <returns>实体类型</returns>
        //public static T RestCommon<T>(OsHttpRequest request, Func<OsHttpResponse, T> funcFormat,
        //    Func<OsHttpResponse, T> errorButResponseFormat = null)
        //    where T : ResultModel, new()
        //{
        //    T t = default(T);

        //    OsHttpResponse response = ExecuteSync(request);
        //    if (response.ResponseStatus == ResponseStatus.Completed
        //        || response.ResponseStatus == ResponseStatus.ErrorButResponse)
        //    {
        //        if (response.ResponseStatus == ResponseStatus.ErrorButResponse
        //            && errorButResponseFormat != null)
        //        {
        //            t = errorButResponseFormat(response);
        //        }
        //        else
        //        {
        //            t = funcFormat(response);
        //        }
        //        return t;
        //    }
        //    return t;
        //}
    }
}