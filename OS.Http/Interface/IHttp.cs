using System;
using OS.Http.Models;

namespace OS.Http.Interface
{
    public interface IHttp
    {
        #region 
        /// <summary>
		/// 同步执行
		/// </summary>
		/// <param name="request">请求的参数</param>
		/// <returns>自定义的Response结果</returns>
        OsHttpResponse ExecuteSync(OsHttpRequest request);

        /// <summary>
        /// 异步请求
        /// </summary>
        /// <param name="request">请求的参数</param>
        /// <returns>自定义的Response结果</returns>
        void ExecuteAsync(OsHttpRequest request,Action<OsHttpResponse> action);
        #endregion
    }
}
