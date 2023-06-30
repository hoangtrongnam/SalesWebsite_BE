using UnitOfWork.Interface;
using Common;
using Models.ResponseModels.Cart;
using Models.RequestModel.Cart;
using Models.ResponseModels.Product;
using static Models.ResponseModels.OrderResponseModel;

namespace Services
{
    public interface ICartServices
    {
        ApiResponse<CartResponeModel> Get(Guid customerID, int pageIndex);
        ApiResponse<int> Create(CartRequestModel model);
        ApiResponse<int> Update(CartRequestModel model, Guid cartID);
        ApiResponse<int> Delete(Guid cartID);
        ApiResponse<int> AddProductExisted(CartRequestModel item);
        bool QuantityValid(int newQuantity, int oldQuantity, Guid productID, Guid wareHouseID, IUnitOfWorkAdapter context);
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
                    Guid cartID = context.Repositories.CartRepository.GetCartIDByCustomerID(item.CustomerID);

                    //1. Check KH đã có giỏ hàng chưa
                    if (cartID != Guid.Empty) //1.1. Customer đã có cart
                    {
                        var product = context.Repositories.CartRepository.GetCartProduct(item.ProdutID, cartID).FirstOrDefault();

                        if (product == null) //1.1.1. Product chưa trong Cart_Product
                        {
                            //1.1.1.1 kiểm tra sl trong kho còn đủ không
                            if (!QuantityValid(item.Quantity, 0, item.ProdutID, item.WarehouseID, context))
                                return ApiResponse<int>.ErrorResponse("số lượng order lớn hơn số lượng trong kho");//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)

                            //1.1.1.2.Add Cart_Product
                            efectRow = context.Repositories.CartRepository.InsertCartProduct(item, cartID, Guid.NewGuid());

                            if (efectRow < 1)
                                return ApiResponse<int>.ErrorResponse("thêm sản phẩm thất bại");

                            context.SaveChanges();

                            return ApiResponse<int>.SuccessResponse(efectRow, "thêm sản phẩm thành công");
                        }
                        else //1.1.2. Product đã có trong Cart_Product
                        {
                            //note case khac kho chua lam
                            //1.1.2.1. Check số lượng hàng trong kho (lay warehouse user nhap) còn đủ không
                            if (!QuantityValid(item.Quantity, 0, item.ProdutID, item.WarehouseID, context))
                                return ApiResponse<int>.ErrorResponse("số lượng order lớn hơn số lượng trong kho");//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)

                            ////1.1.2.2. Update giỏ hàng (UpdateCartProduct) so luong, status
                            //item.Quantity = oldQuantity + item.Quantity;
                            //efectRow = context.Repositories.CartRepository.UpdateCartProduct(item, CartID, Parameters.StatusQuantityCartProductUpdate);

                            //if (efectRow < 1)
                            //    return ApiResponse<int>.ErrorResponse("Sửa giỏ hàng thất bại");

                            //context.SaveChanges();

                            return ApiResponse<int>.SuccessResponse(0, "Đã có sản phẩm: " + product.Name + " với số lượng: " + product.Quantity +
                                " trong giỏ hàng. Khách hàng có muốn hủy trước khi thêm mới không?"); // nếu đồng ý call API AddProductExisted
                        }
                    }
                    else //2. Customer chưa có cart
                    {
                        //2.1. check số lượng trong kho còn đủ không
                        if (!QuantityValid(item.Quantity, 0, item.ProdutID, item.WarehouseID, context))
                            return ApiResponse<int>.ErrorResponse("số lượng order lớn hơn số lượng trong kho");//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)

                        //2.2. tạo cart
                        Guid newCartID = context.Repositories.CartRepository.CreateCart(item.CustomerID, Guid.NewGuid());
                        if (newCartID != Guid.Empty)
                        {
                            //2.2.1. tạo CartProduct
                            efectRow = context.Repositories.CartRepository.InsertCartProduct(item, newCartID, Guid.NewGuid());

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

        public ApiResponse<int> AddProductExisted(CartRequestModel item)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    int efectRow = 0;
                    Guid cartID = context.Repositories.CartRepository.GetCartIDByCustomerID(item.CustomerID);

                    //1. Check KH đã có giỏ hàng chưa
                    if (cartID != Guid.Empty) //1.1. Customer đã có cart
                    {
                        var product = context.Repositories.CartRepository.GetCartProduct(item.ProdutID, cartID).FirstOrDefault();

                        if (product != null) //1.1.1. Product chưa trong Cart_Product
                        {
                            //1.1.1.1 kiểm tra sl trong kho (kho user nhập) còn đủ không
                            if (!QuantityValid(item.Quantity, 0, item.ProdutID, item.WarehouseID, context))
                                return ApiResponse<int>.ErrorResponse("số lượng order lớn hơn số lượng trong kho");//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)

                            //1.1.1.2. Remove Product trong cartProduct không cần phải remove lun cart vì nó sẽ add lại vào cartproduct
                            int updateCartProdct = context.Repositories.CartRepository.UpdateCartProduct(item, cartID, Parameters.StatusDeleteCartProduct);
                            if (updateCartProdct < 1)
                                return ApiResponse<int>.ErrorResponse("Xóa sản phẩm: " + product.Name + " trong giỏ hàng thất bại");

                            //1.1.1.3.Add product into Cart_Product
                            efectRow = context.Repositories.CartRepository.InsertCartProduct(item, cartID, Guid.NewGuid());

                            if (efectRow < 1)
                                return ApiResponse<int>.ErrorResponse("thêm sản phẩm vào giỏ hàng thất bại");

                            context.SaveChanges();

                            return ApiResponse<int>.SuccessResponse(efectRow, "thêm sản phẩm vào giỏ hàng thành công");

                        }
                        else return ApiResponse<int>.ErrorResponse("sai flow 1.1.1");
                    }
                    return ApiResponse<int>.ErrorResponse("sai flow 1.1");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool QuantityValid(int newQuantity, int oldQuantity, Guid productID, Guid wareHouseID, IUnitOfWorkAdapter context)
        {
            var productQuantityInStock = context.Repositories.CartRepository.GetProductInStock(productID, wareHouseID);
            if (oldQuantity + newQuantity <= productQuantityInStock)
                return true;//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)

            return false;
        }

        public ApiResponse<int> Delete(Guid cartID)
        {
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

        public ApiResponse<CartResponeModel> Get(Guid customerID, int pageIndex)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.CartRepository.Get(customerID, pageIndex);
                    if (result.CartID != Guid.Empty)
                    {
                        decimal totalPayment = 0;

                        //1. trả về list KM theo từng Product
                        foreach (var item in result.lstProduct)
                        {
                            //var lstPromote = new PriceResponseModel();
                            //1.2 tính tiền 1 sp (price*quantity vs KM)
                            decimal totalPrice = 0;
                            var lstPromote = context.Repositories.ProductRepository.GetPrices(item.ProductID);
                            item.lstPromote = lstPromote;
                            

                            if(lstPromote.Select(p => p.PriceID).Contains(item.PromoteID))
                            {
                                //1.2.1 case KM %

                                //1.2.2 case KM tiền
                            }else
                            {
                                totalPrice = item.Price * item.Quantity;
                                item.TotalPrice = totalPrice;
                            }

                            totalPayment += totalPrice;
                        }
                        //2. tính tổng số tiền cần thanh toán
                        result.TotalPayment = totalPayment;

                        return ApiResponse<CartResponeModel>.SuccessResponse(result);
                    }
                    else
                    {
                        return ApiResponse<CartResponeModel>.ErrorResponse("CustomerID: " + customerID + " chưa có giỏ hàng");
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

        public ApiResponse<int> Update(CartRequestModel item, Guid cartID)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    //note chưa làm case khác Warehouse

                    //get thông tin cart_product them ProductID và cartID
                    var product = context.Repositories.CartRepository.GetCartProduct(item.ProdutID, cartID).FirstOrDefault();
                    if (product == null)
                        return ApiResponse<int>.ErrorResponse("Cập nhật giỏ hàng thất bại. Không tìm thấy sản phẩm cần sửa");

                    //1. Kiểm tra là xóa 1 product hay chỉ update Quantity
                    if (item.Quantity == 0)
                    {
                        //1.1. Xóa 1 Product trong giỏ háng
                        item.WarehouseID = product.WareHouseID;
                        item.Quantity = 0;
                        int efectRow = context.Repositories.CartRepository.UpdateCartProduct(item, cartID, Parameters.StatusDeleteCartProduct);

                        if (efectRow < 1)
                            return ApiResponse<int>.ErrorResponse("Xóa 1 sản phẩm trong giỏ hàng thất bại");

                        context.SaveChanges();

                        return ApiResponse<int>.SuccessResponse(efectRow, "Xóa 1 sản phẩm trong giỏ hàng thành công");
                    }
                    else if (item.Quantity > 0) //1.2 Update số lượng của 1 product trong giỏ hàng
                    {
                        //1.2.1. Check số lượng hàng trong kho còn đủ không
                        if (!QuantityValid(item.Quantity, 0, item.ProdutID, product.WareHouseID, context))
                            return ApiResponse<int>.ErrorResponse("số lượng order lớn hơn số lượng trong kho");//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)

                        //1.2.2 Update giỏ hàng (UpdateCartProduct) so luong, status
                        item.WarehouseID = product.WareHouseID;
                        int efectRow = context.Repositories.CartRepository.UpdateCartProduct(item, cartID, Parameters.StatusQuantityCartProductUpdate);

                        if (efectRow < 1)
                            return ApiResponse<int>.ErrorResponse("Cập nhật số lượng 1 product trong giỏ hàng thất bại");

                        context.SaveChanges();

                        return ApiResponse<int>.SuccessResponse(efectRow, "Cập nhật số lượng 1 product trong giỏ hàng thành công");
                    }
                    return ApiResponse<int>.ErrorResponse("Cập nhật giỏ hàng thất bại");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
