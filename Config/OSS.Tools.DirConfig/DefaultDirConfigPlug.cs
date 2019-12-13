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
using OSS.Common.Resp;

namespace OSS.Tools.DirConfig
{

    /// <summary>
    /// 默认配置处理
    /// </summary>
    public class DefaultDirConfigPlug : IDirConfigPlug
    {
        private static string _defaultPath;

        static DefaultDirConfigPlug()
        {
            InitialPath();
        }

        private static void InitialPath()
        {
            var basePat = Directory.GetCurrentDirectory();
            var sepChar = Path.DirectorySeparatorChar;
            var binIndex = basePat.IndexOf(string.Concat(sepChar, "bin", sepChar), StringComparison.OrdinalIgnoreCase);
            if (binIndex > 0)
            {
                basePat = basePat.Substring(0, binIndex);
            }

            _defaultPath = Path.Combine(basePat, _folderName);
            if (!Directory.Exists(_defaultPath))
            {
                Directory.CreateDirectory(_defaultPath);
            }
        }

        private const string _folderName = "Configs";
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
            
            FileStream fs = null;
            try
            {
                var filePath = string.Concat(_defaultPath, Path.DirectorySeparatorChar, key, ".config");
                fs = new FileStream(filePath, FileMode.Create,FileAccess.Write);

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
            try
            {
                var fileFullName = string.Concat(_defaultPath, Path.DirectorySeparatorChar, key, ".config");

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
            var fileName = string.Concat(_defaultPath, Path.DirectorySeparatorChar, key, ".config");
            
            if (!File.Exists(fileName))
                return new Resp(RespTypes.InnerError, "移除字典配置时出错");

            File.Delete(fileName);
            return new Resp();
        }
    }
}
