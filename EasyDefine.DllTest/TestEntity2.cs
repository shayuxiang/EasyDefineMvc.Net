using EasyDefine.Mongo.Runtime;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.DllTest
{
    public class TestEntity2: MongoBaseEntity
    {
        public long TestID { get; set; }

        public string Url { get; set; }
    }
}
