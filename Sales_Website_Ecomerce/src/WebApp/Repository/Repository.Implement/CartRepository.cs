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

        private int GetProductInStock(int productID, int wareHouseID)
        {
            var result = Query<object>("SP_Get_ProductStock", new { ID = productID, WareHouseID = wareHouseID }, commandType: CommandType.StoredProcedure).ToList();
           
            return Query<object>("SP_Get_ProductStock", new { ID = productID, WareHouseID = wareHouseID }, commandType: CommandType.StoredProcedure).ToList().Count();
        }

        public int Create(CartRequestModel item)
        {
            int CartID = GetCartIDByCustomerID(item.CustomerID);
            if (CartID != 0) //Customer đã có cart
            {
                //1.Check Product đã có trong Cart_Product
                var product = GetCartProduct(item.ProdutID, CartID).FirstOrDefault();

                //1.1 Check số lượng hàng trong kho còn đủ không
                if (product.Quantity + item.Quantity > GetProductInStock(item.ProdutID, item.WarehouseID))
                    return -1; //số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)
                else
                {
                    int oldQuantity = product.Quantity;

                    if (product.WareHouseID != 0) //Product đã có cart
                    {
                        item.Quantity = oldQuantity + item.Quantity;
                        //1.2 update lại so luong, status
                        return UpdateCartProduct(item, CartID);
                    }
                    else
                    {
                        //2.Add Cart_Product
                        return InsertCartProduct(item, CartID);
                    }
                }                
            }
            else //Customer chưa có cart
            {
                var parameters = new DynamicParameters(new
                {
                    CustomerID = item.CustomerID
                });
                parameters.Add("@CartID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                Execute("sp_InsertCart", parameters, commandType: CommandType.StoredProcedure);

                int cartID = parameters.Get<int>("@CartID");
                if(cartID!= 0)
                {
                    return InsertCartProduct(item, CartID);
                }

                //SqlCommand command = CreateCommand("sp_InsertCart");
                //command.Parameters.AddWithValue("@CustomerID", item.CustomerID);
                //command.CommandType = System.Data.CommandType.StoredProcedure;

                //if (command.ExecuteNonQuery() != 0) //add cart success
                //{
                //    #region Process cart_product
                //    //1. Get CartID
                //    CartID = GetCartIDByCustomerID(item.CustomerID);

                //    //2. Add Cart_Product
                //    return InsertCartProduct(item, CartID);

                //    #endregion
                //}
            }
            return 0;//không xác định
        }

        private int UpdateCartProduct(CartRequestModel item, int cartID)
        {
            var parameters = new DynamicParameters(new
            {
                CartID =cartID,
                ProdutID = item.ProdutID,
                Quantity = item.Quantity,
                StatusID = item.StatusID,
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

        private int InsertCartProduct(CartRequestModel item, int cartID)
        {
            var parameters = new DynamicParameters(new
            {
                CartID = cartID,
                ProdutID = item.ProdutID,
                Quantity = item.Quantity,
                StatusID = item.StatusID,
                WareHouseID = item.WarehouseID
            });

            var result = Execute("sp_UpdateCartProduct", parameters, commandType: CommandType.StoredProcedure);
            return result;

            //SqlCommand command = CreateCommand("sp_InsertCartProduct");
            //command.Parameters.AddWithValue("@CartID", CartID);
            //command.Parameters.AddWithValue("@ProdutID", item.ProdutID);
            //command.Parameters.AddWithValue("@Quantity", item.Quantity);
            //command.Parameters.AddWithValue("@StatusID", item.StatusID);
            //command.CommandType = System.Data.CommandType.StoredProcedure;
            //return command.ExecuteNonQuery();
        }

        private List<CartModel> GetCartProduct(int produtID, int cartID)
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
        private int GetCartIDByCustomerID(int customerID)
        {
            var result = QueryFirstOrDefault<CartResponeModel>("sp_GetCartByIDCustomer", new { CustomerID = customerID }, commandType: CommandType.StoredProcedure);
            return result.CartID;
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
