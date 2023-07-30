using Common;
using Dapper;
using Models.RequestModel.Cart;
using Models.ResponseModels.Cart;
using Repository.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Implement
{
    public class CartRepository : Repository, ICartRepository
    {
        public CartRepository(SqlConnection context, SqlTransaction _transaction)
        {
            this._context = context;
            this._transaction = _transaction;
        }

        public int GetProductInStock(Guid productID, Guid wareHouseID)
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

        public Guid CreateCart(Guid customerID, Guid cartID)
        {
            var parameters = new DynamicParameters(new
            {
                CustomerID = customerID,
                Stautus = Parameters.StatusCartInsert, //New cart
                CartID = cartID
            });
            //parameters.Add("@CartID", dbType: DbType.Guid, direction: ParameterDirection.Output);
            var result = Execute("sp_InsertCart", parameters, commandType: CommandType.StoredProcedure);

            return result > 0 ? cartID : Guid.Empty;
        }

        public int UpdateCartProduct(CartRequestModel item, Guid cartID, int status)
        {
            var parameters = new DynamicParameters(new
            {
                CartID = cartID,
                ProdutID = item.ProductId,
                Quantity = item.Quantity,
                StatusID = status,
                WareHouseID = item.WarehouseId,
                PromoteID = item.PromoteId
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

        public int UpdateCart(Guid cartID, int status)
        {
            var parameters = new DynamicParameters(new
            {
                CartID = cartID,
                StatusID = status
            });

            var result = Execute("sp_UpdateCart", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public int InsertCartProduct(CartRequestModel item, Guid cartID, Guid cartProductID)
        {
            var parameters = new DynamicParameters(new
            {
                CartID = cartID,
                ProdutID = item.ProductId,
                Quantity = item.Quantity,
                StatusID = Parameters.StatusCartProductInsert, //status them moi
                WareHouseID = item.WarehouseId,
                PromoteID = item.PromoteId,
                CartProductID = cartProductID,
                colorId = item.ColorId,
                sizeId = item.SizeId
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

        public List<CartModel> GetCartProduct(Guid produtID, Guid cartID)
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
        public Guid GetCartIDByCustomerID(Guid customerID)
        {
            var result = QueryFirstOrDefault<CartResponeModel>("sp_GetCartByIDCustomer", new { CustomerID = customerID }, commandType: CommandType.StoredProcedure);
            return result == null ? Guid.Empty : result.CartId;
        }

        //public int Update(CartRequestModel item, int CartID)
        //{
        //    return UpdateCartProduct(item, CartID);
        //}

        public CartResponeModel Get(Guid customerId, int pageIndex = 1)
        {
            //chua lam case lay gia cua product
            CartResponeModel cart = new CartResponeModel();
            //1 get cartID
            Guid CartId = GetCartIDByCustomerID(customerId);
            cart.CartId = CartId;

            //2 get CartProduct
            var lstProduct = GetCartProduct(Guid.Empty, CartId); //'00000000-0000-0000-0000-000000000000'

            cart.lstProduct = lstProduct;

            //reader.Close();

            return cart;
        }

        public int Remove(CartRequestModel item, Guid cartId) //xóa mềm
        {
            //1. Update status table cart
            int updateCart = UpdateCart(cartId, Parameters.StatusDeleteCart);

            //2. Update status table cart_product
            item.ProductId = Guid.Empty;
            int updateCartProdct = UpdateCartProduct(item, cartId, Parameters.StatusDeleteCartProduct);

            if (updateCartProdct > 0 && updateCart > 0)
                return 1;

            return 0;
        }

        public int GetNumberProductsInCart(Guid cartId)
        {
            var parameters = new DynamicParameters(new
            {
                CartID = cartId,
            });
            var result = QueryFirstOrDefault<int>("sp_GetNumberProductsInCart", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        //public int Remove(CartRequestModel item, int cartID) //xóa cứng
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

        public bool ValidateIDExists(CartRequestModel model, out string err)
        {
            if (model.Quantity <= 0)
            {
                err = "Quantity phải lớn hơn không!";
                return false;
            }

            var parameters = new DynamicParameters(new
            {
                warehouseId = model.WarehouseId,
                productId = model.ProductId,
                colorId = model.ColorId,
                sizeId = model.SizeId,
                promoteId = model.PromoteId
            });
            parameters.Add("@Err", dbType: DbType.String, direction: ParameterDirection.Output);
            Execute("SP_ValidateIDExists", parameters, commandType: CommandType.StoredProcedure);
            err = parameters.Get<string>("@Err");

            return string.IsNullOrEmpty(err) ? false : true;
        }
    }
}
