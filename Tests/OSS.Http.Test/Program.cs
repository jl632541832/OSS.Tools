
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

            //var result= Test().WaitResult();
            //var con = result.Content.ReadAsStringAsync().WaitResult();
            //Console.Write(con);

            var result = GetTest().WaitResult();
            var resp = result.Content.ReadAsStringAsync().WaitResult();
            Console.Write(resp);

            Console.ReadLine();
        }


        private static async Task<HttpResponseMessage> GetTest()
        {
            var req = new OsHttpRequest();
            req.AddressUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=wxaa9e6cb3f03afa97&secret=0fc0c6f735a90fda1df5fc840e010144&code=ssss&grant_type=authorization_code";
            req.HttpMothed = HttpMothed.GET;
            
            return await req.RestSend();
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
          
            //  文件上传测试
            //var imageFile = new FileStream("E:\\111.png", FileMode.Open, FileAccess.Read);
            //req.FileParameters.Add(new FileParameter("media", imageFile, "111.png", "image/jpeg")); 
            // 表单参数测试
            //req.FormParameters.Add(new FormParameter("description", "测试"));
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
