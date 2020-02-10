using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSS.Common.BasicMos.Resp;
using OSS.Tools.Cache;
using OSS.Tools.DirConfig;

namespace OSS.Tools.Tests.CacheTests
{
    public class CacheTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task DirConfigTest()
        {
            var testRes = await CacheHelper.GetWithSet("TesttttttKey", 
                () => Task.FromResult(new Resp()),TimeSpan.FromSeconds(10),res=>!res.IsSuccess());
           
            var secRes = await CacheHelper.GetWithSet("TesttttttKey",
                () => Task.FromResult(new Resp()),TimeSpan.FromSeconds(10),res=>!res.IsSuccess());

            Assert.IsTrue(testRes.IsSuccess());
        }
    }

}