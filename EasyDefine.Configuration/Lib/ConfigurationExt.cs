using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Configuration.Lib
{

    public static class ConfigurationExt
    {
        /// <summary>
        /// 启动EasyDefine的日志捕获
        /// </summary>
        /// <param name="services"></param>
        public static void AddEasyDefineLog(this IServiceCollection services)
        {
            Log.LogBegin();
        }

        /// <summary>
        /// 附加用户信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="Message"></param>
        /// <param name="exp"></param>
        public static void EdLog<T>(this Controller controller, string Message, Exception exp = null)
        {
            Log.Write<T>(Message, exp);
        }

        /// <summary>
        /// 附加用户错误捕获
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="Message"></param>
        /// <param name="exp"></param>
        public static void EdError<T>(this Controller controller, string Message, Exception exp = null)
        {
            Log.Error<T>(Message, exp);
        }
    }
}
