using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyDefine.ServiceFramework.Models
{
    /// <summary>
    /// 标记所有API元素
    /// </summary>
    public class ApiEnumerable
    {
        /// <summary>
        /// 所有方法
        /// </summary>
        public List<ApiModel> ApiModels { get; set; } = new List<ApiModel>();

        /// <summary>
        /// 所有需要重新编译的Entity
        /// </summary>
        public List<EntityModel> EntityModels { get; set; } = new List<EntityModel>();

        /// <summary>
        /// 所有需要重新编译的枚举
        /// </summary>
        public List<EntityModel> EnumModels { get; set; } = new List<EntityModel>();

        /// <summary>
        /// 返回Vue对象
        /// </summary>
        /// <returns></returns>
        public string ToVue(string RouteName,string Host,bool IsAuth) {
            //定义枚举
            var func = $@"/** @description 全局对象:服务器地址 */var Host = '{Host}';";
            foreach (var model in EnumModels)
            {
                var enumjs = $@"/** @description 枚举[{model.Description}]  */ var Enum_{RouteName}_{model.ClassName} = new Object();";
                var func_body = "";
                foreach (var param in model.Parameters)
                {
                    var defaultValue = Convert.ToInt32(param.DefaultValue);
                    func_body += $@"/** @description {param.Description}    @example var x {{=}} Enum_{RouteName}_{model.ClassName}.{param.Name};   */ Enum_{RouteName}_{model.ClassName}.{param.Name} = {defaultValue}; ";
                }
                enumjs = enumjs + func_body;
                //所有的初始化
                func += enumjs;
            }
            //定义对象
            foreach (var model in EntityModels)
            {
                //原型方式创建js对象
                var entityjs = $@"/** @description 实体对象[{model.Description}]  */ function Model_{RouteName}_{model.ClassName}(){{}}";
                var func_body = "";
                foreach (var param in model.Parameters) {
                    var defaultValue = "null";
                    //自定义类型
                    if (!param.FieldType.IsPrimitive && !param.FieldType.Assembly.FullName.Contains("System") && !param.FieldType.FullName.Contains("Microsoft"))
                    {
                        defaultValue = $@"new Model_{RouteName}_{param.FieldType.Name}()";
                    }
                    //系统类型
                    else if (param.FieldType.GUID == typeof(int).GUID || param.FieldType.GUID == typeof(long).GUID || param.FieldType.GUID == typeof(float).GUID || param.FieldType.GUID == typeof(decimal).GUID || param.FieldType.GUID == typeof(uint).GUID)
                    {
                        defaultValue = "0";
                    }
                    else if (param.FieldType.GUID == typeof(IEnumerable<>).GUID || param.FieldType.GUID == typeof(List<>).GUID)
                    {
                        defaultValue = "[]";
                    }
                    else if (param.FieldType.GUID == typeof(string).GUID)
                    {
                        defaultValue = "''";
                    }
                    else if (param.FieldType.GUID == typeof(DateTime).GUID|| param.FieldType.GUID == typeof(DateTime?).GUID)
                    {
                        defaultValue = "new Date()";
                    }
                    func_body += $@"/** @description {param.Description}    @example var x {{=}} Model_{RouteName}_{model.ClassName}.{param.Name}; */ Model_{RouteName}_{model.ClassName}.prototype.{param.Name} = {defaultValue};";
                }
                entityjs += entityjs + func_body;
                func += entityjs;
            }
            //定义接口对象
            //Linq分组处理          
            IEnumerable<IGrouping<string, ApiModel>> query = ApiModels.GroupBy(api => api.ControllerName);
            foreach (var coll in query) {
                List<ApiModel> api = coll.ToList<ApiModel>();
                //原型方式创建js-api对象
                var apijs = $@"function API_{RouteName}_{api.First().ControllerName}(){{}};";
                var func_body = "";
                foreach (var method in api) {
                    var _params = "";
                    var func_content = "";
                    var data = "";
                    var url = $@"Host+'{RouteName}/{method.ControllerName}/{method.ActionName}'";
                    //参数整理
                    foreach (var p in method.ParamTypes) {
                        _params += $@"{p.Name},";
                        data += $@"{p.Name}:{p.Name},";
                    }
                    if (data.Length > 0)
                    {
                        data = data.Substring(0, data.Length - 1);
                        data = $@"{{{data}}}";
                    }
                    else {
                        data = "{}";
                    }
                    var header_async = IsAuth ? $@",headers:{{'Authorization':token,'AppKey':AppKey}}" : "";
                    var header = IsAuth ? $@",headers:{{'Authorization':authCall(),'AppKey':AppKey}}" : "";
                    //请求方式
                    if (method.RequestMethod == "get")
                    {
                        var ret = $@"return axios({{method:'get',url:{url}{header},params:{data}}});";
                        var async_ret = $@"return authCall().then(function(token){{
                                    return axios({{method:'get',url:{url}{header_async},params:{data}}});
                        }})";
                        if (IsAuth)
                        {
                            func_content += $@"if(!isAsync){{ {ret} }} else {{{async_ret} }}";
                        }
                        else
                        {
                            func_content += ret;
                        }
                    }
                    else if (method.RequestMethod == "post")
                    {
                        var ret = $@"return axios({{method:'post',url:{url}{header},params:{data}}});";
                        var async_ret = $@"return authCall().then(function(token){{
                                    return axios({{method:'post',url:{url}{header_async},params:{data}}});
                        }})";
                        if (IsAuth)
                        {
                            func_content += $@"if(!isAsync){{ {ret} }} else {{{async_ret} }}";
                        }
                        else
                        {
                            func_content += ret;
                        }
                    }
                    else if (method.RequestMethod == "put")
                    {
                        var ret = $@"return axios({{method:'put',url:{url}{header},params:{data}}});";
                        var async_ret = $@"return authCall().then(function(token){{
                                    return axios({{method:'put',url:{url}{header_async},params:{data}}});
                        }})";
                        if (IsAuth)
                        {
                            func_content += $@"if(!isAsync){{ {ret} }} else {{{async_ret} }}";
                        }
                        else {
                            func_content += ret;
                        }
                    }
                    else if (method.RequestMethod == "delete")
                    {
                        var ret = $@"return axios({{method:'delete',url:{url}{header},params:{data}}});";
                        var async_ret = $@"return authCall().then(function(token){{
                                    return axios({{method:'delete',url:{url}{header_async},params:{data}}});
                        }})";
                        if (IsAuth)
                        {
                            func_content += $@"if(!isAsync){{ {ret} }} else {{{async_ret} }}";
                        }
                        else
                        {
                            func_content += ret;
                        }
                    }
                    if (_params.Length > 0)
                    {
                        _params = _params.Substring(0, _params.Length - 1);
                    }
                    func_body += $@"API_{RouteName}_{api.First().ControllerName}.prototype.{method.ActionName} = function({_params}){{{func_content}}};";
                }
                //添加到代码返回
                apijs = apijs + func_body;
                func += apijs;
            }
            if (IsAuth) {
                func += $@"var authCall = null;";
                func += $@"var appKey = null;";
                func += $@"var isAsync = false;";
            }
            return func;
        }

        public string ToDoc(string RouteName, string Host) {
            //定义枚举
            var func = $@"";
            foreach (var model in EnumModels)
            {
                var enumjs = $@"<span class='EnumName'>Enum_{RouteName}_{model.ClassName}(枚举)</span><span class='EnumDetail'>.Net Core描述:[{model.Description}]</span>";
                var func_body = "";
                foreach (var param in model.Parameters)
                {
                    var defaultValue = Convert.ToInt32(param.DefaultValue);
                    func_body += $@"<ul class='EnumParam'><li>参数名称: {param.Name} </li> <li> {param.Description}</li> <li>示例： var x = Enum_{RouteName}_{model.ClassName}.{param.Name};</li> <li>编译值:{defaultValue}</li></ul> ";
                }
                enumjs = enumjs + func_body;
                //所有的初始化
                func += $@"<li>{enumjs}</li>";
            }
            func = $@"<p class='title_p'>服务域:[{RouteName}]</p><ul class='enum_ul'>{func}</ul>";
            //定义对象
            foreach (var model in EntityModels)
            {
                //原型方式创建js对象
                var entityjs = $@"<span  class='ModelName'>Model_{RouteName}_{model.ClassName}(实体对象)</span><span  class='ModelDetail'>.Net Core描述:[{model.Description}]</span>";
                var func_body = "";
                foreach (var param in model.Parameters)
                {
                    var defaultValue = "null";
                    //自定义类型
                    if (!param.FieldType.IsPrimitive && !param.FieldType.Assembly.FullName.Contains("System") && !param.FieldType.FullName.Contains("Microsoft"))
                    {
                        defaultValue = $@"自定义类型:{param.FieldType.Name}";
                    }
                    //系统类型
                    else if (param.FieldType.GUID == typeof(int).GUID || param.FieldType.GUID == typeof(long).GUID || param.FieldType.GUID == typeof(float).GUID || param.FieldType.GUID == typeof(decimal).GUID || param.FieldType.GUID == typeof(uint).GUID)
                    {
                        defaultValue = "数字类型";
                    }
                    else if (param.FieldType.GUID == typeof(IEnumerable<>).GUID || param.FieldType.GUID == typeof(List<>).GUID)
                    {
                        defaultValue = "数组";
                    }
                    else if (param.FieldType.GUID == typeof(string).GUID)
                    {
                        defaultValue = "字符串";
                    }
                    else if (param.FieldType.GUID == typeof(DateTime).GUID || param.FieldType.GUID == typeof(DateTime?).GUID)
                    {
                        defaultValue = "时间日期";
                    }
                    func_body += $@"<ul class='ModelParam'><li>参数名称: {param.Name} </li> <li> {param.Description}</li> <li>示例： var x = new Model_{RouteName}_{model.ClassName}; console.log(x.{param.Name});</li> <li>传入类型:{defaultValue}</li></ul> ";
                }
                entityjs = entityjs + func_body;
                func += $@"<ul  class='entity_ul'>{entityjs}</ul>";
            }
            //定义接口对象
            //Linq分组处理          
            IEnumerable<IGrouping<string, ApiModel>> query = ApiModels.GroupBy(api => api.ControllerName);
            foreach (var coll in query)
            {

                List<ApiModel> api = coll.ToList<ApiModel>();
                //原型方式创建js-api对象
                var apijs = $@"<span class='apiName'>API_{RouteName}_{api.First().ControllerName}(API对象)</span>";
                //apijs += $@"<span class='apiDetail'>.Net Core描述:[{api.First().Description}]</span>";
                var func_body = "";
                foreach (var method in api)
                {
                    var _params = "";
                    var data = "";
                    var url = $@"{{Host}}/{RouteName}/{method.ControllerName}/{method.ActionName}";
                    var link = $@"{Host}/{RouteName}/{method.ControllerName}/{method.ActionName}";
                    //参数整理
                    foreach (var p in method.ParamTypes)
                    {
                        var typeStr = "";
                        //自定义类型
                        if (!p.ParamType.IsPrimitive && !p.ParamType.Assembly.FullName.Contains("System") && !p.ParamType.FullName.Contains("Microsoft"))
                        {
                            typeStr = $@"自定义类型:{p.ParamType.Name}";
                        }
                        //系统类型
                        else if (p.ParamType.GUID == typeof(int).GUID || p.ParamType.GUID == typeof(long).GUID || p.ParamType.GUID == typeof(float).GUID || p.ParamType.GUID == typeof(decimal).GUID || p.ParamType.GUID == typeof(uint).GUID)
                        {
                            typeStr = "数字类型";
                        }
                        else if (p.ParamType.GUID == typeof(IEnumerable<>).GUID || p.ParamType.GUID == typeof(List<>).GUID)
                        {
                            typeStr = "数组";
                        }
                        else if (p.ParamType.GUID == typeof(string).GUID)
                        {
                            typeStr = "字符串";
                        }
                        else if (p.ParamType.GUID == typeof(DateTime).GUID || p.ParamType.GUID == typeof(DateTime?).GUID)
                        {
                            typeStr = "时间日期";
                        }
                        _params += $@"<li class='apiParamUnit'>参数{p.Name}</li><li class='apiParamUnit'>传入类型:{typeStr}</li>";
                    }
                    func_body += $@"<ul class='apiParam'><li class='apiUnit'>方法名称:{method.ActionName}</li><li class='apiUnit'><span>请求方式:{method.RequestMethod}</span></li><li class='apiUnit'>请求地址:<a target='_blank' href='{link}'>{url}</a><li class='apiDesc'>.Net Core描述:[{method.Description}]<li/></li>{_params}</ul>";
                }
                //添加到代码返回
                apijs = apijs + func_body;
                func += $@"<ul class='func_ul'>{apijs}</ul>";
            }

            return func;
        }
    }
}
