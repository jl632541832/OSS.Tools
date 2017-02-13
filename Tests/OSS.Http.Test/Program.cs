using System;
using System.Net.Http;
using System.Net.Http.Headers;
using OSS.Http.Mos;

namespace OSS.Http.Test
{
    class Program
    {
        private static void Main(string[] args)
        {
            var req = new OsHttpRequest();
            req.RequestSet = msg => msg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            req.Uri = new Uri("http://www.baidu.com");

            var result = req.RestSend();
            result.Wait();
            var content = result.Result.Content.ReadAsStringAsync();
            content.Wait();
            Console.WriteLine(content.Result);
            Console.Read();


            var client = new HttpClient();
            var resTask = client.GetAsync("http://www.baidu.com");

        }
    }
}
