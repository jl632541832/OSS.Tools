using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OSS.Http.Mos;

namespace OSS.Http.Test
{
    class Program
    {
        private static void Main(string[] args)
        {
            var req = new OsHttpRequest();
            req.RequestSet = msg => msg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            req.HttpMothed=HttpMothed.POST;
            req.AddressUrl = "http://www.baidu.com";
            req.CustomBody = "name=testname";
            req.FormParameters.Add(new FormParameter("who","me"));


            var result = req.RestSend();
            result.Wait();
            var content = result.Result.Content.ReadAsStringAsync();
            content.Wait();
    

        }
    }
}
