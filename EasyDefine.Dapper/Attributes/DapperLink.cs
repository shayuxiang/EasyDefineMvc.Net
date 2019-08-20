using EasyDefine.Dapper.Core;
using EasyDefine.Dapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Dapper.Attributes
{
    public class DapperLink : Attribute
    {
        //标识取得的链接是主库还是从库
        public SourcePointEnum PointEnum { get; set; } = SourcePointEnum.Master;

        //标识从库的编号
        public int SlaveId { get; set; } = 1;
        //命名空间
        public string NameSpace { get; set; }
        internal string Name { get; set; }

        /// <summary>
        /// 获取当前接口的所有方法
        /// </summary>
        /// <returns></returns>
        public List<DapperCommand> GetDapperCommand(Type _interface)
        {
            List<DapperCommand> methods = new List<DapperCommand>();
            foreach (var m in _interface.GetMethods())
            {
                var a = m.GetCustomAttributes(typeof(DapperCommand), true);
                if (a.Length > 0)
                {
                    var _method = a[0] as DapperCommand;
                    _method.Name = m.Name;
                    _method.ReturnType = m.ReturnType;
                    _method.ParamInfos = m.GetParameters();
                    methods.Add(_method);
                }
            }
            return methods;
        }
    }
}
