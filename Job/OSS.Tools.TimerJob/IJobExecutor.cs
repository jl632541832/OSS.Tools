using System;
using System.Threading;
using System.Threading.Tasks;

namespace OSS.Tools.TimerJob
{
    /// <summary>
    ///  任务 提供者 接口
    /// </summary>
    public interface IJobExecutor
    {
        /// <summary>
        ///  运行状态
        /// </summary>
        StatusFlag StatusFlag { get; }

        /// <summary>
        ///  工作名称
        /// </summary>
        string JobName { get; }

        /// <summary>
        /// 开始任务
        /// </summary>
        Task StartJob(CancellationToken cancellationToken);

        /// <summary>
        ///  结束任务
        /// </summary>
        Task StopJob(CancellationToken cancellationToken);
    }


    public enum StatusFlag
    {
        Waiting,
        Running,
        Stopping,
        Stopped
    }

    internal class InternalExecutor : BaseJobExecutor
    {
        private readonly Func<CancellationToken, Task> _startAction;
        private readonly Func<CancellationToken, Task> _stopAction;
           
        /// <inheritdoc />
        public InternalExecutor(string jobName, Func<CancellationToken, Task> startAction, Func<CancellationToken, Task> stopAction)
        {
            _startAction = startAction;
            _stopAction = stopAction;
            JobName = jobName;
        }
            
        protected override Task Executing(CancellationToken cancellationToken)
        {
            return _startAction?.Invoke(cancellationToken) ?? Task.CompletedTask;
        }

        protected override Task Stopped(CancellationToken cancellationToken)
        {
            return _stopAction?.Invoke(cancellationToken) ??Task.CompletedTask;
        }
    }
}
