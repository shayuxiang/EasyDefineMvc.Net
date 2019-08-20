using EasyDefine.ServiceFramework.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyDefine.ServiceFramework
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =true)]
    public class ScriptMethod : Attribute
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// 方法描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 返回类型
        /// </summary>
        public Type ReturnType { get; internal set; }

        /// <summary>
        /// 方法参数
        /// </summary>
        private ParameterInfo[] _ParamInfos = default(ParameterInfo[]);
        public ParameterInfo[] ParamInfos
        {
            get { return _ParamInfos; }
            internal set
            {
                ParamSetting?.Invoke(value);
                _ParamInfos = value;
            }
        }

        public Action<ParameterInfo[]> ParamSetting;

        /// <summary>
        /// 方法内执行代码
        /// </summary>
        public List<CodeExecute> CodeExecutes { get; internal set; } = null;

        /// <summary>
        /// 方法内返回
        /// </summary>
        public Return Return { get; internal set; }

        public override string ToString()
        {
            return $@"Method:{Name},Describe:{Describe}";
        }
    }

}
