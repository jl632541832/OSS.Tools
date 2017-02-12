using System;
using OSS.Http.Mos;

namespace OSS.Http.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var req = new OsHttpRequest();

            req.Uri = new Uri("http://www.baidu.com");

            var result = req.SendRest();
            result.Wait();
            var content = result.Result.Content.ReadAsStringAsync();
            content.Wait();
            Console.WriteLine(content.Result);
            Console.Read();
        }
    }
}
