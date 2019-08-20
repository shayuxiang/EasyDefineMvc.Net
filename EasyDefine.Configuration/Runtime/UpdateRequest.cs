using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Configuration
{
    public class UpdateRequest
    {
        public List<UpdateFields> Fields { get; set; } = new List<UpdateFields>();

        /// <summary>
        /// 修改条件
        /// </summary>
        public string WhereExp { get; set; }
    }


    /// <summary>
    /// 插入的对象
    /// </summary>
    public class UpdateFields
    {

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
