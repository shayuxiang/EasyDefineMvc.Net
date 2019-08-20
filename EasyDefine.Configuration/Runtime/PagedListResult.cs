using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Configuration
{
    /// <summary>
    /// 分页数据结果集
    /// </summary>
    public class PagedListResult<T>
    {
        public PagedListResult(int pageIndex, int pageSize, int total, IEnumerable<T> items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
            Items = items;
            PageCount = PageSize > 0 ? ((Total + PageSize - 1) / PageSize) : 0;
        }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数据量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; private set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<T> Items { get; set; }
    }
}
