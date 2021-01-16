using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OSS.Tools.TimerJob
{
 
    /// <summary>
    ///  列表循环处理任务执行
    ///  从 GetExecuteSource() 获取执行数据源，循环并通过 ExecuteItem() 执行个体任务，直到没有数据源返回
    ///       如果执行时间过长，重复触发时 当前任务还在进行中，则不做任何处理
    /// </summary>
    public abstract class BaseListJobExecutor<IType> : IJobExecutor
    {
        private bool _isRunning = false;
        private bool _jobCommandStarted = false;
        private readonly bool _isExecuteOnce;


        /// <summary>
        ///  列表任务执行者
        /// </summary>
        protected BaseListJobExecutor(string jobName):this(jobName,false)
        {
        }

        /// <summary>
        ///  列表任务执行者
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="getSourceOnce">是否只获取一次数据源</param>
        protected BaseListJobExecutor(string jobName,bool getSourceOnce)
        {
            JobName = jobName;
            _isExecuteOnce = getSourceOnce;
        }

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

        #region 任务接口实现

        /// <summary>
        ///   开始任务
        /// </summary>
        public async Task StartJob(CancellationToken cancellationToken)
        {
            //  任务依然在执行中，不需要再次唤起
            if (_isRunning)
                return;

            _isRunning = _jobCommandStarted = true;

            var pageIndex = 0;
            IList<IType> list; // 结清实体list

            await OnBegin();
            while (IsStillRunning(cancellationToken)
                   && (list = await GetExecuteSource(pageIndex++))?.Count > 0)
            {
                for (var i = 0; IsStillRunning(cancellationToken) && i < list?.Count; i++)
                {
                    await ExecuteItem(list[i], i);
                }

                if (_isExecuteOnce)
                {
                    break;
                }
            }

            await OnEnd();
            _isRunning = false;
        }



        /// <summary>
        /// 结束任务
        /// </summary>
        public Task StopJob(CancellationToken cancellationToken)
        {
            _jobCommandStarted = false;
            return Task.CompletedTask;
        }

        /// <summary>
        /// 结束任务
        /// </summary>
        public Task StopJob()
        {
            return StopJob(CancellationToken.None);
        }

        #endregion

        #region 扩展方法

        /// <summary>
        /// 获取list数据源, 此方法会被循环调用
        /// </summary>
        /// <param name="pageIndex">获取数据源的页数索引，从0开始</param>
        /// <returns></returns>
        protected abstract Task<IList<IType>> GetExecuteSource(int pageIndex);

        /// <summary>
        ///  个体任务执行
        /// </summary>
        /// <param name="item">单个实体</param>
        /// <param name="index">在数据源中的索引</param>
        protected abstract Task ExecuteItem(IType item, int index);


        /// <summary>
        ///  此轮任务开始
        /// </summary>
        protected virtual Task OnBegin()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  此轮任务结束
        /// </summary>
        protected virtual Task OnEnd()
        {
            return Task.CompletedTask;
        }

        #endregion


        private bool IsStillRunning(CancellationToken cancellationToken)
        {
            return _jobCommandStarted
                   && !cancellationToken.IsCancellationRequested;
        }

    }
}
