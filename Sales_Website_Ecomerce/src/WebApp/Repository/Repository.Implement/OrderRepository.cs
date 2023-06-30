using Common;
using Dapper;
using Models.RequestModel.Orders;
using Models.ResponseModels;
using Models.ResponseModels.Cart;
using Models.ResponseModels.Product;
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

        //public int Create(OrderRequestModel item)
        //{
        //    //1.1 Insert into table Order
        //    int efectRowCart = InsertOrder(item.CustomerID, Parameters.StatusOrderInsert); //New Order (StatusOrderInsert: co dơn hàng mới cần sale xác nhận))

        //    //1.2 insert into table OrderProduct

        //    int efectRowCartProduct = InsertOrderProduct(item.lstProduct, item.CartID);
        //    if (orderID > 0 && orderProductInsert > 0)
        //        return orderID; //Insert into table Order and OrderProduct Success

        //    return 0;
        //}

        public Guid InsertOrder(Guid customerID, int status, Guid orderID)
        {
            var parameters = new DynamicParameters(new
            {
                CustomerID = customerID,
                Status = status,
                OrderID = orderID
            });

            //parameters.Add("@OrderID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = Execute("sp_InsertOrder", parameters, commandType: CommandType.StoredProcedure);

            return result > 0 ? orderID : Guid.Empty;
        }

        public int InsertOrderProduct(List<ProductModel> lstProduct, Guid orderID)
        {
            var parameters = new DynamicParameters(new
            {
                ListProduct = lstProduct.ToDataTable().AsTableValuedParameter("OrderProductType"),
                OrderID = orderID,
                //OrderProductID = orderProductID
            });

            var effectRow = Execute("sp_InsertOrderProduct", parameters, commandType: CommandType.StoredProcedure);

            return effectRow > 0 ? effectRow : 0 ;
        }

        public OrderResponseModel Get(Guid orderID)
        {
            OrderResponseModel orderResponseModel = new OrderResponseModel();
            //1. Get customer info: chưa làm

            //2. Get Oder Info
            orderResponseModel = GetOrderInfo(orderID);

            if (!(orderResponseModel == null))
            {
                //3. Get Order Detail
                var orderdetail = GetOrderDetail(orderID);
                if (orderdetail.lstProduct.Count() > 0)
                {
                    orderResponseModel.lstProduct = orderdetail.lstProduct;
                    //orderResponseModel.TotalPayment = totalPayment;
                    return orderResponseModel;
                }
            }
            return null;
        }

        public OrderResponseModel GetOrderInfo(Guid orderID)
        {
            var parameters = new DynamicParameters(new
            {
                OrderID = orderID,
                Status = -1 //get đơn hàng chưa bị hủy
            });
            var result = QueryFirstOrDefault<OrderResponseModel>("sp_GetOrderById", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        //public OrderResponseModel GetLstOrder(int Status)
        //{
        //    var command = CreateCommand("sp_GetListOrder");
        //    command.Parameters.AddWithValue("@Status", Status);

        //    //Thêm tham số đầu ra cho stored procedure
        //    SqlParameter outputParam = new SqlParameter("@OrderList", SqlDbType.VarChar, 8000);
        //    outputParam.Direction = ParameterDirection.Output;
        //    command.Parameters.Add(outputParam);

        //    command.CommandType = CommandType.StoredProcedure;
        //    command.ExecuteNonQuery();

        //    OrderResponseModel orderResponseModel = new OrderResponseModel();
        //    List<Product> lstProducts = new List<Product>();

        //    //Lấy giá trị đầu ra từ tham số đầu ra
        //    orderResponseModel.OrderID = (string)outputParam.Value;

        //    using (var reader = command.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            Product product = new Product();
        //            product.ProductID = string.IsNullOrEmpty(reader["ProductID"].ToString()) ? 0 : Convert.ToInt32(reader["ProductID"]);
        //            product.Quantity = string.IsNullOrEmpty(reader["Quantity"].ToString()) ? 0 : Convert.ToInt32(reader["Quantity"]);

        //            lstProducts.Add(product);
        //        }
        //        orderResponseModel.lstProduct = lstProducts;

        //        reader.Close();
        //    };
        //    return orderResponseModel;
        //}

        public OrderResponseModel GetOrderDetail(Guid orderID)
        {
            OrderResponseModel orderResponseModel = GetOrderProduct(orderID);
            //totalPayment = orderResponseModel.TotalPayment;

            return orderResponseModel;
        }

        //public int Remove(int OrderID)
        //{
        //    //1. Delete OrderProducts
        //    var command = CreateCommand("sp_DeleteOrderProducts");
        //    command.Parameters.AddWithValue("@OrderID", OrderID);
        //    command.CommandType = System.Data.CommandType.StoredProcedure;
        //    if (command.ExecuteNonQuery() > 0)
        //    {
        //        //2. Delete Orders
        //        command = CreateCommand("sp_DeleteOrders");
        //        command.Parameters.AddWithValue("@OrderID", OrderID);
        //        command.CommandType = System.Data.CommandType.StoredProcedure;
        //        return command.ExecuteNonQuery();
        //    }
        //    else return 0; //Delete OrderProducts: Error
        //}

        public int Update(OrderCommonRequest item, Guid orderID, decimal totalPayment = 0)
        {
            var parameters = new DynamicParameters(new
            {
                DepositAmount = item.DepositAmount,
                Note = item.Note,
                Status = item.Status,
                OrderID = orderID,
                TotalPayment = totalPayment
            });

            var result = Execute("sp_UpdateOrder", parameters, commandType: CommandType.StoredProcedure);

            return result;
        }

        private OrderResponseModel GetOrderProduct(Guid orderID)
        {
            //var command = CreateCommand("sp_GetOrderDetailById");
            //command.Parameters.AddWithValue("@OrderID", orderID);
            //command.CommandType = System.Data.CommandType.StoredProcedure;
            OrderResponseModel orderResponseModel = new OrderResponseModel();

            //List<Product> lstProduct = new List<Product>();
            //decimal totalPayment = 0;
            //using (var reader = command.ExecuteReader())
            //{
            //    while (reader.Read())
            //    {
            //        Product product = new Product();
            //        product.ProductID = string.IsNullOrEmpty(reader["ProductID"].ToString()) ? 0 : Convert.ToInt32(reader["ProductID"]);
            //        product.PromoteID = string.IsNullOrEmpty(reader["PromoteID"].ToString()) ? 0 : Convert.ToInt32(reader["PromoteID"]);
            //        product.Name = reader["Name"].ToString() ?? "";
            //        //product.Code = reader["Code"].ToString() ?? "";
            //        product.Quantity = string.IsNullOrEmpty(reader["Quantity"].ToString()) ? 0 : Convert.ToInt32(reader["Quantity"]);
            //        product.Price = string.IsNullOrEmpty(reader["Price"].ToString()) ? 0 : Convert.ToDecimal(reader["Price"]);
            //        product.WarehouseID = string.IsNullOrEmpty(reader["WarehouseID"].ToString()) ? 0 : Convert.ToInt32(reader["WarehouseID"]);
            //        //totalPayment += product.Quantity * product.Price;

            //        lstProduct.Add(product);
            //    }
            //    reader.Close();
            //}
            //orderResponseModel.TotalPayment = totalPayment;
            //orderResponseModel.lstProduct = lstProduct;
            ////
            var param = new DynamicParameters(new
            {
                OrderID = orderID
            });

            var lstProductsOrder = Query<Product>("sp_GetOrderDetailById", param, commandType: CommandType.StoredProcedure).ToList();
            orderResponseModel.lstProduct = lstProductsOrder;

            //return result;

            return orderResponseModel;
        }

        /// <summary>
        /// get 1 record Promote by PromoteID and Expire
        /// </summary>
        /// <param name="PromoteID"></param>
        /// <returns></returns>
        public PriceResponseModel GetPromote(Guid PromoteID)
        {
            var result = QueryFirstOrDefault<PriceResponseModel>("SP_GetPromoteByPromoteID", new { PromoteID = PromoteID }, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}