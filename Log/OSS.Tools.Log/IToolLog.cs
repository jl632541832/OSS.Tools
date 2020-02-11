﻿#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  日志插件接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;

namespace OSS.Tools.Log
{
    /// <summary>
    /// 日志写实现接口
    /// </summary>
    public interface IToolLog
    {
        /// <summary>
        ///   日志写功能
        /// </summary>
        ///<param name="info">日志实体</param>
        Task WriteLogAsync(LogInfo info);
    }
}