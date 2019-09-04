using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Mongo.Runtime
{
    /// <summary>
    /// EasyDefine定义的mongo-base-entity
    /// </summary>
    public class MongoBaseEntity
    {
        public ObjectId _id { get; set; }

        public DateTime EdCreateTime { get; set; } = DateTime.Now;
    }
}
