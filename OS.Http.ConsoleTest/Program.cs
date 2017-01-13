using System;
using System.IO;
using System.Threading;
using OS.Http.Models;

namespace OS.Http.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
             Console.WriteLine("请输入请求地址(quit 直接退出)：");
            string url = string.Empty;
            while ((url=Console.ReadLine())!="quit")
            {
                if (!string.IsNullOrEmpty(url))
                {
                    Console.WriteLine("输入 async 执行异步请求，输入 sync 执行同步请求：");
                    string cmd = Console.ReadLine();
                    if (cmd == "async")
                    {
                        AsyncHttp(url);
                    }
                    else if (cmd == "sync")
                    {
                        SyncHttp(url);
                    }
                }
                Console.WriteLine("请输入请求地址：");
            }
        }

        private static void SyncHttp(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                OsHttpRequest request = new OsHttpRequest();
                request.AddressUrl = url;
                request.HttpMothed = HttpMothed.POST;
                request.Parameters.Add(new Parameter("name", "test", ParameterType.Form));
                FileStream file_stream = null;
                try
                {

                    file_stream = new FileStream("E:\\test.txt", FileMode.Open, FileAccess.Read);

                    //byte[] bites = new byte[file_stream.Length];

                    //using (file_stream)
                    //{
                    //    file_stream.Read(bites, 0, bites.Length);
                    //}

                    FileParameter file = new FileParameter("test", file_stream, "test_file", contentType: "text/plain");
                    request.FileParameterList.Add(file);

                    var response = HttpUtil.ExecuteSync(request);

                    Console.WriteLine(response.Content);

                    Console.WriteLine("请求结束！！！");
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (file_stream != null)
                        file_stream.Close();
                }

            }
        }


        private static void AsyncHttp(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                OsHttpRequest request = new OsHttpRequest();
                request.AddressUrl = url;
                request.HttpMothed = HttpMothed.POST;
                request.Parameters.Add(new Parameter("name", "test", ParameterType.Form));
                FileStream file_stream = null;
                try
                {
                    file_stream = new FileStream("E:\\test.txt", FileMode.Open, FileAccess.Read);
                    
                    FileParameter file = new FileParameter("test", file_stream, "test_file", contentType: "text/plain");
                    request.FileParameterList.Add(file);
                    Console.WriteLine(string.Format("当前主线程ID:{0}", Thread.CurrentThread.ManagedThreadId));
                    HttpUtil.ExecuteAsync(request, res =>
                    {
                        Console.WriteLine("进入异步响应 ：");
                        Console.WriteLine(res.Content);
                        Console.WriteLine(" ");
                        Console.WriteLine(string.Format("当前子线程ID:{0}", Thread.CurrentThread.ManagedThreadId));
                        Console.WriteLine("异步结束 ");
                    });
                    Console.WriteLine(string.Format("当前主线程ID:{0}", Thread.CurrentThread.ManagedThreadId));
                }
                catch (Exception)
                {

                }
                finally
                {
                    if (file_stream != null)
                        file_stream.Close();
                }

            }
        }

     

    }
}
