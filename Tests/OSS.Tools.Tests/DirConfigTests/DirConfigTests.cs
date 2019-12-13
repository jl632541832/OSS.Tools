using NUnit.Framework;
using OSS.Common.Resp;
using OSS.Tools.DirConfig;

namespace Tests
{
    public class DirConfigTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DirConfigTest()
        {
            var config = new ConfigTest() {Name = "ConfigTest"};
            DirConfigHelper.SetDirConfig("Test_Config", config);

            var rConfig = DirConfigHelper.GetDirConfig<ConfigTest>("Test_Config");

            Assert.True(rConfig?.Name== "ConfigTest");
            Assert.True(DirConfigHelper.RemoveDirConfig("Test_Config").IsSuccess());
        }
    }

    public class ConfigTest
    {
        public string Name { get; set; }
    }
}