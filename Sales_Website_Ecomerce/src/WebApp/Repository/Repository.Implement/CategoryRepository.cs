using Dapper;
using Models.RequestModel.Category;
using Models.ResponseModels.Category;
using Repository.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Implement
{
    public class CategoryRepository : Repository, ICategoryRepository
    {
        public CategoryRepository(SqlConnection context, SqlTransaction _transaction)
        {
            this._context = context;
            this._transaction = _transaction;
        }
        /// <summary>
        /// Create Category
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public CategoryResponseModel Create(CreateCategoryRepositoryRequestModel item, Guid TenantID)
        {
            var parameters = new DynamicParameters(new
            {
                CategoryID = item.CategoryID,
                CategoryCode = item.CategoryCode,
                Value = item.Value,
                Parent = item.Parent,
                Name = item.Name,
                TenantID = TenantID,
                Description = item.Description,
                CreateBy = item.CreateBy
            });

            var result = QueryFirstOrDefault<CategoryResponseModel>("SP_Create_Category", parameters, commandType: CommandType.StoredProcedure);

            return result;
        }
        /// <summary>
        /// Get child categoy by categoryid
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public List<CategoryResponseModel> GetChildCategoysById(Guid CategoryID)
        {
            var result = Query<CategoryResponseModel>("SP_Get_AllChildCategoryById", new { CategoryID = CategoryID }, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
        /// <summary>
        /// Update category repository
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Update(UpdateCategoryRequestModel item, Guid CategoryID)
        {
            var parameters = new DynamicParameters(new
            {
                CategoryID = CategoryID,
                Name = item.Name,
                Parent = item.Parent,
                Description = item.Description,
                UpdateBy = item.UpdateBy
            });
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
            Execute("SP_UpdateCategory", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@Result");
        }
        /// <summary>
        /// Remove Category Repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Remove(Guid CategoryID)
        {
            var parameters = new DynamicParameters(new{CategoryID = CategoryID});
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
            Execute("SP_DeleteCategory", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@Result");
        }
        /// <summary>
        /// Get category by id repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CategoryResponseModel Get(Guid id)
        {
            var result = QueryFirstOrDefault<CategoryResponseModel>("SP_Get_CategoryByID", new { CategoryID = id}, commandType: CommandType.StoredProcedure);

            return result;
        }
        /// <summary>
        /// Get All Category
        /// </summary>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public List<CategoryResponseModel> GetAllCategory(Guid TenantID)
        {
            var result = Query<CategoryResponseModel>("SP_GetAllCategoryTenant", new { TenantID = TenantID}, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
        /// <summary>
        /// Get status by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<StatusResponseModel> GetStatus(string key)
        {
            var result = Query<StatusResponseModel>($"SELECT StatusID,Status,Name,Description,Type FROM Status(Nolock) WHERE Type = '{key}'", commandType: CommandType.Text).ToList();
            return result;
        }
    }
}
