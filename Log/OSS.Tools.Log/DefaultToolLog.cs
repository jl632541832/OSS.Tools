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

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OSS.Tools.Log
{
    /// <summary>
    /// 系统默认写日志模块
    /// </summary>
    public class DefaultToolLog : IToolLog
    {
        private readonly string _logBaseDirPath = null;
        private const string _logFormat = "{0:T}    Code:{1}    Key:{2}   Detail:{3}\r\n";

        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultToolLog()
        {
            // todo  测试地址是否ok
            _logBaseDirPath = Path.Combine(Directory.GetCurrentDirectory(), "log");

            if (!Directory.Exists(_logBaseDirPath))
                Directory.CreateDirectory(_logBaseDirPath); 
        }

        private string getLogFilePath(string module, LogLevelEnum level)
        {
            var date = DateTime.Now;
            var dirPath = Path.Combine(_logBaseDirPath,string.Concat(module,"_",level) ,DateTime.Now.ToString("yyMM"));//string.Format(@"{0}\{1}\{2}\",_logBaseDirPath, module, level);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            return Path.Combine(dirPath, date.ToString("MMddHH")+( date.Minute>30?"00":"30")+ ".txt");
        }

        private static readonly object obj = new object();

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="info"></param>
        public void WriteLog(LogInfo info)
        {
            Task.Run(() =>
            {
                lock (obj)
                {
                    var filePath = getLogFilePath(info.module_name, info.level);

                    using (StreamWriter sw =
                        new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.Write), Encoding.UTF8))
                    {
                        sw.WriteLine(_logFormat,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            info.log_id,info.msg_key,info.msg_body);
                    }
                }
            });
        }
    }
}
