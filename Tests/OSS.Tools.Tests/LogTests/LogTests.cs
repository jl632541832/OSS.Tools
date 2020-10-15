using System.Threading.Tasks;
using NUnit.Framework;
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
        public Task LogInfoTest()
        {
            LogHelper.Info("test");
            return Task.CompletedTask;
        }
    }

}