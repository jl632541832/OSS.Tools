using NUnit.Framework;
using OSS.Tools.DirConfig;

namespace OSS.Tools.Tests.DirConfigTests
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
            DirConfigHelper.RemoveDirConfig("Test_Config");
        }
    }

    public class ConfigTest
    {
        public string Name { get; set; }
    }
}