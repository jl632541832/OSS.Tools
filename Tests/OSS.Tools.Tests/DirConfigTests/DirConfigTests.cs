using System.Threading.Tasks;
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
        public async Task DirConfigTest()
        {
            var config = new ConfigTest() {Name = "ConfigTest"};
            await DirConfigHelper.SetDirConfig("Test_Config", config);

            var rConfig = await DirConfigHelper.GetDirConfig<ConfigTest>("Test_Config");

            Assert.True(rConfig?.Name == "ConfigTest");
            await DirConfigHelper.RemoveDirConfig("Test_Config");
        }
    }

    public class ConfigTest
    {
        public string Name { get; set; }
    }
}