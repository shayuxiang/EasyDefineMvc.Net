using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.ServiceFramework.Attributes
{
    /// <summary>
    /// 接口方法的返回
    /// </summary>
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = false)]
    public class Return : Attribute
    {
        /// <summary>
        /// 返回的变量
        /// </summary>
        public string Var { get; set; } 
    }
}
