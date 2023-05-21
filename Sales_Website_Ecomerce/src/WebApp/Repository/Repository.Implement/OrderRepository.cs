using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interface;
using System.Data;
using System.Data.SqlClient;
using static Models.ResponseModels.OrderResponseModel;

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

        public OrderResponseModel Get(int orderID)
        {
            OrderResponseModel orderResponseModel = new OrderResponseModel();
            //1. Get customer info

            //2. Get Oder Info
            orderResponseModel = GetOrderInfo(orderID);

            //3. Get Order Detail
            orderResponseModel.lstProduct = GetOrderDetail(orderID, out decimal totalPayment).lstProduct;
            orderResponseModel.TotalPayment = totalPayment;

            return orderResponseModel;
        }

        public OrderResponseModel GetOrderInfo(int orderID)
        {
            var command = CreateCommand("sp_GetOrderById");
            command.Parameters.AddWithValue("@OrderID", orderID);
            command.Parameters.AddWithValue("@Status", -1);//get all
            command.CommandType = System.Data.CommandType.StoredProcedure;
            OrderResponseModel orderResponseModel = new OrderResponseModel();

            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                //ResultModel result = new ResultModel();
                orderResponseModel.OrderID = reader["OrderID"].ToString() ?? "";
                orderResponseModel.CustomerID = reader["CustomerID"].ToString() ?? "";
                orderResponseModel.DepositAmount = reader["DepositAmount"].ToString() ?? "";
                orderResponseModel.Note = reader["Note"].ToString() ?? "";
                orderResponseModel.Status = reader["Status"].ToString() ?? "";
                orderResponseModel.CreateDate = reader["CreateDate"].ToString() ?? "";

                reader.Close();
            };
            return orderResponseModel;
        }

        public OrderResponseModel GetLstOrder(int Status)
        {
            var command = CreateCommand("sp_GetListOrder");
            command.Parameters.AddWithValue("@Status", Status);

            //Thêm tham số đầu ra cho stored procedure
            SqlParameter outputParam = new SqlParameter("@OrderList", SqlDbType.VarChar, 8000);
            outputParam.Direction = ParameterDirection.Output;
            command.Parameters.Add(outputParam);

            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();

            OrderResponseModel orderResponseModel = new OrderResponseModel();
            List<Product> lstProducts = new List<Product>();

            //Lấy giá trị đầu ra từ tham số đầu ra
            orderResponseModel.OrderID = (string)outputParam.Value;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product product = new Product();
                    product.ProductID = string.IsNullOrEmpty(reader["ProductID"].ToString()) ? 0 : Convert.ToInt32(reader["ProductID"]);
                    product.Quantity = string.IsNullOrEmpty(reader["Quantity"].ToString()) ? 0 : Convert.ToInt32(reader["Quantity"]);

                    lstProducts.Add(product);
                }
                orderResponseModel.lstProduct = lstProducts;
                
                reader.Close();
            };
            return orderResponseModel;
        }

        public OrderResponseModel GetOrderDetail(int orderID, out decimal totalPayment)
        {
            OrderResponseModel orderResponseModel = GetOrderProduct(orderID);
            totalPayment = orderResponseModel.TotalPayment;

            return orderResponseModel;
        }

        public List<OrderResponseModel> GetAll(int pageIndex)
        {
            throw new NotImplementedException();
        }

        public List<OrderResponseModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public OrderResponseModel Get(int orderID, int pageIndex)
        {
            throw new NotImplementedException();
        }
        public int Remove(int OrderID)
        {
            //1. Delete OrderProducts
            var command = CreateCommand("sp_DeleteOrderProducts");
            command.Parameters.AddWithValue("@OrderID", OrderID);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            if (command.ExecuteNonQuery() > 0)
            {
                //2. Delete Orders
                command = CreateCommand("sp_DeleteOrders");
                command.Parameters.AddWithValue("@OrderID", OrderID);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                return command.ExecuteNonQuery();
            }
            else return 0; //Delete OrderProducts: Error
        }

        //public int Remove(CategoryRequestModel item, int id)
        //{
        //    throw new NotImplementedException();
        //}

        public int Update(OrderRequestModel item, int orderID)
        {
            var command = CreateCommand("sp_UpdateOrder");
            command.Parameters.AddWithValue("@DepositAmount", item.DepositAmount);
            command.Parameters.AddWithValue("@Note", item.Note);
            command.Parameters.AddWithValue("@Status", item.Status);
            command.Parameters.AddWithValue("@OrderID", orderID);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            return command.ExecuteNonQuery();
        }

        private OrderResponseModel GetOrderProduct(int orderID)
        {
            var command = CreateCommand("sp_GetOrderDetailById");
            command.Parameters.AddWithValue("@OrderID", orderID);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            OrderResponseModel orderResponseModel = new OrderResponseModel();

            List<Product> lstProduct = new List<Product>();
            decimal totalPayment = 0;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product product = new Product();
                    product.ProductID = string.IsNullOrEmpty(reader["ProductID"].ToString()) ? 0 : Convert.ToInt32(reader["ProductID"]);
                    //product.Name = reader["Name"].ToString() ?? "";
                    //product.Code = reader["Code"].ToString() ?? "";
                    product.Quantity = string.IsNullOrEmpty(reader["Quantity"].ToString()) ? 0 : Convert.ToInt32(reader["Quantity"]);
                    product.Price = string.IsNullOrEmpty(reader["Price"].ToString()) ? 0 : Convert.ToDecimal(reader["Price"]);
                    totalPayment += product.Quantity * product.Price;

                    lstProduct.Add(product);
                }
                reader.Close();
            }
            orderResponseModel.TotalPayment = totalPayment;
            orderResponseModel.lstProduct = lstProduct;

            return orderResponseModel;
        }
        public int Remove(OrderRequestModel item, int OrderID)
        {
            throw new NotImplementedException();
        }
    }
}
