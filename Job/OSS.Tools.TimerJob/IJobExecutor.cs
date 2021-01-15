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
         bool IsRunning { get;  }

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

    internal class InternalExecutor : IJobExecutor
    {
        public string JobName { get; }

        public bool IsRunning { get; private set; }


        private readonly Func<CancellationToken, Task> _startAction;
        private readonly Func<CancellationToken, Task> _stopAction;

        /// <inheritdoc />
        public InternalExecutor(string jobName, Func<CancellationToken, Task> startAction, Func<CancellationToken, Task> stopAction)
        {
            _startAction = startAction;
            _stopAction = stopAction;
            JobName = jobName;
        }

        public Task StartJob(CancellationToken cancellationToken)
        {
            IsRunning = true;
            return _startAction?.Invoke(cancellationToken) ?? Task.CompletedTask;
        }

        public async Task StopJob(CancellationToken cancellationToken)
        {
            await _stopAction?.Invoke(cancellationToken);
            IsRunning = false;
        }
    }
}
