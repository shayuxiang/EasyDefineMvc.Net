
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
    /// 新增类标记
    /// </summary>
    public class DapperInsert: DapperCommand
    {
        /// <summary>
        /// 需要操作的表名
        /// </summary>
        public string TableName { get; set; }

        public string Fields { get; set; }
        
        private bool IsIdentity { get; set; } = false;

        /// <summary>
        /// 生成查询的代码
        /// </summary>
        /// <returns></returns>
        public override string GetSQLCode()
        {
            var cmd = ""; //代码语句
            //判断是否返回插入的值
            if (this.ReturnType == typeof(InsertResult)||this.ReturnType == typeof(Task<InsertResult>))
            {
                IsIdentity = true;
            }
            else if (this.ReturnType == typeof(int) || this.ReturnType == typeof(Task<int>))
            {
                IsIdentity = false;
            }
            else {
                //非法返回
                return string.Empty;
            }
            //参数判断
            if (this.ParamInfos != null && this.ParamInfos.Length > 0)
            {
                //判断返回类型 异步的
                if (this.ReturnType.GetGenericArguments().Length > 0 && this.ReturnType.BaseType == typeof(Task))
                {
                    //返回是Task，在判断是否需要分页
                    var GenericType = this.ParamInfos[0].ParameterType; //获取泛型类型
                    if (GenericType.GUID == typeof(IEnumerable<>).GUID || GenericType.GUID == typeof(List<>).GUID)
                    {
                        var RealGenericType = GenericType.GetGenericArguments()[0]; //获取真实的返回对象
                        var addstr = "EasyDefine.Configuration.InsertRequest request = new EasyDefine.Configuration.InsertRequest();";
                        foreach (var f in RealGenericType.GetProperties())
                        {
                            addstr += $@"
                              var field_{f.Name} = new EasyDefine.Configuration.InsertFields {{ Name = ""{f.Name}"",Value = entity.{f.Name} }};
                              request.Fields.Add(field_{f.Name});";
                        }
                        //简单值类型，插入单个
                        cmd = $@"System.Collections.Generic.List<EasyDefine.Configuration.InsertRequest> requests = new System.Collections.Generic.List<EasyDefine.Configuration.InsertRequest>();";
                        //设置循环遍历参数
                        string param_name = this.ParamInfos[0].Name;
                        cmd += $@"foreach(var entity in {param_name}){{ 
                              {addstr}
                              requests.Add(request);
                        }}";
                        string isIdentity = IsIdentity ? "true" : "false";
                        cmd += $@"DatabaseContext<dynamic> _dbc = new DatabaseContext<dynamic>(SourcePointEnum.{this.SourcePointEnum},{this.SlaveId});";
                        if (IsTrans)
                        {
                            cmd += $@"var u = await _dbc.InsertEntitiesAsync(""{TableName}"", requests,{TransVariName},{TransVariName}.Connection,{isIdentity});";
                        }
                        else
                        {
                            cmd += $@"var u = await _dbc.InsertEntitiesAsync(""{TableName}"", requests, {isIdentity});";
                        }
                        if (IsIdentity) { cmd += "return u;"; } else { cmd += "return u.ResultCount;"; }
                    }
                    else
                    {
                        //简单值类型，插入单个
                        cmd = $@"System.Collections.Generic.List<EasyDefine.Configuration.InsertRequest> requests = new System.Collections.Generic.List<EasyDefine.Configuration.InsertRequest>();
                              EasyDefine.Configuration.InsertRequest request = new EasyDefine.Configuration.InsertRequest();
                              requests.Add(request);";
                        foreach (var p in this.ParamInfos)
                        {
                            cmd += $@"  request.Fields.Add(new EasyDefine.Configuration.InsertFields {{ Name = ""{p.Name}"",Value = {p.Name} }});";
                        }
                        string isIdentity = IsIdentity ? "true" : "false";
                        cmd += $@"DatabaseContext<dynamic> _dbc = new DatabaseContext<dynamic>(SourcePointEnum.{this.SourcePointEnum},{this.SlaveId});";
                        if (IsTrans)
                        {
                            cmd += $@"var u = await _dbc.InsertEntitiesAsync(""{TableName}"", requests,{TransVariName},{TransVariName}.Connection,{isIdentity});";
                        }
                        else
                        {
                            cmd += $@"var u = await _dbc.InsertEntitiesAsync(""{TableName}"", requests, {isIdentity});";
                        }
                        if (IsIdentity) { cmd += "return u;"; } else { cmd += "return u.ResultCount;"; }
                    }
                }
                else
                {
                    //同步的
                    //判断第一个传入的是否为集合类型，从而决定是否为批量操作
                    var GenericType = this.ParamInfos[0].ParameterType; //获取泛型类型
                    if (GenericType.GUID == typeof(IEnumerable<>).GUID || GenericType.GUID == typeof(List<>).GUID)
                    {
                        var RealGenericType = GenericType.GetGenericArguments()[0]; //获取真实的返回对象
                        var addstr = "EasyDefine.Configuration.InsertRequest request = new EasyDefine.Configuration.InsertRequest();";
                        foreach (var f in RealGenericType.GetProperties()) {
                            addstr += $@"
                              var field_{f.Name} = new EasyDefine.Configuration.InsertFields {{ Name = ""{f.Name}"",Value = entity.{f.Name} }};
                              request.Fields.Add(field_{f.Name});";
                        }
                        //简单值类型，插入单个
                        cmd = $@"System.Collections.Generic.List<EasyDefine.Configuration.InsertRequest> requests = new System.Collections.Generic.List<EasyDefine.Configuration.InsertRequest>();";
                        //设置循环遍历参数
                        string param_name = this.ParamInfos[0].Name;
                        cmd += $@"foreach(var entity in {param_name}){{ 
                              {addstr}
                              requests.Add(request);
                        }}";
                        string isIdentity = IsIdentity ? "true" : "false";
                        cmd += $@"DatabaseContext<dynamic> _dbc = new DatabaseContext<dynamic>(SourcePointEnum.{this.SourcePointEnum},{this.SlaveId});";
                        if (IsTrans)
                        {
                            cmd += $@"var u = _dbc.InsertEntities(""{TableName}"", requests,{TransVariName},{TransVariName}.Connection,{isIdentity});";
                        }
                        else
                        {
                            cmd += $@"var u = _dbc.InsertEntities(""{TableName}"", requests, {isIdentity});";
                        }
                        if (IsIdentity) { cmd += "return u;"; } else { cmd += "return u.ResultCount;"; }
                    }
                    else
                    {
                        //简单值类型，插入单个
                        cmd = $@"System.Collections.Generic.List<EasyDefine.Configuration.InsertRequest> requests = new System.Collections.Generic.List<EasyDefine.Configuration.InsertRequest>();
                              EasyDefine.Configuration.InsertRequest request = new EasyDefine.Configuration.InsertRequest();
                              requests.Add(request);";
                        foreach (var p in this.ParamInfos)
                        {
                            cmd += $@"  request.Fields.Add(new EasyDefine.Configuration.InsertFields {{ Name = ""{p.Name}"",Value = {p.Name} }});";
                        }
                        string isIdentity = IsIdentity ? "true" : "false";
                        if (IsTrans)
                        {
                            cmd += $@"var u = _dbc.InsertEntities(""{TableName}"", requests,{TransVariName},{TransVariName}.Connection,{isIdentity});";
                        }
                        else
                        {
                            cmd += $@"var u = _dbc.InsertEntities(""{TableName}"", requests, {isIdentity});";
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
