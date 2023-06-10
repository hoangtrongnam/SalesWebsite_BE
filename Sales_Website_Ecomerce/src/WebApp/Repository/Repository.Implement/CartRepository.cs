using Common;
using Dapper;
using Models.RequestModel.Cart;
using Models.ResponseModels.Cart;
using Models.ResponseModels.WareHouse;
using Repository.Interface;
using Repository.Interfaces.Actions;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace Repository.Implement
{
    public class CartRepository : Repository, ICartRepository
    {
        public CartRepository(SqlConnection context, SqlTransaction _transaction)
        {
            this._context = context;
            this._transaction = _transaction;
        }

        public int GetProductInStock(int productID, int wareHouseID)
        {
            //var parameters = new DynamicParameters(new
            //{
            //    ProductID = productID,
            //    WareHouseID = wareHouseID,
            //    status = 1
            //});
            var result = Query<object>("SP_Get_ProductStock", new { ProductID = productID, WareHouseID = wareHouseID }, commandType: CommandType.StoredProcedure).ToList();

            return result.Count();
        }

        public int CreateCart(int customerID)
        {
            var parameters = new DynamicParameters(new
            {
                CustomerID = customerID,
                Stautus = 10 //New cart
            });
            parameters.Add("@CartID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = Execute("sp_InsertCart", parameters, commandType: CommandType.StoredProcedure);

            return result > 0 ? parameters.Get<int>("CartID") : 0;
        }

        public int UpdateCartProduct(CartRequestModel item, int cartID)
        {
            var parameters = new DynamicParameters(new
            {
                CartID = cartID,
                ProdutID = item.ProdutID,
                Quantity = item.Quantity,
                StatusID = item.StatusID, //status update
                WareHouseID = item.WarehouseID
            });

            var result = Execute("sp_UpdateCartProduct", parameters, commandType: CommandType.StoredProcedure);
            return result;

            //return Query<object>("SP_Get_ProductStock", new { ID = productID, WareHouseID = wareHouseID }, commandType: CommandType.StoredProcedure).ToList().Count();

            //SqlCommand command = CreateCommand("sp_UpdateCartProduct");
            //command.Parameters.AddWithValue("@CartID", CartID);
            //command.Parameters.AddWithValue("@ProdutID", item.ProdutID);
            //command.Parameters.AddWithValue("@Quantity", item.Quantity);
            //command.Parameters.AddWithValue("@StatusID", item.StatusID);
            //command.CommandType = System.Data.CommandType.StoredProcedure;
            //return command.ExecuteNonQuery();
        }

        public int InsertCartProduct(CartRequestModel item, int cartID)
        {
            var parameters = new DynamicParameters(new
            {
                CartID = cartID,
                ProdutID = item.ProdutID,
                Quantity = item.Quantity,
                StatusID = item.StatusID, //status them moi
                WareHouseID = item.WarehouseID
            });

            var result = Execute("sp_InsertCartProduct", parameters, commandType: CommandType.StoredProcedure);
            return result;

            //SqlCommand command = CreateCommand("sp_InsertCartProduct");
            //command.Parameters.AddWithValue("@CartID", CartID);
            //command.Parameters.AddWithValue("@ProdutID", item.ProdutID);
            //command.Parameters.AddWithValue("@Quantity", item.Quantity);
            //command.Parameters.AddWithValue("@StatusID", item.StatusID);
            //command.CommandType = System.Data.CommandType.StoredProcedure;
            //return command.ExecuteNonQuery();
        }

        public List<CartModel> GetCartProduct(int produtID, int cartID)
        {
            var param = new DynamicParameters(new
            {
                CartID = cartID,
                ProductID = produtID
            });

            var result = Query<CartModel>("sp_GetCartProduct", param, commandType: CommandType.StoredProcedure).ToList();

            return result;
        }

        /// <summary>
        /// Get CartID By CustomerID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public int GetCartIDByCustomerID(int customerID)
        {
            var result = QueryFirstOrDefault<CartResponeModel>("sp_GetCartByIDCustomer", new { CustomerID = customerID }, commandType: CommandType.StoredProcedure);
            return result == null ? 0 : result.CartID;
        }

        //public int Remove(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public int Update(CartRequestModel item, int CartID)
        //{
        //    return UpdateCartProduct(item, CartID);
        //}

        //CartResponeModel IReadRepository<CartResponeModel, int>.Get(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public CartResponeModel Get(int customerID, int pageIndex = 1)
        {
            CartResponeModel cart = new CartResponeModel();
            //1 get cartID
            int CartID = GetCartIDByCustomerID(customerID);

            //2 get CartProduct
            var lstProduct = GetCartProduct(0, CartID);


            //while (reader.Read())
            //{
            //    var product = new CartModel();
            //    product.ProductName = reader["Name"].ToString() ?? "";
            //    product.Quantity = string.IsNullOrEmpty(reader["Quantity"].ToString()) ? 0 : Convert.ToInt32(reader["Quantity"]);
            //    product.QuantityMax = string.IsNullOrEmpty(reader["QuantityMax"].ToString()) ? 0 : Convert.ToInt32(reader["QuantityMax"]);
            //    product.Price = string.IsNullOrEmpty(reader["Price"].ToString()) ? 0 : Convert.ToDecimal(reader["Price"]);
            //    product.TotalPrice = product.Quantity * product.Price;
            //    lstProduct.Add(product);
            //}
            //cart.CartID = CartID;
            cart.lstProduct = lstProduct;

            //reader.Close();

            return cart;
        }

        //public int Remove(CartRequestModel item, int cartID)
        //{
        //    item.Quantity = 0;
        //    //1. Delete product in CartProduct
        //    var command = CreateCommand("sp_DeleteCartProduct");
        //    command.Parameters.AddWithValue("@CartID", cartID);
        //    command.Parameters.AddWithValue("@ProdutID", item.ProdutID);
        //    command.CommandType = System.Data.CommandType.StoredProcedure;
        //    if (command.ExecuteNonQuery() != 0)
        //    {
        //        //2. Check Cart con product khong
        //        SqlDataReader reader = GetCartProduct(0, cartID);
        //        bool hasRows = reader.HasRows;
        //        reader.Close();
        //        if (hasRows) //con product trong cart
        //        {
        //            return 1;
        //        }
        //        //3. Remove Cart (khong con product trong cart)
        //        else
        //        {
        //            command = CreateCommand("sp_DeleteCart");
        //            command.Parameters.AddWithValue("@CartID", cartID);
        //            command.CommandType = System.Data.CommandType.StoredProcedure;
        //            return command.ExecuteNonQuery();
        //        }
        //    }
        //    return 0;
        //}
    }
}
