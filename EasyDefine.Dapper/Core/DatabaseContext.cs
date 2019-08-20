using Dapper;
using EasyDefine.Configuration;
using EasyDefine.Configuration.Lib;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace EasyDefine.Dapper.Core
{
    public class DatabaseContext<T>
    {
        /// <summary>
        /// 数据源
        /// </summary>
        private SourcePointEnum Source { get; set; } = SourcePointEnum.Master;

        /// <summary>
        /// 从库Id
        /// </summary>
        private int SlaveId { get; set; } = 1;

        public DatabaseContext(SourcePointEnum source,int SlaveId) {
            this.Source = source;
            this.SlaveId = SlaveId;
        }

        /// <summary>
        /// 同步查询单个泛型对象 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public T QueryFirst(string strSql, DynamicParameters paras, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            Log.Write("QueryFirst:" + strSql);
            if (transaction != null)
            {
                return connection.QueryFirst<T>(strSql, paras, transaction);
            }
            else
            {
                using (var conn = DapperBuilder.Instance.GetConnection(Source, SlaveId))
                {
                    try
                    {
                        var ret = conn.QueryFirst<T>(strSql, paras, transaction);
                        if (transaction == null)
                        {
                            //没有事务，需要单独关闭 
                            conn.Close();
                            conn.Dispose();
                        }
                        return ret;
                    }
                    catch (Exception ex)
                    {
                        return default(T);
                    }
                }
            }
        }

        /// <summary>
        /// 异步查询单个泛型对象
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public async Task<T> QueryFirstAsync(string strSql, DynamicParameters paras, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            Log.Write("QueryFirst:" + strSql);
            if (transaction != null)
            {
                return await connection.QueryFirstAsync<T>(strSql, paras, transaction);
            }
            else
            {
                using (var conn = DapperBuilder.Instance.GetConnection(Source, SlaveId))
                {
                    try
                    {
                        var ret = await conn.QueryFirstAsync<T>(strSql, paras);
                        if (transaction == null)
                        {
                            //没有事务，需要单独关闭 
                            conn.Close();
                            conn.Dispose();
                        }
                        return ret;
                    }
                    catch (Exception ex)
                    {
                        return default(T);
                    }
                }
            }
        }

        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public IEnumerable<T> Query(string sql, DynamicParameters paras, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            try
            {
                if (transaction != null)
                {
                    return connection.Query<T>(sql, paras, transaction);
                }
                else
                {
                    using (var myConn = DapperBuilder.Instance.GetConnection(Source, SlaveId))
                    {
                        var ret = myConn.Query<T>(sql, paras);
                        if (transaction == null)
                        {
                            //没有事务，需要单独关闭 
                            myConn.Close();
                            myConn.Dispose();
                        }
                        return ret;
                    }
                }
            }
            catch (Exception ex) {

                Log.Write("QueryFirst:" + sql, ex);
                return null;
            }
        }

        /// <summary>
        /// 异步查询泛型集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync(string sql, DynamicParameters paras, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            try
            {
                if (transaction != null)
                {
                    return await connection.QueryAsync<T>(sql, paras, transaction);
                }
                else
                {
                    using (var myConn = DapperBuilder.Instance.GetConnection(Source, SlaveId))
                    {
                        var ret = await myConn.QueryAsync<T>(sql, paras);
                        if (transaction == null)
                        {
                            //没有事务，需要单独关闭 
                            myConn.Close();
                            myConn.Dispose();
                        }
                        return ret;
                    }
                }
            }
            catch (Exception ex)
            {

                Log.Write("QueryFirst:" + sql, ex);
                return null;
            }
        }

        /// <summary>
        /// 同步查询分页数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public PagedListResult<T> QueryPaged(string sql, int pageIndex, int pageSize, DynamicParameters paras, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            Log.Write("QueryFirst:" + sql);
            try
            {
                var strSelCountSql = $@"select count(0) From ({sql}) t"; //查询总数
                var strSelPagedSql = $@"select * From ({sql}) t limit @SkipRows,@PageSize"; //分页查询
                var list = default(IEnumerable<T>);
                var total = 0;
                if (paras != null)
                {
                    //添加分页参数
                    paras.Add("@SkipRows", (pageIndex - 1) * pageSize);
                    paras.Add("@PageSize", pageSize);
                }
                //查询
                if (transaction != null)
                {
                    list = connection.Query<T>(strSelPagedSql, paras);
                    total = connection.QueryFirst<int>(strSelCountSql, paras);
                    return new PagedListResult<T>(pageIndex, pageSize, total, list);
                }
                else
                {
                    using (var myConn = DapperBuilder.Instance.GetConnection(Source, SlaveId))
                    {
                        list = myConn.Query<T>(strSelPagedSql, paras);
                        total = myConn.QueryFirst<int>(strSelCountSql, paras);
                        var ret = new PagedListResult<T>(pageIndex, pageSize, total, list);
                        if (transaction == null)
                        {
                            //没有事务，需要单独关闭 
                            myConn.Close();
                            myConn.Dispose();
                        }
                        return ret;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Log.Write("QueryPaged:" + sql, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                Log.Write("QueryPaged:" + sql, ex);
                throw ex;
            }
        }


        /// <summary>
        /// 异步查询分页数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public async Task<PagedListResult<T>> QueryPagedAsync(string sql, int pageIndex, int pageSize, DynamicParameters paras, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            Log.Write("QueryFirst:" + sql);
            try
            {
                var strSelCountSql = $@"select count(0) From ({sql}) t"; //查询总数
                var strSelPagedSql = $@"select * From ({sql}) t limit @SkipRows,@PageSize"; //分页查询
                var list = default(IEnumerable<T>);
                var total = 0;
                if (paras != null)
                {
                    //添加分页参数
                    paras.Add("@SkipRows", (pageIndex - 1) * pageSize);
                    paras.Add("@PageSize", pageSize);
                }
                //查询
                if (transaction != null)
                {
                    list = await connection.QueryAsync<T>(strSelPagedSql, paras);
                    total = await connection.QueryFirstAsync<int>(strSelCountSql, paras);
                    return new PagedListResult<T>(pageIndex, pageSize, total, list);
                }
                else
                {
                    using (var myConn = DapperBuilder.Instance.GetConnection(Source, SlaveId))
                    {
                        list = await myConn.QueryAsync<T>(strSelPagedSql, paras);
                        total = await myConn.QueryFirstAsync<int>(strSelCountSql, paras);
                        var ret = new PagedListResult<T>(pageIndex, pageSize, total, list);
                        if (transaction == null)
                        {
                            //没有事务，需要单独关闭 
                            myConn.Close();
                            myConn.Dispose();
                        }
                        return ret;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Log.Write("QueryPagedAsync:" + sql, ex);
                throw ex;
            }

            catch (Exception ex)
            {
                Log.Write("QueryPagedAsync:" + sql, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 同步添加
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public InsertResult InsertEntities(string TableName, List<InsertRequest> insertRequests, bool IsIdentity = false) {
            try {
                var conn = DapperBuilder.Instance.GetConnection(Source, SlaveId);
                return InsertEntities(TableName, insertRequests, null, conn, IsIdentity);
            }
            catch (MySqlException ex)
            {
                Log.Write("InsertEntities:" + TableName, ex);
                throw ex;
            }

        }

        /// <summary>
        /// 同步添加
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public InsertResult InsertEntities(string TableName, List<InsertRequest> insertRequests, IDbTransaction transaction = null,IDbConnection myConn =null,bool IsIdentity = false)
        {
            try
            {
                int count = 0;
                var cmd = "";
                List<object> Identities = new List<object>();
                foreach (var m in insertRequests)
                {
                    var AtSql = "";
                    var FieldSql = "";
                    DynamicParameters p = new DynamicParameters();
                    //字段
                    foreach (var f in m.Fields)
                    {
                        AtSql += $@"@{f.Name},";
                        FieldSql += $@"{f.Name},";
                        p.Add($@"@{f.Name}", f.Value);
                    }
                    AtSql = AtSql.Substring(0, AtSql.Length - 1);
                    FieldSql = FieldSql.Substring(0, FieldSql.Length - 1);
                    var singlecmd = $@"INSERT INTO {TableName} ({FieldSql}) VALUES({AtSql});";
                    cmd += singlecmd;
                    int _result = 0;
                    if (transaction != null)
                    {
                        _result = myConn.Execute(singlecmd, p, transaction);
                    }
                    else
                    {
                        _result = myConn.Execute(singlecmd, p);
                    }
                    if (_result > 0)
                    {
                        count += _result;
                        if (IsIdentity)
                        {
                            var Ident = myConn.Query<Identity>("SELECT LAST_INSERT_ID() as IdentityKey").AsList<Identity>();
                            Identities.Add(Ident[0].IdentityKey);
                        }
                    }
                }
                Console.WriteLine(cmd);
                InsertResult result = new InsertResult()
                {
                    ResultCount = count,
                    ResultKeys = Identities,
                    IsSuccess = true
                };
                if (myConn == null) return null;
                if (transaction == null)
                {
                    //没有事务，需要单独关闭 
                    myConn.Close();
                    myConn.Dispose();
                    myConn = null;
                }
                return result;
            }
            catch (MySqlException ex)
            {
                Log.Write("trans:" + TableName, ex);
                if (transaction != null)
                {
                    //回滚
                    transaction.Rollback();
                }
                return new InsertResult()
                {
                    ResultCount = 0,
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
            catch (Exception ex)
            {
                Log.Write("trans:" + TableName, ex);
                if (transaction != null)
                {
                    //回滚
                    transaction.Rollback();
                }
                return new InsertResult()
                {
                    ResultCount = 0,
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
        }


        /// <summary>
        /// 同步添加
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<InsertResult> InsertEntitiesAsync(string TableName, List<InsertRequest> insertRequests, bool IsIdentity = false)
        {
            try
            {
                using (var conn = DapperBuilder.Instance.GetConnection(Source, SlaveId))
                {
                    return await InsertEntitiesAsync(TableName, insertRequests, null, conn, IsIdentity);
                }
            }
            catch (Exception ex) {

                Log.Write("InsertEntitiesAsync:" + TableName, ex);
                return null;
            }
        }

        /// <summary>
        /// 同步添加
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<InsertResult> InsertEntitiesAsync(string TableName, List<InsertRequest> insertRequests, IDbTransaction transaction = null, IDbConnection myConn = null, bool IsIdentity = false)
        {
            try
            {
                int count = 0;
                var cmd = "";
                List<object> Identities = new List<object>();
                foreach (var m in insertRequests)
                {
                    var AtSql = "";
                    var FieldSql = "";
                    DynamicParameters p = new DynamicParameters();
                    //字段
                    foreach (var f in m.Fields)
                    {
                        AtSql += $@"@{f.Name},";
                        FieldSql += $@"{f.Name},";
                        p.Add($@"@{f.Name}", f.Value);
                    }
                    AtSql = AtSql.Substring(0, AtSql.Length - 1);
                    FieldSql = FieldSql.Substring(0, FieldSql.Length - 1);
                    var singlecmd = $@"INSERT INTO {TableName} ({FieldSql}) VALUES({AtSql});";
                    cmd += singlecmd;
                    int _result = 0;
                    if (transaction != null)
                    {
                        _result = await myConn.ExecuteAsync(singlecmd, p, transaction);
                    }
                    else
                    {
                        _result = await myConn.ExecuteAsync(singlecmd, p);
                    }
                    if (_result > 0)
                    {
                        count += _result;
                        if (IsIdentity)
                        {
                            var Ident = myConn.Query<Identity>("SELECT LAST_INSERT_ID() as IdentityKey").AsList<Identity>();
                            Identities.Add(Ident[0].IdentityKey);
                        }
                    }
                }
                Console.WriteLine(cmd);
                InsertResult result = new InsertResult()
                {
                    ResultCount = count,
                    ResultKeys = Identities,
                    IsSuccess = true
                };
                if (myConn == null) return null;
                if (transaction == null)
                {
                    //没有事务，需要单独关闭 
                    myConn.Close();
                    myConn.Dispose();
                    myConn = null;
                }
                return result;
            }
            catch (MySqlException ex)
            {
                Log.Write("InsertEntitiesAsync:" + TableName, ex);
                if (transaction != null)
                {
                    //回滚
                    transaction.Rollback();
                }
                return new InsertResult()
                {
                    ResultCount = 0,
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
            catch (Exception ex)
            {
                Log.Write("InsertEntitiesAsync:" + TableName, ex);
                if (transaction != null)
                {
                    //回滚
                    transaction.Rollback();
                }
                return new InsertResult()
                {
                    ResultCount = 0,
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
        }


        /// <summary>
        /// 同步更新
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public UpdateResult UpdateEntities(string TableName, List<UpdateRequest> updateRequest)
        {
            try
            {
                using (var conn = DapperBuilder.Instance.GetConnection(Source, SlaveId))
                {
                    return UpdateEntities(TableName, updateRequest, null, conn);
                }
            }
            catch (Exception ex) {

                Log.Write("InsertEntitiesAsync:" + TableName, ex);
                return null;
            }
        }

        /// <summary>
        /// 同步更新
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public UpdateResult UpdateEntities(string TableName, List<UpdateRequest> updateRequests, IDbTransaction transaction = null, IDbConnection myConn = null)
        {
            try
            {
                int count = 0;
                var cmd = "";
                foreach (var m in updateRequests)
                {
                    var FieldSql = "";
                    var whereExp =  string.IsNullOrEmpty(m.WhereExp.Trim())?"":$@" WHERE {m.WhereExp.Trim()}";
                    DynamicParameters p = new DynamicParameters();
                    //字段
                    foreach (var f in m.Fields)
                    {
                        FieldSql += $@"{f.Name}=@{f.Name},";
                        p.Add($@"@{f.Name}", f.Value);
                    }
                    FieldSql = FieldSql.Substring(0, FieldSql.Length - 1);
                    var singlecmd = $@"UPDATE {TableName} SET {FieldSql} {whereExp};";
                    cmd += singlecmd;
                    int _result = 0;
                    if (transaction != null)
                    {
                        _result = myConn.Execute(singlecmd, p, transaction);
                    }
                    else
                    {
                        _result = myConn.Execute(singlecmd, p);
                    }
                    if (_result > 0)
                    {
                        count += _result;
                    }
                }
                Console.WriteLine(cmd);
                UpdateResult result = new UpdateResult()
                {
                    ResultCount = count,
                    IsSuccess = true
                };
                if (myConn == null) return null;
                if (transaction == null)
                {
                    //没有事务，需要单独关闭 
                    myConn.Close();
                    myConn.Dispose();
                    myConn = null;
                }
                return result;
            }
            catch (MySqlException ex)
            {
                Log.Write("UpdateEntities:" + TableName, ex);
                if (transaction != null)
                {
                    //回滚
                    transaction.Rollback();
                }
                return new UpdateResult()
                {
                    ResultCount = 0,
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
            catch (Exception ex)
            {
                Log.Write("UpdateEntities:" + TableName, ex);
                if (transaction != null)
                {
                    //回滚
                    transaction.Rollback();
                }
                return new UpdateResult()
                {
                    ResultCount = 0,
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
        }

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateEntitiesAsync(string TableName, List<UpdateRequest> updateRequest)
        {
            using (var conn = DapperBuilder.Instance.GetConnection(Source, SlaveId))
            {
                return await UpdateEntitiesAsync(TableName, updateRequest, null, conn);
            }
        }

        /// <summary>
        /// 同步更新
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateEntitiesAsync(string TableName, List<UpdateRequest> updateRequests, IDbTransaction transaction = null, IDbConnection myConn = null)
        {
            try
            {
                int count = 0;
                var cmd = "";
                foreach (var m in updateRequests)
                {
                    var FieldSql = "";
                    var whereExp = string.IsNullOrEmpty(m.WhereExp.Trim()) ? "" : $@" WHERE {m.WhereExp.Trim()}";
                    DynamicParameters p = new DynamicParameters();
                    //字段
                    foreach (var f in m.Fields)
                    {
                        FieldSql += $@"{f.Name}=@{f.Name},";
                        p.Add($@"@{f.Name}", f.Value);
                    }
                    FieldSql = FieldSql.Substring(0, FieldSql.Length - 1);
                    var singlecmd = $@"UPDATE {TableName} SET {FieldSql} {whereExp};";
                    cmd += singlecmd;
                    int _result = 0;
                    if (transaction != null)
                    {
                        _result = await myConn.ExecuteAsync(singlecmd, p, transaction);
                    }
                    else
                    {
                        _result = await myConn.ExecuteAsync(singlecmd, p);
                    }
                    if (_result > 0)
                    {
                        count += _result;
                    }
                }
                Console.WriteLine(cmd);
                UpdateResult result = new UpdateResult()
                {
                    ResultCount = count,
                    IsSuccess = true
                };
                if (myConn == null) return null;
                if (transaction == null)
                {
                    //没有事务，需要单独关闭 
                    myConn.Close();
                    myConn.Dispose();
                    myConn = null;
                }
                return result;
            }
            catch (MySqlException ex)
            {
                Log.Write("UpdateEntities:" + TableName, ex);
                if (transaction != null)
                {
                    //回滚
                    transaction.Rollback();
                }
                return new UpdateResult()
                {
                    ResultCount = 0,
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
            catch (Exception ex)
            {
                Log.Write("UpdateEntities:" + TableName, ex);
                if (transaction != null)
                {
                    //回滚
                    transaction.Rollback();
                }
                return new UpdateResult()
                {
                    ResultCount = 0,
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
        }


        /// <summary>
        /// 同步删除
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public DeleteResult DeleteEntities(string TableName,string WhereExp)
        {
            var conn = DapperBuilder.Instance.GetConnection(Source, SlaveId);
            return DeleteEntities(TableName, WhereExp, null, conn);
        }

        /// <summary>
        /// 同步删除
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public DeleteResult DeleteEntities(string TableName, string WhereExp, IDbTransaction transaction = null, IDbConnection myConn = null)
        {
            try
            {
                int count = 0;
                WhereExp = string.IsNullOrEmpty(WhereExp.Trim()) ? "" : " WHERE " + WhereExp;
                var cmd = $@"DELETE FROM {TableName} {WhereExp}";
                if (transaction != null)
                {
                    count = myConn.Execute(cmd, transaction);
                }
                else
                {
                    count = myConn.Execute(cmd);
                }
                Console.WriteLine(cmd);
                DeleteResult result = new DeleteResult()
                {
                    ResultCount = count,
                    IsSuccess = true
                };
                if (myConn == null) return null;
                if (transaction == null)
                {
                    //没有事务，需要单独关闭 
                    myConn.Close();
                    myConn.Dispose();
                    myConn = null;
                }
                return result;
            }
            catch (MySqlException ex)
            {
                Log.Write("delteEntities:" + TableName, ex);
                if (transaction != null)
                {
                    //回滚
                    transaction.Rollback();
                }
                return new DeleteResult()
                {
                    ResultCount = 0,
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
            catch (Exception ex)
            {
                Log.Write("delteEntities:" + TableName, ex);
                if (transaction != null)
                {
                    //回滚
                    transaction.Rollback();
                }
                return new DeleteResult()
                {
                    ResultCount = 0,
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
        }

        /// <summary>
        /// 异步删除
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteEntitiesAsync(string TableName, string WhereExp)
        {
            var conn = DapperBuilder.Instance.GetConnection(Source, SlaveId);
            return await DeleteEntitiesAsync(TableName, WhereExp, null, conn);
        }

        /// <summary>
        /// 异步删除
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteEntitiesAsync(string TableName, string WhereExp, IDbTransaction transaction = null, IDbConnection myConn = null)
        {
            try
            {
                int count = 0;
                WhereExp = string.IsNullOrEmpty(WhereExp.Trim()) ? "" : " WHERE " + WhereExp;
                var cmd = $@"DELETE FROM {TableName} {WhereExp}";
                if (transaction != null)
                {
                    count = await myConn.ExecuteAsync(cmd, transaction);
                }
                else
                {
                    count = await myConn.ExecuteAsync(cmd);
                }
                Console.WriteLine(cmd);
                DeleteResult result = new DeleteResult()
                {
                    ResultCount = count,
                    IsSuccess = true
                };
                if (myConn == null) return null;
                if (transaction == null)
                {
                    //没有事务，需要单独关闭 
                    myConn.Close();
                    myConn.Dispose();
                    myConn = null;
                }
                return result;
            }
            catch (MySqlException ex)
            {
                Log.Write("delteEntities:" + TableName, ex);
                if (transaction != null)
                {
                    //回滚
                    transaction.Rollback();
                }
                return new DeleteResult()
                {
                    ResultCount = 0,
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
            catch (Exception ex)
            {
                Log.Write("delteEntities:" + TableName, ex);
                if (transaction != null)
                {
                    //回滚
                    transaction.Rollback();
                }
                return new DeleteResult()
                {
                    ResultCount = 0,
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
        }


    }
}
