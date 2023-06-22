using Models.ResponseModels.Tenant;
using Repository.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Implement
{
    public class TenantRepository : Repository, ITenantRepository
    {
        public TenantRepository(SqlConnection context, SqlTransaction _transaction)
        {
            this._context = context;
            this._transaction = _transaction;
        }
        /// <summary>
        /// Get Tenant By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TenantResponseModel Get(Guid id)
        {
            var result = QueryFirstOrDefault<TenantResponseModel>("SP_GetTenantByID", new { TenantID = id }, commandType: CommandType.StoredProcedure);
            return result;
        }
       
    }
}
