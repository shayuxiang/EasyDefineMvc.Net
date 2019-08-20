using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.ServiceFramework
{
    public class CodeExecute:Attribute
    {
        /// <summary>
        /// 变量名
        /// </summary>
        public string Var { get; set; }

        /// <summary>
        /// 执行代码
        /// </summary>
        public string Code { get; set; }
    }
}
