using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interface;
using Repository.Interfaces.Actions;
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

        public int Create(CategoryRequestModel item)
        {
            //throw new NotImplementedException();
            var command = CreateCommand("sp_InsertCategory");
            command.Parameters.AddWithValue("@Name", item.Name);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            return command.ExecuteNonQuery();
        }

        public CategoryResponseModel Get(int id)
        {
            var command = CreateCommand("sp_GetCategoryById");
            command.Parameters.AddWithValue("@categoryId", id);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                //ResultModel result = new ResultModel();
                CategoryResponseModel categoryResponseModel = new CategoryResponseModel();
                categoryResponseModel.Name = reader["Name"].ToString() ?? "";

                return categoryResponseModel;
            };
        }

        public CategoryResponseModel Get(int id, int pageIndex)
        {
            throw new NotImplementedException();
        }

        public List<CategoryResponseModel> GetAll()
        {
            var command = CreateCommand("sp_GetAllCategory");
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var lstCate = new List<CategoryResponseModel>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    CategoryResponseModel categoryResponseModel = new CategoryResponseModel();
                    categoryResponseModel.Name = reader["Name"].ToString(); 
                    lstCate.Add(categoryResponseModel);
                }
            };
            return lstCate;
        }

        public List<CategoryResponseModel> GetAll(int pageIndex)
        {
            throw new NotImplementedException();
        }

        public int Remove(int id)
        {
            //throw new NotImplementedException();
            var command = CreateCommand("sp_DeleteCategoryAndProducts");
            command.Parameters.AddWithValue("@CategoryId", id);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            return command.ExecuteNonQuery();
        }

        public int Remove(CategoryRequestModel item, int id)
        {
            throw new NotImplementedException();
        }

        public int Update(CategoryRequestModel item, int CategoryID)
        {
            //throw new NotImplementedException();
            var command = CreateCommand("sp_UpdateCategory");
            command.Parameters.AddWithValue("@categoryId", CategoryID);
            command.Parameters.AddWithValue("@Name", item.Name);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            return command.ExecuteNonQuery();
        }
    }
}
