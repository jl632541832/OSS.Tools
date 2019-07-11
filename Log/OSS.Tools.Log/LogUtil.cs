#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  日志插件实体及辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using System;
using OSS.Common.Plugs;

namespace OSS.Tools.Log
{

    /// <summary>
    /// 日志写模块
    /// </summary>
    public static class LogUtil
    {
        private static readonly DefaultLogPlug defaultCache = new DefaultLogPlug();

        /// <summary>
        ///   日志模块提供者
        /// </summary>
        public static Func<string, ILogPlug> LogWriterProvider { get; set; }

        /// <summary>
        /// 通过模块名称获取日志模块实例
        /// </summary>
        /// <param name="logModule"></param>
        /// <returns></returns>
        public static Tools.Log.ILogPlug GetLogWrite(string logModule)
        {
            if (string.IsNullOrEmpty(logModule))
                logModule = ModuleNames.Default;

            return LogWriterProvider?.Invoke(logModule) ?? defaultCache;
        }

        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="msg"> 日志信息  </param>
        /// <param name="msgKey">  关键值  </param>
        /// <param name="moduleName"> 模块名称 </param>
        public static string Info(object msg, string msgKey = null, string moduleName = ModuleNames.Default)
        {
            return Log(new LogInfo(LogLevelEnum.Info, msg, msgKey, moduleName));
        }

        /// <summary>
        /// 记录警告，用于未处理异常的捕获
        /// </summary>
        /// <param name="msg"> 日志信息  </param>
        /// <param name="msgKey">  关键值  </param>
        /// <param name="moduleName">模块名称</param>
        public static string Warning(object msg, string msgKey = null, string moduleName = ModuleNames.Default)
        {
            return Log(new LogInfo(LogLevelEnum.Warning, msg, msgKey, moduleName));
        }

        /// <summary>
        /// 记录错误，用于捕获到的异常信息记录
        /// </summary>
        /// <param name="msg"> 日志信息  </param>
        /// <param name="msgKey">  关键值  </param>
        /// <param name="moduleName">模块名称</param>
        public static string Error(object msg, string msgKey = null, string moduleName = ModuleNames.Default)
        {
            return Log(new LogInfo(LogLevelEnum.Error, msg, msgKey, moduleName));
        }

        /// <summary>
        /// 记录错误，用于捕获到的异常信息记录
        /// </summary>
        /// <param name="msg"> 日志信息  </param>
        /// <param name="msgKey">  关键值  </param>
        /// <param name="moduleName">模块名称</param>
        public static string Trace(object msg, string msgKey = null, string moduleName = ModuleNames.Default)
        {
            return Log(new LogInfo(LogLevelEnum.Trace, msg, msgKey, moduleName));
        }


        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="info"></param>
        private static string Log(LogInfo info)
        {
            if (string.IsNullOrEmpty(info.ModuleName))
                info.ModuleName = ModuleNames.Default;

            var logWrite = GetLogWrite(info.ModuleName);

            logWrite.SetLogCode(info);
            logWrite.WriteLog(info);

            return info.LogCode;
        }

    }
}
