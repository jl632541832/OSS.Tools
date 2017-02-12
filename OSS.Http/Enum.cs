#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：Http请求公用枚举
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*****************************************************************************/

#endregion

namespace OSS.Http
{
    /// <summary>
    /// 
    /// </summary>
    public enum HttpMothed
    {
        /// <summary>
        /// Get
        /// </summary>
        GET = 0,

        /// <summary>
        /// post
        /// </summary>
        POST = 10,

        /// <summary>
        /// PUT
        /// </summary>
        PUT = 20,

        /// <summary>
        /// DELETE
        /// </summary>
        DELETE = 30,
        HEAD=40,
        OPTIONS=50,
        TRACE=60

    }

    /// <summary>
    /// 返回的状态
    /// </summary>
    public enum ResponseStatus
    {
        /// <summary>
        /// 没有响应
        /// </summary>
        None,
        /// <summary>
        /// 响应ok
        /// </summary>
        Completed,
        /// <summary>
        /// 响应出错
        /// </summary>
        Error,
        /// <summary>
        /// 响应出错但正确返回数据
        /// </summary>
        ErrorButResponse,
        /// <summary>
        /// 超时
        /// </summary>
        TimedOut,
        /// <summary>
        /// 取消
        /// </summary>
        Aborted
    }


    /// <summary>
    /// 平台参数类型
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// 属于url请求数据
        /// </summary>
        Query,
        /// <summary>
        /// form表单数据
        /// </summary>
        Form,
        ///// <summary>
        ///// 地址中的值    
        ///// </summary>
        //UrlSegment,
        /// <summary>
        /// 头部参数
        /// </summary>
        Header,
        ///// <summary>
        ///// cookie
        ///// </summary>
        //Cookie
    }
}
