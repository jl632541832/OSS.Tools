

namespace OSS.Tools.DataStack
{
    public interface IDataSackProvider
    {
        IStackPusher<TData> CreateStack<TData>(IStackPoper<TData> poper, string sourceName = "default");
    }
}
