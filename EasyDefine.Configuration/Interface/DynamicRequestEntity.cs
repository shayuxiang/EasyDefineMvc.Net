using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Configuration.Interface
{
    /// <summary>
    /// 请求DTO抽象接口
    /// </summary>
    public class DynamicRequestEntity
    {
        /// <summary>
        /// 添加参数键-值
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void Add(string Key, object Value) {

        }

        /// <summary>
        /// 批量添加参数
        /// </summary>
        /// <param name="keyValues"></param>
        public void AddRange(List<KeyValuePair<string, object>> keyValues) {

        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, object>> Get() {
            return null;
        }
    }
}
