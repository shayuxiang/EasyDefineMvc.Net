
using Dapper;
using EasyDefine.Configuration;
using EasyDefine.Dapper.Core;
using EasyDefine.Dapper.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace EasyDefine.Dapper.Attributes
{
    /// <summary>
    /// 查询类标记
    /// </summary>
    public class DapperQuery : DapperCommand
    {
        /// <summary>
        /// 查询指令
        /// </summary>
        public string QueryCommand { get; set; }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <returns></returns>
        public override string GetSQLCode()
        {
            string cmd = "";
            #region 添加参数
            string param = "";
            object pageSize = "20";
            object pageIndex = "1";
            if (this.ParamInfos != null && this.ParamInfos.Length > 0)
            {
                param = $@"DynamicParameters  paras = new DynamicParameters();";
                foreach (var p in this.ParamInfos)
                {
                    if (p.Name.ToLower() == "pagesize")
                    {
                        pageSize = $@"{p.Name}";
                    }
                    else if (p.Name.ToLower() == "pageindex")
                    {
                        pageIndex = $@"{p.Name}";
                    }
                    else if (p.Name.StartsWith("__")) {
                        QueryCommand = QueryCommand.Replace("#" + p.Name.Replace("__", ""), $@"{{{p.Name}}}");
                    }
                    else
                    {
                        if (p.ParameterType.GUID != typeof(IDbTransaction).GUID)
                        {
                            param += $@"paras.Add(""@{p.Name}"", {p.Name});
                            Console.WriteLine($@""输入{p.Name}参数:""+{p.Name});";
                        }
                    }
                }
                //param = $@"Console.WriteLine(""typeof(DynamicParameters)"");";
            }

            #endregion
            var paramstr = string.IsNullOrEmpty(param) ? "null" : "paras";
            //判断返回类型 异步的
            if (this.ReturnType.GetGenericArguments().Length > 0 && this.ReturnType.BaseType == typeof(Task))
            {
                //返回是Task，在判断是否需要分页
                var GenericType = this.ReturnType.GetGenericArguments()[0]; //获取泛型类型
                if (GenericType.GUID == typeof(PagedListResult<>).GUID) //是分页类型
                {
                    var RealGenericType = GenericType.GetGenericArguments()[0]; //获取真实的返回对象
                    //返回类型有泛型
                    cmd = $@"
                    {param}
                    DatabaseContext<{RealGenericType.FullName}> _dbc = new DatabaseContext<{RealGenericType.FullName}>(SourcePointEnum.{this.SourcePointEnum},{this.SlaveId});";
                    if (IsTrans)
                    {
                        cmd += $@"return await _dbc.QueryPagedAsync($@""{QueryCommand}"", {pageIndex}, {pageSize}, {paramstr},{TransVariName},{TransVariName}.Connection);";
                    }
                    else {
                        cmd += $@"return await _dbc.QueryPagedAsync($@""{QueryCommand}"", {pageIndex}, {pageSize}, {paramstr});";
                    }
                }
                else if (GenericType.GUID == typeof(IEnumerable<>).GUID || GenericType.GUID == typeof(List<>).GUID)
                {
                    var RealGenericType = GenericType.GetGenericArguments()[0]; //获取真实的返回对象
                    //是其他集合类型
                    cmd = $@"
                    {param}
                    DatabaseContext<{RealGenericType.FullName}> _dbc = new DatabaseContext<{RealGenericType.FullName}>(SourcePointEnum.{this.SourcePointEnum},{this.SlaveId});";
                    if (IsTrans)
                    {
                        cmd += $@"var ret = await _dbc.QueryAsync($@""{QueryCommand}"",{paramstr},{TransVariName},{TransVariName}.Connection);return ret.AsList();";
                    }
                    else
                    {
                        cmd += $@"var ret = await _dbc.QueryAsync($@""{QueryCommand}"", {paramstr});return ret.AsList();";
                    }
                }
                else {
                    //简单值类型，查单个
                    cmd = $@"
                    {param}
                    DatabaseContext<{GenericType.FullName}> _dbc = new DatabaseContext<{GenericType.FullName}>(SourcePointEnum.{this.SourcePointEnum},{this.SlaveId});";
                    if (IsTrans)
                    {
                        cmd += $@"return await _dbc.QueryFirstAsync($@""{QueryCommand}"", {paramstr},{TransVariName},{TransVariName}.Connection);";
                    }
                    else
                    {
                        cmd += $@"return await _dbc.QueryFirstAsync($@""{QueryCommand}"", {paramstr});";
                    }
                }
            }
            else {
                //同步的
                //返回是Task，在判断是否需要分页
                var GenericType = this.ReturnType; //获取泛型类型
                if (GenericType.GUID == typeof(PagedListResult<>).GUID) //是分页类型
                {
                    //分页变量
                    var RealGenericType = GenericType.GetGenericArguments()[0]; //获取真实的返回对象
                    //返回类型有泛型
                    cmd = $@"
                    {param}
                    DatabaseContext<{RealGenericType.FullName}> _dbc = new DatabaseContext<{RealGenericType.FullName}>(SourcePointEnum.{this.SourcePointEnum},{this.SlaveId});";
                    if (IsTrans)
                    {
                        cmd += $@"return _dbc.QueryPaged($@""{QueryCommand}"", {pageIndex}, {pageSize}, {paramstr},{TransVariName},{TransVariName}.Connection);";
                    }
                    else
                    {
                        cmd += $@" return _dbc.QueryPaged($@""{QueryCommand}"", {pageIndex}, {pageSize}, {paramstr});";
                    }
                }
                else if (GenericType.GUID == typeof(IEnumerable<>).GUID || GenericType.GUID == typeof(List<>).GUID)
                {
                    var RealGenericType = GenericType.GetGenericArguments()[0]; //获取真实的返回对象
                    //是其他集合类型
                    cmd = $@"
                    {param}
                    DatabaseContext<{RealGenericType.FullName}> _dbc = new DatabaseContext<{RealGenericType.FullName}>(SourcePointEnum.{this.SourcePointEnum},{this.SlaveId});";
                    if (IsTrans)
                    {
                        cmd += $@"return _dbc.Query($@""{QueryCommand}"", {paramstr},{TransVariName},{TransVariName}.Connection).AsList();";
                    }
                    else
                    {
                        cmd += $@"return _dbc.Query($@""{QueryCommand}"", {paramstr}).AsList();";
                    }
                }
                else
                {
                    //简单值类型，查单个
                    cmd = $@"
                    {param}
                    DatabaseContext<{GenericType.FullName}> _dbc = new DatabaseContext<{GenericType.FullName}>(SourcePointEnum.{this.SourcePointEnum},{this.SlaveId});";
                    if (IsTrans)
                    {
                        cmd += $@"return _dbc.QueryFirst($@""{QueryCommand}"", {paramstr},{TransVariName},{TransVariName}.Connection);";
                    }
                    else
                    {
                        cmd += $@"return _dbc.QueryFirst($@""{QueryCommand}"", {paramstr});";
                    }
                }
            }
            return $@"try{{
                {cmd}
            }}
            catch(Exception ex){{
                Console.WriteLine(ex);
                throw ex;
            }}";
        }
    }
}
