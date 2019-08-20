using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EasyDefine.Configuration.Runtime
{
    public class ConfigHelper
    {
        //动态编译后服务层的输出
        public static string Srv_OutputRoot
        {
            get
            {
                var config = new JsonConfigurationHelper();
                var setting = config.GetAppSettings<EasyDefineSetting>("EasyDefineSetting");
                if (setting == null) return string.Empty;
                return setting.SOASolution;
            }
        }
        //动态编译后数据层输出
        public static string Dbc_OutputRoot
        {
            get
            {
                var config = new JsonConfigurationHelper();
                var setting = config.GetAppSettings<EasyDefineSetting>("EasyDefineSetting");
                if (setting == null) return string.Empty;
                return setting.DALSolution;
            }
        }


        //动态编译后数据层暑促
        public static string Dto_OutputRoot
        {
            get
            {
                var config = new JsonConfigurationHelper();
                var setting = config.GetAppSettings<EasyDefineSetting>("EasyDefineSetting");
                if (setting == null) return string.Empty;
                return setting.DtoSolution;
            }
        }

        //主库连接字符串
        private static string _masterstring = string.Empty;
        public static string MasterConnectionString
        {
            get
            {
                {
                    var config = new JsonConfigurationHelper();
                    var setting = config.GetAppSettings<EasyDefineSetting>("EasyDefineSetting");
                    if (setting == null) return string.Empty;
                    return _masterstring = setting.MasterDb;
                }
            }
        }
        private static List<string> _slavestring = new List<string>();
        //从库连接字符串
        public static List<string> SlaveConnectionString
        {
            get
            {
                var config = new JsonConfigurationHelper();
                var setting = config.GetAppSettings<EasyDefineSetting>("EasyDefineSetting");
                return _slavestring = setting.SlaveDb;
            }
        }

        /// <summary>
        /// 获取关联文件绝对路径
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        //public string GetScriptFileText(string Name) {
        //    try
        //    {
        //        var assembly = Assembly.LoadFrom($@"{AppContext.BaseDirectory}/../{SolutionRoot}.dll");
        //        Stream istr = assembly.GetManifestResourceStream($@"{SolutionRoot}.Test.test.xml");
        //        System.IO.StreamReader sr = new System.IO.StreamReader(istr);
        //        string str = sr.ReadToEnd();
        //        return str;
        //    }
        //    catch {
        //        throw new Exception { Source = $@"找不到绑定的项目映射资源:{SolutionRoot}" };
        //    }
        //}

        /// <summary>
        /// 获取动态编译文件输出的临时文件夹
        /// </summary>
        /// <returns></returns>
        public string GetTempSourceDir(bool isPrint = true) {
            var dir = $@"{AppContext.BaseDirectory}/../Temp";
            if (isPrint)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Temp文件位于:" + dir);
            }
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        public string GetTempSourceDir()
        {
            return GetTempSourceDir(true);
        }

        /// <summary>
        /// 获取编译输出的服务层动态库路径
        /// </summary>
        /// <returns></returns>
        public string GetSrvOutputPath() {
           return $@"{AppContext.BaseDirectory}/../{Srv_OutputRoot}.dll";
        }

        /// <summary>
        /// 获取编译输出的数据层动态库路径
        /// </summary>
        /// <returns></returns>
        public string GetDbcOutputPath()
        {
            return $@"{AppContext.BaseDirectory}/../{Dbc_OutputRoot}.dll";
        }


    }
}
