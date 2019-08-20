using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyDefine.ServiceFramework.Models
{
    public class EntityModel
    {
        /// <summary>
        /// 对象描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 库名
        /// </summary>
        public string DllName { get; set; }

        /// <summary>
        /// 属性集合
        /// </summary>
        public List<FieldModel> Parameters { get; set; } = new List<FieldModel>();
    }
}
