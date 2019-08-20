using EasyDefine.ServiceFramework.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyDefine.ServiceFramework.Publish
{
    public class InjectSOAAll
    {
        /// <summary>
        /// 单例注入所有
        /// </summary>
        private static InjectSOAAll _Instances = default(InjectSOAAll);
        public static InjectSOAAll Instances
        {
            get
            {
                return _Instances ?? (_Instances = new InjectSOAAll());
            }
        }

        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="service"></param>
        public void Register(Assembly ass, IServiceCollection services)
        {
            var buildAll = CreateTempCode(ass);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("info:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            //编译全部代码
            Console.WriteLine("EasyDefine正在编译[SOA]...");
            try
            {
                Console.WriteLine(buildAll.Key);
                buildAll.Key.InvokeMember("CompilerAllClass", BindingFlags.Default | BindingFlags.InvokeMethod, null, buildAll.Value, null);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("info:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("EasyDefine正在注入[SOA]映射...");
            //注入映射
            CreateImplements(ass, services);
        }

        /// <summary>
        /// 代码生成
        /// </summary>
        private KeyValuePair<Type, object> CreateTempCode(Assembly ass)
        {
            //返回其中一个编译组件即可
            Type retType = default(Type);
            object retRunner = null;
            //遍历接口
            foreach (var _ref in ass.GetTypes())
            {
                if (_ref.IsInterface)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("info:");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    //是接口类型
                    Console.WriteLine("EasyDefine正在生成[SOA]接口实例:" + _ref.Name);
                    //预编译数据接口
                    var runnerType = typeof(SerivceRunner<>);
                    //创建泛型
                    runnerType = runnerType.MakeGenericType(_ref);
                    //创建编译对象
                    object runner = Activator.CreateInstance(runnerType);
                    //编译成代码
                    runnerType.InvokeMember("BuildInMemory", BindingFlags.Default | BindingFlags.InvokeMethod, null, runner, null);
                    retType = runnerType;
                    retRunner = runner;
                }
            }
            return new KeyValuePair<Type, object>(retType, retRunner);
        }

        /// <summary>
        /// 生成实例
        /// </summary>
        private void CreateImplements(Assembly ass, IServiceCollection services)
        {
            //遍历接口
            foreach (var _ref in ass.GetTypes())
            {
                if (_ref.IsInterface)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("info:");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    //是接口类型
                    Console.WriteLine("EasyDefine正在映射[SOA]接口实例:" + _ref.Name);
                    //预编译数据接口
                    var runnerType = typeof(SerivceRunner<>);
                    //创建泛型
                    runnerType = runnerType.MakeGenericType(_ref);
                    //创建对象
                    object runner = Activator.CreateInstance(runnerType);
                    //获取实体类类型
                    Type ImplementClassType = (Type)runnerType.InvokeMember("GetImplementClassType", BindingFlags.Default | BindingFlags.InvokeMethod, null, runner, null);
                    //注入
                    services.AddScoped(_ref, ImplementClassType);
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("info:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("EasyDefine实时服务层[SOA]映射完成,在" + ass.FullName);
        }

    }
}

