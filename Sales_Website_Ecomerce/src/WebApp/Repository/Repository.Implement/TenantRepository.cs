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
        public TenantResponseModel Get(int id)
        {
            return QueryFirstOrDefault<TenantResponseModel>("SP_GetTenantByID", new { ID = id }, commandType: CommandType.StoredProcedure);
        }
        public List<TenantResponseModel> GetAll(int item)
        {
            throw new NotImplementedException();
        }
        public TenantResponseModel GetByCondition(params int[] values)
        {
            throw new NotImplementedException();
        }
    }
}
