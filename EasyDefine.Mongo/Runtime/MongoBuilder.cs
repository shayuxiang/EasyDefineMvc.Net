using EasyDefine.Configuration.Runtime;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Mongo.Runtime
{
    public class MongoBuilder
    {
        private static MongoBuilder _instance = null;

        /// <summary>
        /// 单例实现数据源存取
        /// </summary>
        public static MongoBuilder Instance
        {
            get
            {
                return _instance ?? (_instance = new MongoBuilder());
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        private MongoBuilder()
        {
        }

        /// <summary>
        /// 获得数据库连接
        /// </summary>
        /// <returns></returns>
        public MongoClient GetConnection(MongoPointEnum sourcePointEnum, int slaveId)
        {
            try
            {
                if (sourcePointEnum == MongoPointEnum.Master)
                {
                    //主库
                    return new MongoClient(ConfigHelper.MongoMasterConnectionString);
                }
                else
                {
                    //从库
                    return new MongoClient(ConfigHelper.MongoSlaveConnectionString[slaveId > 0 ? slaveId - 1 : 0]);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
