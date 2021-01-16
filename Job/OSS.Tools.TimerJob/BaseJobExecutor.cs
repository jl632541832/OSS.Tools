﻿using System.Threading;
using System.Threading.Tasks;

namespace OSS.Tools.TimerJob
{ 
    /// <summary>
    /// 任务基类
    ///       如果执行时间过长，重复触发时 当前任务还在进行中，则不做任何处理
    /// </summary>
    public abstract class BaseJobExecutor : IJobExecutor
    {
        private bool _isRunning = false;
        private bool _jobCommandStarted = false;

        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; protected set; }

        /// <summary>
        ///  运行状态
        /// </summary>
        public StatusFlag StatusFlag
        {
            get
            {
                if (_jobCommandStarted && _isRunning)
                    return StatusFlag.Running;
                if (_jobCommandStarted && !_isRunning)
                    return StatusFlag.Waiting;

                return StatusFlag.Stoped;
            }
        }


        /// <summary>
        ///   开始任务
        /// </summary>
        public async Task StartJob(CancellationToken cancellationToken)
        {
            //  任务依然在执行中，不需要再次唤起
            if (_isRunning)
                return;

            _isRunning = _jobCommandStarted = true;

            if (!cancellationToken.IsCancellationRequested)
            {
                await Executing(cancellationToken);
            }
            _isRunning = false;
        }



        /// <summary>
        ///  任务执行
        /// </summary>
        protected abstract Task Executing(CancellationToken cancellationToken);

        /// <summary>
        ///  任务停止
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual Task Stopped(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 结束任务
        /// </summary>
        public Task StopJob(CancellationToken cancellationToken)
        {
            _jobCommandStarted = false;
            return Stopped(cancellationToken);
        }
        /// <summary>
        /// 结束任务
        /// </summary>
        public Task StopJob()
        {
            return StopJob(CancellationToken.None);
        }
    }
}
