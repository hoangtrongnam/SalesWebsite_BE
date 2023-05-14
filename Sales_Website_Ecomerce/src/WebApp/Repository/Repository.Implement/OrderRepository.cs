using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interface;
using Repository.Interfaces.Actions;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Implement
{
    public class OrderRepository : Repository, IOrderRepository
    {
        public OrderRepository(SqlConnection context, SqlTransaction _transaction)
        {
            this._context = context;
            this._transaction = _transaction;
        }

        public int Create(OrderRequestModel item)
        {
            #region 1.1 Insert into table Order
            var command = CreateCommand("sp_InsertOrder");
            command.Parameters.AddWithValue("@CustomerID", item.CustomerID);
            command.Parameters.AddWithValue("@Status", 10); //Có dơn hàng mới cần sale xác nhận

            //Thêm tham số đầu ra cho stored procedure
            SqlParameter outputParam = new SqlParameter("@OrderID", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            command.Parameters.Add(outputParam);

            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();

            //Lấy giá trị đầu ra từ tham số đầu ra
            int orderID = (int)outputParam.Value;

            #endregion

            #region 1.2 insert into table OrderProduct
            //Create a table variable to store the records
            DataTable table = new DataTable();
            table.Columns.Add("OrderID", typeof(int));
            table.Columns.Add("ProductID", typeof(int));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("Price", typeof(decimal));

            // Add the records to the table variable
            foreach (var record in item.lstProduct)
            {
                table.Rows.Add(orderID, record.ProductID, record.Quantity, record.Price);
            }

            // Create a command object to execute the stored procedure
            command = CreateCommand("sp_InsertOrderProduct");
            command.CommandType = CommandType.StoredProcedure;

            // Add the table variable as a parameter to the command object
            SqlParameter parameter = command.Parameters.AddWithValue("@Records", table);
            parameter.SqlDbType = SqlDbType.Structured;
            #endregion

            return command.ExecuteNonQuery() > 0 ? orderID : command.ExecuteNonQuery();
        }

        //public CategoryResponseModel Get(int id)
        //{
        //    var command = CreateCommand("sp_GetCategoryById");
        //    command.Parameters.AddWithValue("@categoryId", id);
        //    command.CommandType = System.Data.CommandType.StoredProcedure;

        //    using (var reader = command.ExecuteReader())
        //    {
        //        reader.Read();
        //        //ResultModel result = new ResultModel();
        //        CategoryResponseModel categoryResponseModel = new CategoryResponseModel();
        //        categoryResponseModel.Name = reader["Name"].ToString() ?? "";

        //        return categoryResponseModel;
        //    };
        //}

        //public CategoryResponseModel Get(int id, int pageIndex)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<CategoryResponseModel> GetAll()
        //{
        //    var command = CreateCommand("sp_GetAllCategory");
        //    command.CommandType = System.Data.CommandType.StoredProcedure;

        //    var lstCate = new List<CategoryResponseModel>();

        //    using (var reader = command.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            CategoryResponseModel categoryResponseModel = new CategoryResponseModel();
        //            categoryResponseModel.Name = reader["Name"].ToString();
        //            lstCate.Add(categoryResponseModel);
        //        }
        //    };
        //    return lstCate;
        //}

        //public List<CategoryResponseModel> GetAll(int pageIndex)
        //{
        //    throw new NotImplementedException();
        //}

        //public int Remove(int id)
        //{
        //    //throw new NotImplementedException();
        //    var command = CreateCommand("sp_DeleteCategoryAndProducts");
        //    command.Parameters.AddWithValue("@CategoryId", id);

        //    command.CommandType = System.Data.CommandType.StoredProcedure;

        //    return command.ExecuteNonQuery();
        //}

        //public int Remove(CategoryRequestModel item, int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public int Update(CategoryRequestModel item, int CategoryID)
        //{
        //    //throw new NotImplementedException();
        //    var command = CreateCommand("sp_UpdateCategory");
        //    command.Parameters.AddWithValue("@categoryId", CategoryID);
        //    command.Parameters.AddWithValue("@Name", item.Name);

        //    command.CommandType = System.Data.CommandType.StoredProcedure;

        //    return command.ExecuteNonQuery();
        //}
    }
}
