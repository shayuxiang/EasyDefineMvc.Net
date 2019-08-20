using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Configuration
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false)]
    public class ApiExcelField:Attribute
    {
        /// <summary>
        /// 数据列显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 数据列是否加粗
        /// </summary>
        public bool IsBlod { get; set; }


        public ApiExcelField(string DisplayName, bool IsBlod = false)
        {
            this.DisplayName = DisplayName;
            this.IsBlod = IsBlod;
        }
    }
}
