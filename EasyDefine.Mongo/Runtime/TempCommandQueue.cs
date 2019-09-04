using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Mongo.Runtime
{
    public class TempCommandQueue
    {
        /// <summary>
        /// 执行队列
        /// </summary>
        public Queue<SQLWhereCommandLine> TempLine { get; set; } = new Queue<SQLWhereCommandLine>();

        /// <summary>
        /// 组Id
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 是否有链接
        /// </summary>
        public bool HasLinked { get; set; } = false;

        /// <summary>
        /// 是否已被调用过
        /// </summary>
        public bool HasExcuted { get; set; } = false;
    }
}
