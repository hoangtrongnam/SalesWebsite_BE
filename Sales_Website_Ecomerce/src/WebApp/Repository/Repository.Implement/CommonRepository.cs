using Dapper;
using Models.RequestModel;
using Repository.Interface;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Reflection.PortableExecutable;

namespace Repository.Implement
{
    public class CommonRepository : Repository, ICommonRepository
    {
        public CommonRepository(SqlConnection context, SqlTransaction _transaction)
        {
            this._context = context;
            this._transaction = _transaction;
        }

        /// <summary>
        /// Get Config Value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfigValue(int key)
        {
            var result = QueryFirstOrDefault<string>($"SELECT Value FROM Config(Nolock) WHERE ConfigKey = {key}", commandType: CommandType.Text);
            return result;
        }

        /// <summary>
        /// Generate Code
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columName"></param>
        /// <returns></returns>
        public string GetCodeGenerate(string tableName, string columName)
        {
            var parameters = new DynamicParameters(new
            {
                Table = tableName,
                CodeName = columName
            });

            parameters.Add("@GeneratedCode", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

            Execute("GetCodeGenerate", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<string>("@GeneratedCode");
        }
        /// <summary>
        /// Write Log Exception
        /// </summary>
        /// <param name="log"></param>
        public void LogExeption(LogExceptionModel log)
        {
            var parameters = new DynamicParameters(new
            {
                UserName = log.UserName,
                IPAddress = log.IPAddress,
                Message = log.Message,
                StackTrace = log.StackTrace,
                RequestPath = log.RequestPath,
                RequestMethod = log.RequestMethod,            
                ExceptionDate = log.ExceptionDate
            }) ;

            string query = "INSERT INTO ExceptionLogs(UserName,IPAddress,Message,StackTrace,RequestPath,RequestMethod,ExceptionDate)\r\n\tVALUES (@UserName,@IPAddress,@Message,@StackTrace,@RequestPath,@RequestMethod,@ExceptionDate)";
            Execute(query, parameters, commandType: CommandType.Text);
        }
    }
}
