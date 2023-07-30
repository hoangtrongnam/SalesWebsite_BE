using Common;
using Dapper;
using Models.RequestModel.AtributeProduct;
using Models.RequestModel.Product;
using Models.ResponseModels.AtributeProduct;
using Models.ResponseModels.Product;
using Repository.Interface;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace Repository.Implement
{
    public class AtributeProductRepository : Repository, IAtributeProductRepository
    {
        public AtributeProductRepository(SqlConnection context, SqlTransaction _transaction)
        {
            this._context = context;
            this._transaction = _transaction;
        }
        /// <summary>
        /// Create Color Repository
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateColor(ColorRepositoryRequestModel model)
        {
            var parameters = new DynamicParameters(new
            {
                ColorID = Guid.NewGuid(),
                ColorCode = model.ColorCode,
                Name = model.Name,
                Description = model.Description
            });

            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            Execute("Sp_CreateColor", parameters, commandType: CommandType.StoredProcedure);

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
        /// Create ProductColorImage repository
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateProductColorImage(List<ProductColorImageRepositoryRequestModel> model)
        {
            var parameters = new DynamicParameters(new
            {
                ListProductImageColor = model.ToDataTable().AsTableValuedParameter("dbo.ProductImageColorType")
            });

            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            Execute("Sp_CreateProductImageColor", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }
        /// <summary>
        /// Create Size Repository
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateSize(SizeRepositoryRequestModel model)
        {
            var parameters = new DynamicParameters(new
            {
                SizeID = Guid.NewGuid(),
                Value = model.Value,
                Description = model.Description
            });

            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            Execute("Sp_CreateSize", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result");
        }
        /// <summary>
        ///  Get all images
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<ImageResponseModel> GetAllImages()
        {
            var result = Query<ImageResponseModel>("SP_GetAllImages", commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
        /// <summary>
        /// get colors repository
        /// </summary>
        /// <returns></returns>
        public List<ColorResponseModel> GetColors()
        {
            var result = Query<ColorResponseModel>("SELECT ColorID, ColorCode,Name, Description FROM [dbo].[Color]", commandType: CommandType.Text).ToList();
            return result;
        }
        /// <summary>
        /// get sizes repository
        /// </summary>
        /// <returns></returns>
        public List<SizeResponseModel> GetSizes()
        {
            var result = Query<SizeResponseModel>("SELECT SizeID, Value, Description FROM [dbo].[Size]", commandType: CommandType.Text).ToList();
            return result;
        }
    }
}
