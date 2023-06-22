using Dapper;
using Models.RequestModel.WareHouse;
using Models.ResponseModels.WareHouse;
using Repository.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Implement
{
    public class WareHouseRepository : Repository, IWareHouseRepository
    {
        public WareHouseRepository(SqlConnection context, SqlTransaction _transaction)
        {
            this._context = context;
            this._transaction = _transaction;
        }
        /// <summary>
        /// Create WareHouse Repository
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Create(CreateWareHouseRepositoryRequestModel item)
        {
            var parameters = new DynamicParameters(new
            {
                WareHouseID = item.WareHouseID,
                WareHouseCode = item.WareHouseCode,
                Name = item.Name,
                Address = item.Address,
                Description = item.Description,
                CreateBy = item.CreateBy
            });
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
            Execute("SP_CreateWareHouse", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@Result"); ;
        }
        /// <summary>
        /// Get warehouse by id Repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WareHouseResponseModel Get(Guid id)
        {
            var result = QueryFirstOrDefault<WareHouseResponseModel>("SP_GetWareHouseByID", new { WareHouseID = id }, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
