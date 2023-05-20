using Dapper;
using Models.RequestModel.Product;
using Repository.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Implement
{
    public class ProductRepository : Repository, IProductRepository
    {
        public ProductRepository(SqlConnection context, SqlTransaction _transaction)
        {
            this._context = context;
            this._transaction = _transaction;
        }
        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Create(CreateProductRequestModel item)
        {
            var parameters = new DynamicParameters(new
            {
                CategoryID = item.CategoryID,
                Name = item.Name,
                Description = item.Description,
                Code = item.Code,
                Quantity = item.Quantity,
                StatusID = item.StatusID,
                TenantID = item.TenantID,
                CreateBy = item.CreateBy
            });
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
            
            Execute("SP_CreateProduct", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }
    }
}
 