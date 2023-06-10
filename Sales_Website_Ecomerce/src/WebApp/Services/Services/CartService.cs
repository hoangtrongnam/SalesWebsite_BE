using UnitOfWork.Interface;
using Common;
using Models.ResponseModels.Cart;
using Models.RequestModel.Cart;

namespace Services
{
    public interface ICartServices
    {
        //ResultModel GetAll(int pageIndex);
        ApiResponse<CartResponeModel> Get(int customerID, int pageIndex);

        ApiResponse<int> Create(CartRequestModel model);
        //ResultModel Update(CartRequestModel model, int cartID);
        ApiResponse<int> Delete(int cartID);
    }
    public class CartServices : ICartServices
    {
        private IUnitOfWork _unitOfWork;

        public CartServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ApiResponse<int> Create(CartRequestModel item)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    int efectRow = 0;
                    int CartID = context.Repositories.CartRepository.GetCartIDByCustomerID(item.CustomerID);

                    //1. Check KH đã có giỏ hàng chưa
                    if (CartID > 0) //1.1. Customer đã có cart
                    {
                        //var productQuantityInStock = context.Repositories.CartRepository.GetProductInStock(item.ProdutID, item.WarehouseID);
                        var product = context.Repositories.CartRepository.GetCartProduct(item.ProdutID, CartID).FirstOrDefault();

                        if (product == null) //1.1.1. Product chưa trong Cart_Product
                        {
                            //1.1.1.1 kiểm tra sl trong kho còn đủ không
                            if (!QuantityValid(item.Quantity, 0, item.ProdutID, item.WarehouseID, context))
                                return ApiResponse<int>.ErrorResponse("số lượng order lớn hơn số lượng trong kho");//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)

                            //1.1.1.2.Add Cart_Product
                            efectRow = context.Repositories.CartRepository.InsertCartProduct(item, CartID);

                            if (efectRow < 1)
                                return ApiResponse<int>.ErrorResponse("cập nhật hàng thất bại");

                            context.SaveChanges();

                            return ApiResponse<int>.SuccessResponse(efectRow, "cập nhật giỏ hang thành công");
                        }
                        else //1.1.2. Product đã có trong Cart_Product
                        {
                            //note case khac kho chua lam
                            //1.1.2.1. Check số lượng hàng trong kho còn đủ không
                            int oldQuantity = product.Quantity;
                            if (!QuantityValid(item.Quantity, oldQuantity, item.ProdutID, product.WareHouseID, context))
                                return ApiResponse<int>.ErrorResponse("số lượng order lớn hơn số lượng trong kho");//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)

                            //1.1.2.2. Update giỏ hàng (UpdateCartProduct) so luong, status
                            item.Quantity = oldQuantity + item.Quantity;
                            efectRow = context.Repositories.CartRepository.UpdateCartProduct(item, CartID, Parameters.StatusQuantityCartProductUpdateSuccess);

                            if (efectRow < 1)
                                return ApiResponse<int>.ErrorResponse("Sửa giỏ hàng thất bại");

                            context.SaveChanges();

                            return ApiResponse<int>.SuccessResponse(efectRow, "Cập nhật giỏ hàng thành công");
                        }
                    }
                    else //2. Customer chưa có cart
                    {
                        //2.1. check số lượng trong kho còn đủ không
                        if (!QuantityValid(item.Quantity, 0, item.ProdutID, item.WarehouseID, context))
                            return ApiResponse<int>.ErrorResponse("số lượng order lớn hơn số lượng trong kho");//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)

                        //2.2. tạo cart
                        int cartID = context.Repositories.CartRepository.CreateCart(item.CustomerID);
                        if (cartID > 1)
                        {
                            //2.2.1. tạo CartProduct
                            efectRow = context.Repositories.CartRepository.InsertCartProduct(item, cartID);

                            if (efectRow < 1)
                                return ApiResponse<int>.ErrorResponse("Thêm giỏ hàng thất bại");

                            context.SaveChanges();

                            return ApiResponse<int>.SuccessResponse(efectRow, "Thêm giỏ hàng thành công");
                        }
                        else return ApiResponse<int>.ErrorResponse("Thêm giỏ hàng thất bại");
                    }

                    //return ApiResponse<int>.ErrorResponse("Thêm giỏ hàng thất bại.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool QuantityValid(int newQuantity, int oldQuantity, int productID, int wareHouseID, IUnitOfWorkAdapter context)
        {
            var productQuantityInStock = context.Repositories.CartRepository.GetProductInStock(productID, wareHouseID);
            if (oldQuantity + newQuantity < productQuantityInStock)
                return true;//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)

            return false;
        }

        public ApiResponse<int> Delete(int cartID)
        {
            //throw new NotImplementedException();
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    CartRequestModel model = new CartRequestModel();
                    var result = context.Repositories.CartRepository.Remove(model, cartID);
                    if (result > 0)
                    {
                        context.SaveChanges();
                        return ApiResponse<int>.SuccessResponse(result, "Xóa thành công");
                    }
                    else return ApiResponse<int>.ErrorResponse("Xóa thất bại");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ApiResponse<CartResponeModel> Get(int customerID, int pageIndex)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.CartRepository.Get(customerID, pageIndex);
                    if (result.CartID == 0)
                    {
                        return ApiResponse<CartResponeModel>.SuccessResponse(result);
                    }
                    else
                    {
                        return ApiResponse<CartResponeModel>.ErrorResponse();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        ////public ResultModel GetAll(int pageIndex)
        ////{
        ////    try
        ////    {
        ////        ResultModel outModel = new ResultModel();
        ////        using (var context = _unitOfWork.Create())
        ////        {
        ////            var result = context.Repositories.ProductRepository.GetAll(pageIndex);
        ////            if (result.Count == 0)
        ////            {
        ////                outModel.Message = "Tìm tất cả sản phấm thất bại";
        ////                outModel.StatusCode = "999";
        ////            }
        ////            else
        ////            {
        ////                outModel.Message = "Tìm tất cả sản phấm thành công";
        ////                outModel.StatusCode = "200";
        ////                outModel.DATA = result;
        ////            }
        ////        }
        ////        return outModel;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new Exception(ex.Message);
        ////    }
        ////}

        //public ResultModel Update(CartRequestModel item, int cartID)
        //{
        //    try
        //    {
        //        ResultModel res = new ResultModel();
        //        using (var context = _unitOfWork.Create())
        //        {
        //            var result = context.Repositories.CartRepository.Update(item, cartID);
        //            if (result == 0)
        //            {
        //                res.Message = "Sửa giỏ hàng thất bại";
        //                res.StatusCode = "999";
        //            }
        //            else
        //            {
        //                context.SaveChanges();
        //                res.Message = "Sửa giỏ hàng thành công";
        //                res.StatusCode = "200";
        //            }
        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
