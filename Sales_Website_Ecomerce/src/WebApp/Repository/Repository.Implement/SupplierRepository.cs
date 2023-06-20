using Dapper;
using Models.RequestModel.Supplier;
using Models.ResponseModels.Supplier;
using Repository.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Implement
{
    public class SupplierRepository : Repository, ISupplierRepository
    {
        public SupplierRepository(SqlConnection context, SqlTransaction _transaction)
        {
            this._context = context;
            this._transaction = _transaction;
        }
        /// <summary>
        /// Create Supplier Repository
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Create(CreateSupplierRepositoryRequestModel item)
        {
            var parameters = new DynamicParameters(new
            {
                SupplierID = item.SupplierID,
                SupplierCode = item.SupplierCode,
                Name = item.Name,
                Address = item.Address,
                PhoneNumber = item.PhoneNumber,
                Description = item.Description,
                CreateBy = item.CreateBy
            });
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
            Execute("SP_CreateSupplier", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@Result");
        }
        /// <summary>
        /// Get supplier by id Repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SupplierResponseModel Get(Guid id)
        {
            var result = QueryFirstOrDefault<SupplierResponseModel>("SP_GetSupplierByID", new { SupplierID = id }, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
