using EasyDefine.ServiceFramework.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.ServiceFramework
{
    /// <summary>
    /// 类标记，用于生成配置文件
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class ScriptClass : Attribute
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public string NameSpace { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 关联的脚本文件路径 禁用
        /// </summary>
        //public string AssociatedScriptRoot { get; set; }

        /// <summary>
        /// 语言,定义全局语言 禁用
        /// </summary>
        //public LanguageType Language { get; set; } = LanguageType.JavaScript;

        /// <summary>
        /// 获取当前接口的所有方法
        /// </summary>
        /// <returns></returns>
        public List<ScriptMethod> GetScriptMethods(Type _interface)
        {
            List<ScriptMethod> methods = new List<ScriptMethod>();
            foreach (var m in _interface.GetMethods())
            {
                var a = m.GetCustomAttributes(typeof(ScriptMethod), true);
                if (a.Length > 0)
                {
                    var _method = a[0] as ScriptMethod;
                    _method.Name = m.Name;
                    _method.ReturnType = m.ReturnType;
                    _method.ParamInfos = m.GetParameters();
                    //添加标注内部代码段
                    if (m.GetCustomAttributes(typeof(CodeExecute), true) != null && m.GetCustomAttributes(typeof(CodeExecute), true).Length > 0)
                    {
                        _method.CodeExecutes = new List<CodeExecute>();
                        foreach (var code in m.GetCustomAttributes(typeof(CodeExecute), true))
                        {
                            _method.CodeExecutes.Add(code as CodeExecute);
                        }
                    }
                    //添加标注返回值
                    if (m.GetCustomAttributes(typeof(Return), true) != null && m.GetCustomAttributes(typeof(Return), true).Length > 0)
                    {
                        var ret = m.GetCustomAttributes(typeof(Return), true);
                        _method.Return = ret[0] as Return;
                    }
                    methods.Add(_method);
                }
            }
            return methods;
        }
    }
}
