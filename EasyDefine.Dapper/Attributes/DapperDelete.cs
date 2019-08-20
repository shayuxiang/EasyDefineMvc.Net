using EasyDefine.Configuration;
using EasyDefine.Dapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyDefine.Dapper.Attributes
{
    /// <summary>
    /// 删除类标记
    /// </summary>
    public class DapperDelete: DapperCommand
    {
        /// <summary>
        /// 需要操作的表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 这个属性在这里并不用来显示或不显示插入的主键，而是用于标记和判断返回值类型
        /// </summary>
        private bool IsIdentity { get; set; } = false;

        /// <summary>
        /// 生成查询的代码
        /// </summary>
        /// <returns></returns>
        public override string GetSQLCode()
        {
            var cmd = ""; //代码语句
            //判断是否返回插入的值
            if (this.ReturnType == typeof(DeleteResult) || this.ReturnType == typeof(Task<DeleteResult>))
            {
                IsIdentity = true;
            }
            else if (this.ReturnType == typeof(int) || this.ReturnType == typeof(Task<int>))
            {
                IsIdentity = false;
            }
            else
            {
                //非法返回
                return string.Empty;
            }
            var WhereExp = "";
            //参数判断
            if (this.ParamInfos != null && this.ParamInfos.Length > 0)
            {
                //返回是Task，在判断是否需要分页
                var GenericType = this.ParamInfos[0].ParameterType; //获取泛型类型
                if (GenericType.GUID == typeof(string).GUID)
                {
                    WhereExp += $@" $@"" {{{this.ParamInfos[0].Name}}} "" ";
                    //执行UPDATE
                    string isIdentity = IsIdentity ? "true" : "false";
                    //判断返回类型 异步的
                    if (this.ReturnType.GetGenericArguments().Length > 0 && this.ReturnType.BaseType == typeof(Task))
                    {
                        cmd += $@"DatabaseContext<dynamic> _dbc = new DatabaseContext<dynamic>(SourcePointEnum.{this.SourcePointEnum},{this.SlaveId});";
                        if (IsTrans)
                        {
                            cmd += $@"var u = await _dbc.DeleteEntitiesAsync(""{TableName}"", {WhereExp},{TransVariName},{TransVariName}.Connection);";
                        }
                        else
                        {
                            cmd += $@"var u = await _dbc.DeleteEntitiesAsync(""{TableName}"", {WhereExp});";
                        }
                        if (IsIdentity) { cmd += "return u;"; } else { cmd += "return u.ResultCount;"; }
                    }
                    else
                    {
                        cmd += $@"DatabaseContext<dynamic> _dbc = new DatabaseContext<dynamic>(SourcePointEnum.{this.SourcePointEnum},{this.SlaveId});";
                        if (IsTrans)
                        {
                            cmd += $@"var u = _dbc.DeleteEntities(""{TableName}"", {WhereExp},{TransVariName},{TransVariName}.Connection);";
                        }
                        else
                        {
                            cmd += $@"var u = _dbc.DeleteEntities(""{TableName}"", {WhereExp});";
                        }
                        if (IsIdentity) { cmd += "return u;"; } else { cmd += "return u.ResultCount;"; }
                    }
                }
                return cmd;
            }
            return string.Empty;
        }
    }
}
