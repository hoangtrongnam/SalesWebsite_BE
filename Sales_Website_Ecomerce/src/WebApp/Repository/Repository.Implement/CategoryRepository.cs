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
        public CategoryResponseModel Create(CreateCategoryRequestModel item)
        {
            var parameters = new DynamicParameters(new
            {
                Parent = item.Parent,
                Name = item.Name,
                TenantID = item.TenantID,
                Description = item.Description,
                CreateBy = item.CreateBy
            });

            var result = QueryFirstOrDefault<CategoryResponseModel>("SP_Create_Category", parameters, commandType: CommandType.StoredProcedure);

            return result;
        }
        /// <summary>
        /// Get categoy by condition
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public CategoryResponseModel GetByCondition(GetCategoryCommonRequestModel item)
        {
            var parameters = new DynamicParameters(new
            {
                Parent = item.Parent,
                Name = item.Name,
                TenantID = item.TenantID,
                ID = item.ID,
            });
            var result = QueryFirstOrDefault<CategoryResponseModel>("SP_Get_CategoryTenantByID_Or_Name", parameters, commandType: CommandType.StoredProcedure);

            return result;
        }
        /// <summary>
        /// Get category by tenant, parent
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public List<CategoryResponseModel> GetCategoryTenantParent(GetCategoryCommonRequestModel item)
        {
            var parameters = new DynamicParameters(new
            {
                Parent = item.Parent,
                TenantID = item.TenantID,
            });
            var result = Query<CategoryResponseModel>("SP_Get_CategoryTenantParent", parameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
        /// <summary>
        /// Update category repository
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Update(UpdateCategoryRequestModel item, int id)
        {
            var parameters = new DynamicParameters(new
            {
                ID = id,
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
        public int Remove(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
            Execute("SP_DeleteCategory", new {ID = id }, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@Result");
        }
        /// <summary>
        /// Get category by id repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CategoryResponseModel Get(int id)
        {
            var result = QueryFirstOrDefault<CategoryResponseModel>("SP_Get_CategoryByID", new {ID = id}, commandType: CommandType.StoredProcedure);

            return result;
        }
    }
}
