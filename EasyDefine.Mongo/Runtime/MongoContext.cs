using EasyDefine.Configuration.Lib;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Mongo.Runtime
{
    public class MongoContext<T>
    {
        /// <summary>
        /// 数据源
        /// </summary>
        private MongoPointEnum Source { get; set; } = MongoPointEnum.Master;

        /// <summary>
        /// 从库Id
        /// </summary>
        private int SlaveId { get; set; } = 1;

        /// <summary>
        /// Mongo链接
        /// </summary>
        private IMongoCollection<T> collection { get; set; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="source"></param>
        /// <param name="SlaveId"></param>
        public MongoContext(MongoPointEnum source, int SlaveId, string dbName, string objName)
        {
            this.Source = source;
            this.SlaveId = SlaveId;
            var db = MongoBuilder.Instance.GetConnection(Source, SlaveId).GetDatabase(dbName);
            collection = db.GetCollection<T>(objName);
        }

        public List<T> Query(FilterDefinition<T> filter = null)
        {
            //var filter = Builders<BsonDocument>.Filter;
            try
            {
                var document = collection.Find<T>(filter).ToList();
                if (filter == null)
                {
                    document = collection.Find<T>(e => true).ToList();
                }
                Log.Write($@"ED-Mongo-Query:{document.ToJson()}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($@":ED-Mongo-Query--");
                var json = document.ToJson();
                Console.WriteLine(json);
                Console.ForegroundColor = ConsoleColor.White;
                return document;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($@":ED-Mongo-Query-Error--{DateTime.Now},{ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                return new List<T>();
            }
        }


        public void Add(T Entity) {
            try
            {
                collection.InsertOne(Entity);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($@":ED-Mongo-Add-Error--{DateTime.Now},{ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
        }

    }
}
