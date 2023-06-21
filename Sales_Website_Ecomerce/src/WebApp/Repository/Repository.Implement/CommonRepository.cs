using Dapper;
using Repository.Interface;
using System.Data;
using System.Data.SqlClient;

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
    }
}
