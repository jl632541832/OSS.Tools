using System;
using OSS.Http;
using OSS.Http.Mos;

namespace OS.Http.NET45.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var req=new OsHttpRequest();

            req.Uri = new Uri("http://www.baidu.com");

            var result = req.RestSend();
            result.Wait();

            Console.WriteLine(result.Result.Content);
            Console.Read();
        }
    }
}
