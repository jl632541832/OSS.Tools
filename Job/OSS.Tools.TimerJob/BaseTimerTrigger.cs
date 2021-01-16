using System;
using System.Threading;
using System.Threading.Tasks;

namespace OSS.Tools.TimerJob
{
    /// <summary>
    ///   定时器基础类
    /// </summary>
    public class BaseTimerTrigger : IDisposable
    {
        private Timer _timer;

        private readonly TimeSpan _dueTime;
        private readonly TimeSpan _periodTime;       
 
        private CancellationToken _cancellationToken=CancellationToken.None;

        /// <summary>
        ///  工作执行者
        /// </summary>
        public IJobExecutor JobExcutor { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dueTime">到期开始执行时间</param>
        /// <param name="periodTime">间隔时间</param>
        /// <param name="jobExcutor">任务执行者</param>
        protected BaseTimerTrigger(TimeSpan dueTime, TimeSpan periodTime, IJobExecutor jobExcutor)
        {
            _dueTime = dueTime;
            _periodTime = periodTime;
            JobExcutor = jobExcutor;
        }

        /// <inheritdoc />
        protected BaseTimerTrigger(TimeSpan dueTime, TimeSpan periodTime,string jobName, Func<CancellationToken, Task> startAction, Func<CancellationToken, Task> stopAction)
        {
            _dueTime = dueTime;
            _periodTime = periodTime;
            JobExcutor = new InternalExecutor(jobName,startAction, stopAction);
        }
        /// <inheritdoc />
        protected BaseTimerTrigger(TimeSpan dueTime, TimeSpan periodTime, string jobName, Func<CancellationToken, Task> startAction)
        {
            _dueTime = dueTime;
            _periodTime = periodTime;
            JobExcutor = new InternalExecutor(jobName, startAction, null);
        }

        #region 扩展方法

        /// <summary>
        /// 指定时分秒和当前的时间差
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        protected static TimeSpan PointTimeSpan(int hour, int minute, int second)
        {
            const int fullDaySeconds = 24 * 60 * 60;

            var now = DateTime.Now;
            var startSeconds = now.Hour * 60 * 60 + now.Minute * 60 + now.Second;
            var endSeconds = (hour * 60 * 60 + minute * 60 + second * 60) % fullDaySeconds;//防止输入溢出一天的周期

            var spanSeconds = endSeconds - startSeconds;
            if (spanSeconds < 0)
                spanSeconds += fullDaySeconds;

            return TimeSpan.FromSeconds(spanSeconds);
        }

        #endregion

        #region  基础方法

        /// <summary>
        ///   配置并触发定时器    
        /// </summary>
        /// <returns></returns>
        private Task StartTimerTrigger(CancellationToken cancellationToken)
        {
            if (cancellationToken!=CancellationToken.None)
            {
                _cancellationToken = cancellationToken;
            }

            if (_timer == null)
                _timer = new Timer(ExcuteJob, null, _dueTime, _periodTime);
            else
                _timer.Change(_dueTime, _periodTime);

            return Task.CompletedTask;
        }

        /// <summary>
        ///  停止定时器
        /// </summary>
        private void StopTimerTrigger()
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        }



        /// <inheritdoc />
        public void Dispose()
        {
            _timer?.Dispose();
        }

        #endregion

        /// <summary>
        ///  系统级任务执行启动
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            return StartTimerTrigger(cancellationToken);
        }

        /// <summary>
        ///  系统级任务执行关闭
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            await JobExcutor.StopJob(cancellationToken);
            StopTimerTrigger();
        }

        private void ExcuteJob(object obj)
        {
            JobExcutor?.StartJob(_cancellationToken).Wait(_cancellationToken);
        }




    }
}
