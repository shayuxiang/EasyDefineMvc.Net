using EasyDefine.Mongo.Runtime;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.DllTest
{
    public class TestEntity : MongoBaseEntity
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime CreateDate { get; set; }

        public TestEntity2 Entity { get; set; } = null;
    }
}
