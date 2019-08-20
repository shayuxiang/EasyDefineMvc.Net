using EasyDefine.Configuration.Runtime;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace EasyDefine.Dapper.Core
{
    public class DapperBuilder
    {

        private static DapperBuilder _instance = null;

        /// <summary>
        /// 单例实现数据源存取
        /// </summary>
        public static DapperBuilder Instance
        {
            get
            {
                return _instance ?? (_instance = new DapperBuilder());
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        private DapperBuilder() {
        }

        /// <summary>
        /// 获得数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection(SourcePointEnum sourcePointEnum, int slaveId)
        {
            try
            {
                if (sourcePointEnum == SourcePointEnum.Master)
                {
                    //主库
                    return new MySqlConnection(ConfigHelper.MasterConnectionString);
                }
                else
                {
                    //从库
                    return new MySqlConnection(ConfigHelper.SlaveConnectionString[slaveId > 0 ? slaveId - 1 : 0]);
                }
            }
            catch
            {
                throw;
            }
        }


    }
}
