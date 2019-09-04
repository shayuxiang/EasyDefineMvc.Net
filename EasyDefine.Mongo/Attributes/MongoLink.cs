using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Mongo.Attributes
{
    public class MongoLink : Attribute
    {
        /// <summary>
        /// Mongo数据库名
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        ///  Mongo对象名
        /// </summary>
        public string ObjName { get; set; }

    }
}
