using System;
using System.Net.Http;
using System.Threading.Tasks;
using OSS.Http.Connect;
using OSS.Http.Models;

namespace OSS.Http
{
    /// <summary>
    /// http请求辅助类
    /// </summary>
    public static class HttpUtil
    {
        private static HttpClientHandler m_Handler;
        private static OsRest m_Client ;

        public static Action<HttpClientHandler> SetHandler { get; set; }
    

        /// <summary>
        /// 同步的请求方式
        /// </summary>
        /// <param name="request">请求的参数</param>
        /// <returns>自定义的Response结果</returns>
        public static Task<HttpResponseMessage> ExecuteSync(this OsHttpRequest request,HttpCompletionOption completionOption=HttpCompletionOption.ResponseContentRead)
        {
            if (m_Client==null)
            {
                m_Handler=new HttpClientHandler();
                ConfigClientHandler(m_Handler);
                m_Client=new OsRest(m_Handler);
            }
            return m_Client.RestSend(request, completionOption);
        }

        /// <summary>
        /// 配置请求处理类
        /// </summary>
        /// <returns></returns>
        public static void ConfigClientHandler(HttpClientHandler reqHandler)
        {
            reqHandler.AllowAutoRedirect = true;
            reqHandler.MaxAutomaticRedirections = 5;
            reqHandler.UseCookies = true;
            SetHandler?.Invoke(reqHandler);
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