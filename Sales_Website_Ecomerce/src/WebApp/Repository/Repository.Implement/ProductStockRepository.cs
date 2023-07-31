using Common;
using Dapper;
using Models.RequestModel.ProductStock;
using Models.ResponseModels.ProductStocks;
using Repository.Interface;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

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
        public int Create(CreateProductStockRepositoryRequestModel item)
        {
            var parameters = new DynamicParameters(new
            {
                ProductStockID = item.ProductStockID,
                ProductStockCode = item.ProductStockCode,
                ProductID = item.ProductID,
                SupplierID = item.SupplierID,
                ColorID = item.ColorID,
                SizeID = item.SizeID,
                Name = item.Name,
                Code = item.Code,
                Status = item.Status,
                ImportPrice = item.ImportPrice,
                ExfactoryPrice = item.ExfactoryPrice,
                WareHouseID = item.WareHouseID,
                Description = item.Description,
                CreateBy = Parameters.CreateBy
            });
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
            Execute("SP_CreateProductStock", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@Result");
        }

        /// <summary>
        /// Method Get Product Hold
        /// SangNguyen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ProductStockResponseModel GetHoldProduct(HoldProductRequestModel model)
        {
            var parameters = new DynamicParameters(new
            {
                OrderID = model.OrderID,
                StatusID = Parameters.SaleHoldProduct,
                UpdateBy = "",
                ListImage = model.LstHoldProducts.ToDataTable().AsTableValuedParameter("dbo.HoldProductType"),
            });

            var effectRow = Query<ProductStockResponseModel>("SP_GetHoldProductStock", parameters, commandType: CommandType.StoredProcedure);
            return new ProductStockResponseModel();
        }

        /// <summary>
        /// Method Update table ProductStock (HoldProduct)
        /// SangNguyen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int HoldProduct(HoldProductRequestModel model)
        {
            var parameters = new DynamicParameters(new
            {
                LstHoldProduct = model.LstHoldProducts.ToDataTable().AsTableValuedParameter("dbo.HoldProductType"),
                OrderID = model.OrderID,
                StatusID = Parameters.SaleHoldProduct,
                UpdateBy = "",
            });

            var effectRow = Execute("SP_HoldProductStock", parameters, commandType: CommandType.StoredProcedure);
            return effectRow;
        }
    }
}
