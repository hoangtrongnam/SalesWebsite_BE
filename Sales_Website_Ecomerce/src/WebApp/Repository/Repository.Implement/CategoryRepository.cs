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
        public CategoryResponseModel Get(GetCategoryCommonRequestModel item)
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
        public List<CategoryResponseModel> GetAll(GetCategoryCommonRequestModel item)
        {
            var parameters = new DynamicParameters(new
            {
                Parent = item.Parent,              
                TenantID = item.TenantID,             
            });
            var result = Query<CategoryResponseModel>("SP_Get_CategoryTenantParent", parameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public CategoryResponseModel GetByCondition(params int[] values)
        {
            throw new NotImplementedException();
        }
    }
}
