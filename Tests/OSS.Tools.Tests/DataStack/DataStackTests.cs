using NUnit.Framework;
using OSS.Tools.DataStack;

using System.Threading.Tasks;

namespace OSS.Tools.Tests.DataStack
{
    public class DataStackTests
    {
        [SetUp]
        public void Setup()
        {
        }
        private static readonly IStackPusher<MsgData> _pusher = DataStackFactory.CreateStack(new MsgPoper());

        [Test]
        public async Task DataStackTest()
        {
            var pushRes= await _pusher.Push(new MsgData() { name = "test" });
            Assert.True(pushRes);

            await Task.Delay(2000);
        }


        private static readonly IStackPusher<MsgData> _fpusher = DataStackFactory.CreateStack<MsgData>(async (data)=>
        {
            await Task.Delay(1000);
            Assert.True(data.name == "test");
            return true;
        });

        [Test]
        public async Task DataStackFuncTest()
        {
            var pushRes = await _fpusher.Push(new MsgData() { name = "test" });
            Assert.True(pushRes);
            await Task.Delay(2000);
        }
    }


    public class MsgData
    {
        public string name { get; set; }
    }


    public class MsgPoper : IStackPoper<MsgData>
    {
        public async Task<bool> Pop(MsgData data)
        {
            await Task.Delay(1000);
            Assert.True(data.name == "test");
            return true;
        }
    }
}
