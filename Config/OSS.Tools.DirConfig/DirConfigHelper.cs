#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

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
using OSS.Common.Plugs;
using OSS.Common.Resp;

namespace OSS.Tools.DirConfig
{
    /// <summary>
    /// 字典配置通用存储获取信息
    /// </summary>
    public static class DirConfigHelper
    {
        private static readonly DefaultToolDirConfig defaultCache = new DefaultToolDirConfig();

        /// <summary>
        ///   配置信息模块提供者
        /// </summary>
        public static Func<string, IToolDirConfig> DirConfigProvider { get; set; }

        /// <summary>
        /// 通过模块名称获取
        /// </summary>
        /// <param name="dirConfigModule"></param>
        /// <returns></returns>
        private static IToolDirConfig GetDirConfig(string dirConfigModule)
        {
            if (string.IsNullOrEmpty(dirConfigModule))
                dirConfigModule = ModuleNames.Default;

            return DirConfigProvider?.Invoke(dirConfigModule) ?? defaultCache;
        }


        /// <summary>
        /// 设置字典配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dirConfig"></param>
        /// <param name="moduleName">模块名称</param>
        /// <typeparam name="TConfig"></typeparam>
        /// <returns></returns>
        public static Resp SetDirConfig<TConfig>(string key, TConfig dirConfig,
            string moduleName = ModuleNames.Default) where TConfig : class, new()
        {
            return GetDirConfig(moduleName).SetDirConfig(key, dirConfig);
        }


        /// <summary>
        ///   获取字典配置
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="key"></param>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static TConfig GetDirConfig<TConfig>(string key,  string moduleName = ModuleNames.Default) where TConfig : class ,new()
        {
            return GetDirConfig(moduleName).GetDirConfig<TConfig>(key);
        }

        /// <summary>
        ///  移除配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static Resp RemoveDirConfig( string key, string moduleName = ModuleNames.Default)
        {
            return GetDirConfig(moduleName).RemoveDirConfig(key);
        }
    }
}