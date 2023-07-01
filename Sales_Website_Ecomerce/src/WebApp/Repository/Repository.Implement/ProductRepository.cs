using Common;
using Dapper;
using Models.RequestModel;
using Models.RequestModel.Product;
using Models.ResponseModels.Product;
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
        public int Create(CreateOnlyProductRepositoryRequestModel item, Guid tenantID)
        {
            var parameters = new DynamicParameters(new
            {
                ProductID = item.ProductID,
                ProductCode = item.ProductCode,
                CategoryID = item.CategoryID,
                Name = item.Name,
                Code = item.Code,
                Description = item.Description,
                Price = item.Price,
                Status = item.Status,
                TenantID = tenantID,
                CreateBy = Parameters.CreateBy
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
        public int CreateImages(List<ImageRepositoryRequestModel> item)
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
        public int CreatePrices(List<PriceRepositoryRequestModel> item)
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
        public ProductResponseModel Get(Guid id)
        {
            var result = QueryFirstOrDefault<ProductResponseModel>("SP_GetProductByID", new { ProductID = id }, commandType: CommandType.StoredProcedure);
            return result;
        }
        /// <summary>
        /// get list image by productid
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public List<ImageResponseModel> GetImages(Guid productID)
        {
            var result = Query<ImageResponseModel>("SP_GetImagesByProductID",new { ProductID  = productID}, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
        /// <summary>
        /// get list price by productid
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public List<PriceResponseModel> GetPrices(Guid productID)
        {
            var result = Query<PriceResponseModel>("SP_GetPricesByProductID", new { ProductID = productID }, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
        /// <summary>
        /// Get product by category repository
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        public List<ProductResponseModel> GetProductCategory(Guid categoryID)
        {
            var result = Query<ProductResponseModel>("SP_GetProductByCategory", new { CategoryID = categoryID }, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        /// <summary>
        /// Get products by tenant
        /// </summary>
        /// <returns></returns>
        public List<ProductResponseModel> GetProducts(Guid tenantId)
        {
            var result = Query<ProductResponseModel>("SELECT * FROM Product a WHERE a.TenantID=@TenantID", new { TenantID = tenantId }, commandType: CommandType.Text).ToList();
            return result;
        }
        /// <summary>   
        /// Update Product Repository
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Update(UpdateProductRequestModel item, Guid id)
        {
            var parameters = new DynamicParameters(new
            {
                ProductID = id,
                Name = item.Name,
                Price = item.Price,
                Code = item.Code,
                Description = item.Description,
                UpdateBy = Parameters.CreateBy
            });
            parameters.Add("@Result", dbType: DbType.Int32,direction: ParameterDirection.Output);

            Execute("SP_UpdateProduct", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }
    }
}
 