using EasyDefine.Mongo.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.DllTest
{
    [MongoLink(DbName ="test")]
    public interface MongoInterface
    {
        [MongoQuery(QueryCommand = "Name like @Name")]
        List<TestEntity> GetTest(string Name);
    }
}
