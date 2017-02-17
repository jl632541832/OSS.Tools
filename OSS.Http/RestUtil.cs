#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：Http请求辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*    	修改日期：2017-2-12
*    	修改内容：迁移至HttpClient框架下
*       
*****************************************************************************/

#endregion

using System.Net.Http;
using System.Threading.Tasks;
using OSS.Http.Mos;

namespace OSS.Http
{
    /// <summary>
    /// http请求辅助类
    /// </summary>
    public static class RestUtil
    {
        private static OsRest m_Client ;
        private static HttpMessageHandler m_MessageHandler;

        /// <summary>
        ///   消息处理信息
        /// </summary>
        public static HttpMessageHandler MessageHandler
        {
            get { return m_MessageHandler ?? (m_MessageHandler = GetClientHandler()); }
            set { m_MessageHandler = value; }
        }

        /// <summary>
        /// 同步的请求方式
        /// </summary>
        /// <param name="request">请求的参数</param>
        /// <param name="completionOption"></param>
        /// <returns>自定义的Response结果</returns>
        public static Task<HttpResponseMessage> RestSend(this OsHttpRequest request,
            HttpCompletionOption completionOption=HttpCompletionOption.ResponseContentRead)
        {
            if (m_Client==null)
            {
                m_Client=new OsRest(MessageHandler);
            }
            return m_Client.RestSend(request, completionOption);
        }

        /// <summary>
        /// 配置请求处理类
        /// </summary>
        /// <returns></returns>
        private static HttpClientHandler GetClientHandler()
        {
            var reqHandler = new HttpClientHandler();

            reqHandler.AllowAutoRedirect = true;
            reqHandler.MaxAutomaticRedirections = 5;
            reqHandler.UseCookies = true;

            return reqHandler;
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