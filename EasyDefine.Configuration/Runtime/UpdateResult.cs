using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Configuration
{
    public class UpdateResult
    {
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsSuccess { get; set; } = false;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 插入的条数
        /// </summary>
        public int ResultCount { get; set; } = 0;
    }
}
