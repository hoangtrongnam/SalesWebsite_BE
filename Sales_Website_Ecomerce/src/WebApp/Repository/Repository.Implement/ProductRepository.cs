using Common;
using Dapper;
using Models.RequestModel.Product;
using Models.ResponseModels.Category;
using Models.ResponseModels.Product;
using Repository.Interface;
using Repository.Interfaces.Actions;
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
        public int Create(CreateOnlyProductRequestModel item)
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

        /// <summary>
        /// Insert mutiple image
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int CreateImages(List<ImageRequestModel> item)
        {
            var parameters = new DynamicParameters(new
            {
                ListImage = item.ToDataTable().AsTableValuedParameter("dbo.ImageType")
            });

            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            Execute("SP_CreateImage", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }
        /// <summary>
        /// Insert mutiple price
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int CreatePrices(List<PriceRequestModel> item)
        {
            var parameters = new DynamicParameters(new
            {
                ListPrice = item.ToDataTable().AsTableValuedParameter("dbo.PriceType")
            });

            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            Execute("SP_CreatePrice", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }
        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductResponseModel Get(int id)
        {
            var result = QueryFirstOrDefault<ProductResponseModel>("SP_GetProductByID", new { ProductID = id }, commandType: CommandType.StoredProcedure);
            return result;
        }
        /// <summary>
        /// get list image by productid
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public List<ImageResponseModel> GetImages(int ProductID)
        {
            var result = Query<ImageResponseModel>("SP_GetImagesByProductID",new { ProductID  = ProductID}, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
        /// <summary>
        /// get list price by productid
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public List<PriceResponseModel> GetPrices(int ProductID)
        {
            var result = Query<PriceResponseModel>("SP_GetPricesByProductID", new { ProductID = ProductID }, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
        /// <summary>
        /// Get product by category repository
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        public List<ProductResponseModel> GetProductCategory(int CategoryID)
        {
            var result = Query<ProductResponseModel>("SP_GetProductByCategory", new { CategoryID = CategoryID }, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
        /// <summary>
        /// Update Product Repository
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Update(UpdateProductRequestModel item, int id)
        {
            var parameters = new DynamicParameters(new
            {
                ProductID = id,
                Name = item.Name,
                Code = item.Code,
                Description = item.Description,
                UpdateBy = item.UpdateBy
            });
            parameters.Add("@Result", dbType: DbType.Int32,direction: ParameterDirection.Output);

            Execute("SP_UpdateProduct", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }
    }
}
 