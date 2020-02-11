using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSS.Common.BasicMos.Resp;
using OSS.Tools.Cache;
using OSS.Tools.Log;

namespace OSS.Tools.Tests.LogTests
{
    public class LogTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LogInfoTest()
        {
            LogHelper.Info("test");
        }
    }

}