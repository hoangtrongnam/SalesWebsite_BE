using Common;
using Dapper;
using Models.RequestModel;
using Models.ResponseModels;
using Models.ResponseModels.Cart;
using Repository.Interface;
using System.Collections.Generic;
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
            //1.1 Insert into table Order
            int orderID = InsertOrder(item.CustomerID, Parameters.StatusOrderInsert); //New Order (StatusOrderInsert: co dơn hàng mới cần sale xác nhận))

            //1.2 insert into table OrderProduct

            int orderProductInsert = InsertOrderProduct(item.lstProduct, orderID);
            if (orderID > 0 && orderProductInsert > 0)
                return orderID; //Insert into table Order and OrderProduct Success

            return 0;
        }

        private int InsertOrder(int customerID, int status)
        {
            var parameters = new DynamicParameters(new
            {
                CustomerID = customerID,
                Status = status
            });

            parameters.Add("@OrderID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = Execute("sp_InsertOrder", parameters, commandType: CommandType.StoredProcedure);

            return result > 0 ? parameters.Get<int>("OrderID") : 0;
        }

        private int InsertOrderProduct(List<ProductModel> lstProduct, int orderID)
        {
            var parameters = new DynamicParameters(new
            {
                ListProduct = lstProduct.ToDataTable().AsTableValuedParameter("OrderProductType"),
                OrderID = orderID
            });

            var result = Execute("sp_InsertOrderProduct", parameters, commandType: CommandType.StoredProcedure);

            return result;
        }

        public OrderResponseModel Get(int orderID)
        {
            OrderResponseModel orderResponseModel = new OrderResponseModel();
            //1. Get customer info: chưa làm

            //2. Get Oder Info
            orderResponseModel = GetOrderInfo(orderID);

            //3. Get Order Detail
            var lstProduct = GetOrderDetail(orderID, out decimal totalPayment);
            if (lstProduct.lstProduct.Count() > 0)
            {
                orderResponseModel.lstProduct = lstProduct.lstProduct;
                orderResponseModel.TotalPayment = totalPayment;
                return orderResponseModel;
            }
            return null;
        }

        public OrderResponseModel GetOrderInfo(int orderID)
        {
            var parameters = new DynamicParameters(new
            {
                OrderID = orderID,
                Status = -1 //get all
            });
            var result = QueryFirstOrDefault<OrderResponseModel>("sp_GetOrderById", parameters, commandType: CommandType.StoredProcedure);
            return result;
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
                    product.Name = reader["Name"].ToString() ?? "";
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
    }
}