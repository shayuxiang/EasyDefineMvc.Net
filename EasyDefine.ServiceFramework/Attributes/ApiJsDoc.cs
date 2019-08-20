using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.ServiceFramework.Attributes
{
    /// <summary>
    /// 标注为API文档的方法-Doc
    /// 用于反馈API Action的调用文档模型
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiJsDoc: ScriptMethod
    {
        public ApiJsDoc(string RouteName, string HostName)
        {
            this.ParamSetting = (p) => {
                if (p.Length == 1)
                {
                    var p1name = p[0].Name;
                    this.CodeExecutes = new List<CodeExecute>();
                    CodeExecute codeExecute = new CodeExecute();
                    codeExecute.Code = $@"var _ret = EasyDefine.ServiceFramework.Runtime.ApiInfo.GetApis({p1name}).ToDoc(""{RouteName}"",""{HostName}"")";
                    this.Return = new Attributes.Return();
                    this.Return.Var = "ret";
                    this.CodeExecutes.Add(codeExecute);
                }
            };
        }
    }
}
