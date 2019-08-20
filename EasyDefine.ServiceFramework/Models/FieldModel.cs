using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.ServiceFramework.Models
{
    public class FieldModel
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 属性描述描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public Type FieldType { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }
    }
}
