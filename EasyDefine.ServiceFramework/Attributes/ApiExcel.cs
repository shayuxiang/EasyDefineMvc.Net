using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.ServiceFramework
{
    public class ApiExcel : ScriptMethod
    {
        public ApiExcel()
        {
            this.ParamSetting = (p) => {
                if (p.Length == 2)
                {
                    var p1name = p[0].Name.StartsWith("_")? p[0].Name.Replace("_", string.Empty) : p[0].Name;
                    var p2name = p[1].Name.StartsWith("_")? p[1].Name.Replace("_", string.Empty) : p[1].Name;
                    Type generType;
                    if (p[1].ParameterType.GetGenericTypeDefinition().GUID == typeof(IEnumerable<>).GUID ||
                    p[1].ParameterType.GetGenericTypeDefinition().GUID  == typeof(List<>).GUID)
                    {
                        generType = p[1].ParameterType.GetGenericArguments()[0];
                        this.CodeExecutes = new List<CodeExecute>();
                        CodeExecute codeExecute = new CodeExecute();
                        codeExecute.Code += $@"var _ret = (new EasyDefine.ServiceFramework.Runtime.ExcelExport()).AddExcelPage<{generType.FullName}>({p1name},{p2name});";
                        this.Return = new Attributes.Return();
                        this.Return.Var = "ret";
                        this.CodeExecutes.Add(codeExecute);
                    }
                }
            };
        }
    }
}
