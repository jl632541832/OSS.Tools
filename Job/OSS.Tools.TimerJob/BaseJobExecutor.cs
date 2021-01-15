using System.Threading;
using System.Threading.Tasks;

namespace OSS.Tools.TimerJob
{
 
    /// <summary>
    /// 任务基类
    ///       如果执行时间过长，重复触发时 当前任务还在进行中，则不做任何处理
    /// </summary>
    public abstract class BaseJobExecutor : IJobExecutor
    {
        /// <summary>
        ///  运行状态
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        ///  工作名称
        /// </summary>
        public string JobName { get; }

        /// <summary>
        ///   开始任务
        /// </summary>
        public async Task StartJob(CancellationToken cancellationToken)
        {
            //  任务依然在执行中，不需要再次唤起
            if (IsRunning)
                return;

            IsRunning = true;
            if (!cancellationToken.IsCancellationRequested)
            {
                await Execute();
            }
            IsRunning = false;
        }



        /// <summary>
        ///  任务执行
        /// </summary>
        protected abstract Task Execute();



        /// <summary>
        /// 结束任务
        /// </summary>
        public Task StopJob(CancellationToken cancellationToken)
        {
            IsRunning = false;
            return Task.CompletedTask;
        }
    }
}
