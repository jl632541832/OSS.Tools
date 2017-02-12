using OSS.Http;
using OSS.Http.Models;

namespace OS.Http.NET45.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var req=new OsHttpRequest();

            req.AddressUrl = "http://www.baidu.com";

            var result = req.SendRest();
        }
    }
}
