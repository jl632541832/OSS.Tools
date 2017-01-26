using OSS.Http.Interface;
using OSS.Http.Models;
using System;
using System.Net;

namespace OSS.Http.Connect
{
    internal partial class RestHttp
    {
        public void ExecuteAsync(OsHttpRequest request, Action<OsHttpResponse> action)
        {
            OsHttpResponse response = new OsHttpResponse();

            if (string.IsNullOrEmpty(request.AddressUrl)
                || request.HttpMothed == HttpMothed.None)
            {
                throw new ArgumentException("对不起，请求出错，请检查参数等设置（地址，请求方式等）！", "request");

            }

            HttpWebRequest webrequest = ConfigureWebRequest(request);

            SendBodyDataAsyncAndCallBack(webrequest, request, action);

        }
    }
}
