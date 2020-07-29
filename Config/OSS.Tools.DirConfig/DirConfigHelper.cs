﻿#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  配置插件辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using System;

namespace OSS.Tools.DirConfig
{
    /// <summary>
    /// 字典配置通用存储获取信息
    /// </summary>
    public static class DirConfigHelper
    {
        private static readonly DefaultToolDirConfig defaultCache = new DefaultToolDirConfig();

        /// <summary>
        ///   配置信息来源提供者
        /// </summary>
        public static Func<string, IToolDirConfig> DirConfigProvider { get; set; }

        /// <summary>
        /// 来源名称格式化
        /// </summary>
        public static Func<string, string> SourceFormat { get; set; }

        /// <summary>
        /// 通过来源名称获取
        /// </summary>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        private static IToolDirConfig GetDirConfig(string sourceName)
        {
            if (string.IsNullOrEmpty(sourceName))
                sourceName = "default";

            if (SourceFormat != null)
                sourceName = SourceFormat.Invoke(sourceName);

            return DirConfigProvider?.Invoke(sourceName) ?? defaultCache;
        }


        /// <summary>
        /// 设置字典配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dirConfig"></param>
        /// <param name="sourceName">来源名称</param>
        /// <typeparam name="TConfig"></typeparam>
        /// <returns></returns>
        public static bool SetDirConfig<TConfig>(string key, TConfig dirConfig,
            string sourceName = "default") where TConfig : class, new()
        {
            return GetDirConfig(sourceName).SetDirConfig(key, dirConfig);
        }


        /// <summary>
        ///   获取字典配置
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="key"></param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        public static TConfig GetDirConfig<TConfig>(string key,  string sourceName = "default") where TConfig : class ,new()
        {
            return GetDirConfig(sourceName).GetDirConfig<TConfig>(key);
        }

        /// <summary>
        ///  移除配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        public static void RemoveDirConfig( string key, string sourceName = "default")
        {
             GetDirConfig(sourceName).RemoveDirConfig(key);
        }
    }
}