using EasyDefine.Mongo.Runtime;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EasyDefine.Mongo.Attributes
{
    public class MongoQuery : Attribute
    {
        public string QueryCommand { get; set; }

        public MongoQuery() {
        }
    }
}
