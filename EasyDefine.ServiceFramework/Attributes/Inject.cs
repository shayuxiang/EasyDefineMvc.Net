using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.ServiceFramework
{
    /// <summary>
    /// 获取数据层接入
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface,AllowMultiple =true)]
    public class Inject : Attribute
    {
        /// <summary>
        /// 变量名
        /// </summary>
        private string _variable = ""; 
        public string VariableName
        {
            get
            {
                return $"_{_variable}";
            }
            set
            {
                _variable = value;
            }
        }

        /// <summary>
        /// 数据操作引用实例名称
        /// </summary>
        public Type Ref = null;
    }
}
