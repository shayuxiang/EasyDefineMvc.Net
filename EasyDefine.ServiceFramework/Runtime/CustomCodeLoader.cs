using EasyDefine.Configuration.Runtime;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace EasyDefine.ServiceFramework.Runtime
{
    /// <summary>
    /// 实例自定义代码加载器
    /// </summary>
    internal class CustomCodeLoader
    {
        /// <summary>
        /// 执行语言
        /// </summary>
        private readonly LanguageType TheLanguageType;

        /// <summary>
        /// 方法的代码段
        /// </summary>
        private List<CustomCodeEntity> customCodeEntities = new List<CustomCodeEntity>();

        /// <summary>
        /// 配置代码路径
        /// </summary>
        private XmlDocument xmldoc = new XmlDocument();
        /// <summary>
        /// 加载用户代码构造
        /// </summary>
        /// <param name="languageType"></param>
        /// <param name="sourceRoute"></param>
        public CustomCodeLoader(LanguageType languageType,string sourceRoute, ConfigHelper configHelper)
        {
            //TheLanguageType = languageType;
            //xmldoc.LoadXml(configHelper.GetScriptFileText(sourceRoute)); //载入文件
            //XmlNodeList methods = xmldoc.SelectNodes("/services/method");
            //if (methods.Count > 0) {
            //    foreach (XmlNode e in methods) {

            //        var entity = new CustomCodeEntity
            //        {
            //            MethodName = e.Attributes["name"].Value,
            //            UserCode = new CodeSnippetStatement(e.InnerText)
            //        };
            //        customCodeEntities.Add(entity);
            //    }
            //}
        }

        /// <summary>
        /// 获取用户代码
        /// </summary>
        /// <returns></returns>
        public CustomCodeEntity GetMethodCode(string methodName) {
            return customCodeEntities.Find(e => e.MethodName == methodName);
        }
    }
}
