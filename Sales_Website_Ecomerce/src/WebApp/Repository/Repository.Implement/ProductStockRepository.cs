using Dapper;
using Models.RequestModel.ProductStock;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Repository.Implement
{
    public class ProductStockRepository : Repository, IProductStockRepository
    {
        public ProductStockRepository(SqlConnection connection, SqlTransaction transaction)
        {
            this._context = connection;
            this._transaction = transaction;
        }
        /// <summary>
        /// Create ProductStock Repository
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Create(CreateProductStockRequestModel item)
        {
            var parameters = new DynamicParameters(new
            {
                ProductID = item.ProductID,
                SupplierID = item.SupplierID,
                Name = item.Name,
                Code = item.Code,
                StatusID = item.StatusID,
                WareHouseID = item.WareHouseID,
                Description = item.Description,
                CreateBy = item.CreateBy
            });
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
            Execute("SP_CreateProductStock", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@Result");
        }
    }
}
