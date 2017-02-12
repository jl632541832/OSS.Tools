using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSS.Http.Models;

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
