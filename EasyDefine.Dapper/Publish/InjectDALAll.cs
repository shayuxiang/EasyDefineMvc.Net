using EasyDefine.Configuration.Runtime;
using EasyDefine.Dapper.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyDefine.Dapper.Publish
{
    /// <summary>
    /// 注入所有的DAL类
    /// </summary>
    public class InjectDALAll
    {
        /// <summary>
        /// 单例注入所有
        /// </summary>
        private static InjectDALAll _Instances = default(InjectDALAll);
        public static InjectDALAll Instances
        {
            get
            {
                return _Instances ?? (_Instances = new InjectDALAll());
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
            Console.WriteLine("EasyDefine正在编译[DAL]...");
            buildAll.Key.InvokeMember("CompilerAllClass", BindingFlags.Default | BindingFlags.InvokeMethod, null, buildAll.Value, null);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("info:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("EasyDefine正在注入[DAL]映射...");
            //注入映射
            CreateImplements(ass, services);
        }

        /// <summary>
        /// 代码生成
        /// </summary>
        private KeyValuePair<Type,object> CreateTempCode(Assembly ass) {
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
                    Console.WriteLine("EasyDefine正在生成[DAL]接口实例:" + _ref.Name);
                    //预编译数据接口
                    var runnerType = typeof(DataQueryRunner<>);
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
        private void CreateImplements(Assembly ass, IServiceCollection services) {
            //遍历接口
            foreach (var _ref in ass.GetTypes())
            {
                if (_ref.IsInterface)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("info:");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    //是接口类型
                    Console.WriteLine("EasyDefine正在映射[DAL]接口实例:" + _ref.Name);
                    //预编译数据接口
                    var runnerType = typeof(DataQueryRunner<>);
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
            Console.WriteLine("EasyDefine数据访问层[DAL]映射完成,在" + ass.FullName);
        }

    }
}
