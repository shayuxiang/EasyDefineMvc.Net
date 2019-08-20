using EasyDefine.Configuration.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace EasyDefine.Configuration.Lib
{
    public class Log
    {
        private static object locker = new object();
        private static ConfigHelper configHelper = new ConfigHelper();
        private static List<LogMessage> logMessages = new List<LogMessage>();
        private static AutoResetEvent exitEvent;
        private static Thread thread;

        public static void LogBegin(int waitTime = 1000) {
            exitEvent = new AutoResetEvent(false);

            thread = new Thread(() => {
                while (true) {
                    #region 执行写入日志
                    var dir = $@"{configHelper.GetTempSourceDir(false)}/../logs/{DateTime.Now.ToString("yyyyMM")}/";
                    var file = $@"{dir}{DateTime.Now.ToString("yyyyMMdd")}.log";
                    //按年月归档文件夹
                    if (!Directory.Exists(dir))
                    {
                        //不存在 则创建路径
                        Directory.CreateDirectory(dir);
                    }
                    Stream stream = null;
                    if (!File.Exists(file))
                    {
                        stream = File.Create(file);
                    }
                    else
                    {
                        stream = new FileStream(file, FileMode.Append);
                    }
                    using (stream)
                    {
                        //写入日志信息堆栈
                        if (logMessages.Count > 0)
                        {
                            lock (locker)
                            {
                                var Message = logMessages[0].ToString();
                                using (StreamWriter sw = new StreamWriter(stream))
                                {
                                    sw.WriteLine(Message);
                                    logMessages.RemoveAt(0);
                                    sw.Close();
                                }
                            }
                        }
                        stream.Close();
                    }
                    #endregion
                    //等待退出
                    if (exitEvent.WaitOne(waitTime)) {
                        break;
                    }
                }
            });
            thread.Start();
        }


        public void EndLog()
        {
            exitEvent.Set();
            thread.Join();
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static bool Write(string Msg, Exception exp = null)
        {
            try
            {
                lock (locker)
                {
                    if (exp == null)
                    {
                        logMessages.Add(new LogMessage { Info = Msg, From = "EasyDefineThread", ex = exp, Level = LogMessageLevel.SystemInformation });
                    }
                    else
                    {
                        logMessages.Add(new LogMessage { Info = Msg, From = "EasyDefineThread", ex = exp, Level = LogMessageLevel.SystemWarring });
                    }
                }
                return true;
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// 系统错误
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static bool Error(string Msg, Exception exp )
        {
            try
            {
                lock (locker)
                {
                    logMessages.Add(new LogMessage { Info = Msg, From = "EasyDefineThread", ex = exp, Level = LogMessageLevel.SystemError });
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Msg"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static bool Write<T>(string Msg, Exception exp = null)
        {
            try
            {
                lock (locker)
                {
                    if (exp == null)
                    {
                        logMessages.Add(new LogMessage { Info = Msg, From = typeof(T).ToString(), ex = exp, Level = LogMessageLevel.SystemInformation });
                        }
                    else
                    {
                        logMessages.Add(new LogMessage { Info = Msg, From = typeof(T).ToString(), ex = exp, Level = LogMessageLevel.SystemWarring });
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 用户错误
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static bool Error<T>(string Msg, Exception exp)
        {
            try
            {
                lock (locker)
                {
                    logMessages.Add(new LogMessage { Info = Msg, From = typeof(T).ToString(), ex = exp, Level = LogMessageLevel.SystemError });
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
