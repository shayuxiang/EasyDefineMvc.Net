using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.ServiceFramework
{
    /// <summary>
    /// 标注为API文档的VUE方法
    /// 用于反馈API Action的调用文档模型
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiDom : ScriptMethod
    {

        public ApiDom()
        {
            this.ParamSetting = (p)=> {
                if (p.Length == 1)
                {
                    this.CodeExecutes = new List<CodeExecute>();
                    CodeExecute codeExecute = new CodeExecute();
                    codeExecute.Code = $@"var _ret = EasyDefine.ServiceFramework.Runtime.ApiInfo.GetApis({p[0].Name})";
                    this.Return = new Attributes.Return();
                    this.Return.Var = "ret";
                    this.CodeExecutes.Add(codeExecute);
                }
            };
        }
    }
}
