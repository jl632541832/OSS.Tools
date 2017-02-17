
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using OSS.Http.Mos;

namespace OSS.Http.Test
{
    class Program
    {
        private  static void Main(string[] args)
        {
            //var req = new OsHttpRequest();
            //req.RequestSet = msg => msg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //req.HttpMothed=HttpMothed.POST;
            //req.AddressUrl = "http://www.baidu.com";
            //req.CustomBody = "name=testname";
            //req.FormParameters.Add(new FormParameter("who","me"));


            //var result = req.RestSend();
            //result.Wait();
            //var content = result.Result.Content.ReadAsStringAsync();
            //content.Wait();

            var result= Test().WaitResult();
            var con = result.Content.ReadAsStringAsync().WaitResult();
            Console.Write(con);

            Console.Read();
        }


        private static async Task<HttpResponseMessage> Test()
        {
            //OsHttpRequest req = new OsHttpRequest();

            //req.AddressUrl = "http://www.baidu.com";
            //req.HttpMothed = HttpMothed.GET;
            //return await req.RestSend();

            var req = new OsHttpRequest();
            req.AddressUrl = "http://localhost:59489/";
            req.HttpMothed = HttpMothed.POST;
            var imageFile = new FileStream("E:\\111.png", FileMode.Open, FileAccess.Read);
            req.FileParameters.Add(new FileParameter("media", imageFile, "111.png", "image/jpeg")); //video/mpeg4
            //req.FormParameters.Add(new FormParameter("description", "{\"title\":\"title\", \"introduction\":\"introduction\"}"));
            return await req.RestSend();
        }
    }

    public static class TaskExtention
    {
        /// <summary>
        ///   等待异步执行结果
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static TResult WaitResult<TResult>(this Task<TResult> task)
        {
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 等待异步执行结果
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="milliseconds">等待的毫秒数</param>
        /// <returns></returns>
        public static TResult WaitResult<TResult>(this Task<TResult> task, int milliseconds)
        {
            task.Wait();
            return task.Result;
        }
    }
}
