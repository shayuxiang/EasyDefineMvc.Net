using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.ServiceFramework.Attributes
{
    /// <summary>
    /// 生成Vue.js的头引用文件
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiVue : ScriptMethod
    {

        public ApiVue(string RouteName,string HostName,bool IsAuth = false)
        {
            this.ParamSetting = (p) => {
                if (p.Length == 1)
                {
                    var p1name = p[0].Name;
                    this.CodeExecutes = new List<CodeExecute>();
                    CodeExecute codeExecute = new CodeExecute();
                    codeExecute.Code = $@"var _ret = EasyDefine.ServiceFramework.Runtime.ApiInfo.GetApis({p1name}).ToVue(""{RouteName}"",""{HostName}"",{IsAuth.ToString().ToLower()})";
                    this.Return = new Attributes.Return();
                    this.Return.Var = "ret";
                    this.CodeExecutes.Add(codeExecute);
                }
            };
        }
    }
}
