#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  配置插件默认实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using System;
using System.IO;
using System.Xml.Serialization;
using OSS.Common.ComModels;
using OSS.Common.Resp;

namespace OSS.Tools.DirConfig
{

    /// <summary>
    /// 默认配置处理
    /// </summary>
    public class DefaultDirConfigPlug : IDirConfigPlug
    {
        private static readonly string _defaultPath;

        static DefaultDirConfigPlug()
        {
            var basePat  = Directory.GetCurrentDirectory();
            var sepChar  = Path.DirectorySeparatorChar;
            var binIndex = basePat.IndexOf(string.Concat(sepChar, "bin", sepChar), StringComparison.OrdinalIgnoreCase);
            _defaultPath = binIndex > 0 ? basePat.Substring(0, binIndex) : basePat;
        }


        /// <summary>
        /// 设置字典配置信息
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="key"></param>
        /// <param name="dirConfig"></param>
        /// <returns></returns>
        public Resp SetDirConfig<TConfig>(string key, TConfig dirConfig) where TConfig : class, new()
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "配置键值不能为空！");

            if (dirConfig == null)
                throw new ArgumentNullException("dirConfig", "配置信息不能为空！");

            var path = string.Concat(_defaultPath, Path.DirectorySeparatorChar, "Configs");

            FileStream fs = null;
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                fs = new FileStream(string.Concat(path, Path.DirectorySeparatorChar, key, ".config"), FileMode.Create,
                    FileAccess.Write);

                var type = typeof(TConfig);

                var xmlSer = new XmlSerializer(type);
                xmlSer.Serialize(fs, dirConfig);

                return new Resp();
            }
            finally
            {
                fs?.Dispose();
            }
        }


        /// <summary>
        ///   获取字典配置
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TConfig GetDirConfig<TConfig>(string key) where TConfig : class, new()
        {

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "配置键值不能为空！");

            FileStream fs = null;
            var path = string.Concat(_defaultPath, Path.DirectorySeparatorChar, "Config");

            try
            {
                var fileFullName = string.Concat(path, Path.DirectorySeparatorChar, key, ".config");

                if (!File.Exists(fileFullName))
                    return null;

                fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Read, FileShare.Read);

                var type = typeof(TConfig);

                var xmlSer = new XmlSerializer(type);
                return (TConfig)xmlSer.Deserialize(fs);
            }
            finally
            {
                fs?.Dispose();
            }
        }

        /// <summary>
        ///  移除配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Resp RemoveDirConfig(string key)
        {
            var path = string.Concat(_defaultPath, Path.DirectorySeparatorChar, "Config");
            var fileName = string.Concat(path, Path.DirectorySeparatorChar, key, ".config");
            
            if (!File.Exists(fileName))
                return new Resp(RespTypes.InnerError, "移除字典配置时出错");

            File.Delete(fileName);
            return new Resp();
        }
    }
}
