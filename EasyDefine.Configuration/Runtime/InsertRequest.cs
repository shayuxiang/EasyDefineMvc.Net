using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Configuration
{
    public class InsertRequest
    {
        public List<InsertFields> Fields { get; set; } = new List<InsertFields>();
    }

    /// <summary>
    /// 插入的对象
    /// </summary>
    public class InsertFields {

        /// <summary>
        /// 插入的字段
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 插入的值
        /// </summary>
        public object Value { get; set; }
    }
}
