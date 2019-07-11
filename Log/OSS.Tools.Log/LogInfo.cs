#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  日志插件默认实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using OSS.Common.Plugs;

namespace OSS.Tools.Log
{

    /// <summary>
    /// 日志等级
    /// </summary>
    public enum LogLevelEnum
    {
        /// <summary>
        /// 跟踪查看
        /// </summary>
        Trace,

        /// <summary>
        /// 信息
        /// </summary>
        Info,

        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 警告
        /// </summary>
        Warning,
    }

    /// <summary>
    /// 日志实体
    /// </summary>
    public sealed class LogInfo
    {
        /// <summary>
        /// 空构造函数
        /// </summary>
        public LogInfo()
        {
        }

        /// <summary>
        /// 日志构造函数
        /// </summary>
        /// <param name="loglevel"></param>
        /// <param name="logMsg"></param>
        /// <param name="msgKey"></param>
        /// <param name="moduleName"></param>
        internal LogInfo(LogLevelEnum loglevel, object logMsg, string msgKey = null, string moduleName = ModuleNames.Default)
        {
            Level = loglevel;
            ModuleName = moduleName;
            this.Msg = logMsg;
            MsgKey = msgKey;
        }

        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevelEnum Level { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        ///   key值  可以是自定义的标识  
        ///   根据此字段可以处理当前module下不同复杂日志信息
        /// </summary>
        public string MsgKey { get; set; }

        /// <summary>
        /// 日志信息  可以是复杂类型  如 具体实体类
        /// </summary>
        public object Msg { get; set; }

        /// <summary>
        /// 编号（全局唯一）
        /// </summary>
        public string LogCode { get; set; }
    }

}
