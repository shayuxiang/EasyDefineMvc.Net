using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Configuration.Lib
{
    public class LogMessage
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string From { get; set; }

        public LogMessageLevel Level { get; set; } = LogMessageLevel.UserInfomation;

        /// <summary>
        /// 系统错误
        /// </summary>
        public Exception ex { get; set; }

        public override string ToString()
        {
            switch (Level)
            {
                case LogMessageLevel.SystemError:
                    return $@"ELog-System-Error[{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}]:{Info},Source:{From},{ex.Message}";
                case LogMessageLevel.SystemInformation:
                    return $@"ELog-System-Info[{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}]:{Info},Source:{From}";
                case LogMessageLevel.SystemWarring:
                    return $@"ELog-System-Info[{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}]:{Info},Source:{From}";
                case LogMessageLevel.UserError:
                    return $@"ELog-User-Error[{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}]:{Info},Source:{From},{ex.Message}";
                case LogMessageLevel.UserInfomation:
                    return $@"ELog-User-Info[{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}]:{Info},Source:{From}";
                case LogMessageLevel.UserWarring:
                    return $@"ELog-System-Info[{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}]:{Info},Source:{From}";
            }
            return string.Empty;
        }
    }

    public enum LogMessageLevel
    {
        SystemInformation = 0,
        SystemWarring = 1,
        SystemError = 2,
        UserInfomation = 3,
        UserWarring = 4,
        UserError = 5
    }
}
