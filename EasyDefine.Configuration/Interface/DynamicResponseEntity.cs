using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Configuration.Interface
{
    /// <summary>
    /// 响应DTO抽象接口
    /// </summary>
    public class DynamicResponseEntity
    {
        /// <summary>
        /// 获取实体对象JSON序列化
        /// </summary>
        /// <returns></returns>
        public string GetJson() {
            return string.Empty;
        }

        /// <summary>
        /// 获取实体对象JSON序列化-排除不需要的字段
        /// </summary>
        /// <param name="IgnoreFields"></param>
        /// <returns></returns>
        public string GetJson(List<string> IgnoreFields) {
            return string.Empty;
        }
    }
}
