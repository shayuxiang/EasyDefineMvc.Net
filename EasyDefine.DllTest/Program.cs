using EasyDefine.Mongo.Runtime;
using MongoDB.Driver;
using System;

namespace EasyDefine.DllTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //MongoContext<TestEntity> mongoContext = new MongoContext<TestEntity>(MongoPointEnum.Master, 0, "test", "ObjClass2");
            //var test2 = new TestEntity2 { Url = "http://www.easydefinemvc.cn", TestID = 66 } ;

            //MongoContext<TestEntity2> mongoContext1 = new MongoContext<TestEntity2>(MongoPointEnum.Master, 0, "test", "ObjClass");
            //mongoContext1.Add(test2);

            //mongoContext.Add(new TestEntity { Name = "沙宇祥", Age = 30, Entity = test2 });
            //mongoContext.Add(new TestEntity { Name = "李闯", Age = 25 });
            //mongoContext.Add(new TestEntity { Name = "胡鑫", Age = 22 });
           // var list = mongoContext.Query(Builders<TestEntity>.Filter.Gte(e=>e.Age,30) & Builders<TestEntity>.Filter.Where(e=>e.Name.StartsWith("李")||e.Name.StartsWith("沙")));

            string WhereExp = "(Name = 123 or (Name like @Name2) or Age > 25 or (Name like @Name)) or (Name <> 123 or Age <=10)";

            SqlWhere sqlWhere = new SqlWhere(WhereExp);
            sqlWhere.ToSQLUnit();
            sqlWhere.Sentence();
            sqlWhere.GetWhereTrees();
            sqlWhere.ToMongoBuilders<TestEntity>();
            Console.ReadLine();
        }
    }
}
