using EasyDefine.Dapper.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyDefine.Dapper.Interface
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class DapperCommand : Attribute
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// 方法返回
        /// </summary>
        public Type ReturnType { get; internal set; }

        /// <summary>
        /// 方法参数
        /// </summary>
        public ParameterInfo[] ParamInfos { get; internal set; }

        
        /// <summary>
        /// 从库Id
        /// </summary>
        public int SlaveId { get; internal set; } = 1;

        /// <summary>
        /// 数据库来源
        /// </summary>
        public SourcePointEnum SourcePointEnum { get; internal set; }

        /// <summary>
        /// 是否需要封闭在存储过程内执行
        /// </summary>
        public bool IsTrans { get; set; } = false;

        /// <summary>
        /// 存储过程的对象名称
        /// </summary>
        public string TransVariName { get; set; }

        /// <summary>
        /// 虚方法:获取当前的SQL执行代码
        /// </summary>
        /// <returns></returns>
        public virtual string GetSQLCode() {
            return string.Empty;
        }
    }
}
