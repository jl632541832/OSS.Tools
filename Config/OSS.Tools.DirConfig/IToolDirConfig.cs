﻿#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  配置插件接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion


using System.Threading.Tasks;

namespace OSS.Tools.DirConfig
{
    /// <summary>
    /// 字典配置接口
    /// </summary>
    public interface IToolDirConfig
    {
        /// <summary>
        /// 添加字典配置
        /// </summary>
        /// <param name="key">配置关键字</param>
        /// <param name="dirConfig">配置具体信息</param>
        /// <typeparam name="TConfig">配置信息类型</typeparam>
        /// <returns></returns>
        Task<bool> SetDirConfig<TConfig>(string key, TConfig dirConfig) where TConfig : class ,new();


        /// <summary>
        /// 添加字典配置
        /// </summary>
        /// <param name="key">配置关键字</param>
        /// <typeparam name="TConfig">配置信息类型</typeparam>
        /// <returns></returns>
        Task<TConfig> GetDirConfig<TConfig>(string key) where TConfig : class ,new();

        /// <summary>
        /// 移除配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveDirConfig(string key);
    }
}