using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.ServiceFramework.Runtime
{
    /// <summary>
    /// 自定义代码对象
    /// </summary>
    internal class CustomCodeEntity
    {
        public CodeSnippetStatement UserCode { get; set; }

        public string MethodName { get; set; }
    }
}
