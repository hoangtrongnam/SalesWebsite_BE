using Models.RequestModel;
using UnitOfWork.Interface;
using Common;
using Models.ResponseModels;
using Models.RequestModel.Cart;

namespace Services
{
    public interface IOrderServices
    {
        ApiResponse<int> Create(OrderRequestModel model);
        ApiResponse<int> Delete(int orderID, int customerID);
        ApiResponse<OrderResponseModel> Get(int orderID);
        ApiResponse<int> Update(OrderRequestModel item, int orderID);
        //OrderResponseModel GetlistProduct(int orderID);
    }
    public class OrderServices : IOrderServices
    {
        private IUnitOfWork _unitOfWork;
        private readonly ICartServices cartServices;

        public OrderServices(IUnitOfWork unitOfWork, ICartServices cartServices)
        {
            _unitOfWork = unitOfWork;
            this.cartServices = cartServices;
        }
        public ApiResponse<int> Create(OrderRequestModel model)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    //1. Insert table Order and OrderProduct
                    var OrderID = context.Repositories.OrderRepository.Create(model);

                    if (OrderID <= 0)
                        return ApiResponse<int>.ErrorResponse("Đặt hàng thất bại");
                    else
                    {
                        foreach (var item in model.lstProduct)
                        {
                            //2. Check SL đủ không
                            if (!cartServices.QuantityValid(item.Quantity, 0, item.ProductID, item.WareHouseID, context))
                                return ApiResponse<int>.ErrorResponse("số lượng order lớn hơn số lượng trong kho");//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)

                            //3. Delete Product from Cart
                            CartRequestModel cartModel = new CartRequestModel();
                            cartModel.ProdutID = item.ProductID;

                            var removecart = context.Repositories.CartRepository.Remove(cartModel, model.CartID);
                            if (removecart < 1)
                                return ApiResponse<int>.ErrorResponse("Xóa giỏ hàng thất bại");

                            ////3. Get Product Info
                            //var product = context.Repositories.ProductRepository.Get(item.ProductID);
                            //ProductRequestModel productRequestModel = new ProductRequestModel();
                            //productRequestModel.Quantity = product.Quantity - item.Quantity;
                            //if (productRequestModel.Quantity < 0)
                            //    throw new Exception("số lượng không đử");
                            ////4. Update Prodcut Quantity
                            //context.Repositories.ProductRepository.Update(productRequestModel, item.ProductID);
                        }
                        //4. Insert table notifications (NV)
                        NotificationRequestModel notificationRequestModel = new NotificationRequestModel();
                        notificationRequestModel.Status = Parameters.StatusNVNotify;
                        notificationRequestModel.CreateBy = model.CustomerID;
                        notificationRequestModel.Content = "Đơn hàng: " + OrderID + " cần xử lý.";
                        var notifyNV = context.Repositories.NotificationRepository.Create(notificationRequestModel);
                        if (notifyNV < 1)
                            return ApiResponse<int>.ErrorResponse("Thông báo NV thất bại");

                        //5. Insert table notifications (KH)
                        notificationRequestModel.Status = Parameters.StatusKHNotify;
                        notificationRequestModel.CreateBy = model.CustomerID;
                        notificationRequestModel.Content = "Đơn hàng: " + OrderID + " đặt thành công.";
                        var notifyKH = context.Repositories.NotificationRepository.Create(notificationRequestModel);
                        if (notifyKH < 1)
                            return ApiResponse<int>.ErrorResponse("Thông báo KH thất bại");
                        //

                        context.SaveChanges();
                        return ApiResponse<int>.SuccessResponse(1, "Dặt hàng thành công");
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public ApiResponse<int> Delete(int orderID, int customerID)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    //1. Delete Orders (chỉ cần xóa order vì xóa đơn hàng là xóa tất cả product trong order)
                    OrderRequestModel model = new OrderRequestModel();
                    model.Status = Parameters.StatusOrderDelete;
                    model.Note = "Nhân viên hủy đơn hàng";
                    var deleteOrder = context.Repositories.OrderRepository.Update(model, orderID);
                    if (deleteOrder < 1)
                        return ApiResponse<int>.ErrorResponse("Xóa đơn hàng thất bại");

                    //2. Revert Quantity Product (chờ api của Toai)
                    //foreach (var item in model.lstProduct)
                    //{
                    //    //2.1. Get Product
                    //    var product = context.Repositories.ProductRepository.Get(item.ProductID);
                    //    ProductRequestModel productRequestModel = new ProductRequestModel();
                    //    productRequestModel.Quantity = product.Quantity + item.Quantity;

                    //    //2.2. Update Product Quantity 
                    //    context.Repositories.ProductRepository.Update(productRequestModel, item.ProductID);
                    //}

                    //3.1 Insert table notifications (KH)
                    NotificationRequestModel notificationRequestModel = new NotificationRequestModel();
                    notificationRequestModel.Status = Parameters.StatusKHNotify;
                    notificationRequestModel.Content = "Đơn hàng: " + orderID + " đã bị hủy";
                    notificationRequestModel.CreateBy = customerID;
                    var notifyKH = context.Repositories.NotificationRepository.Create(notificationRequestModel);
                    if (notifyKH < 1)
                        return ApiResponse<int>.ErrorResponse("Thông báo KH thất bại");

                    //3.2. Insert table notifications (NV)
                    notificationRequestModel.Status = Parameters.StatusNVNotify;
                    notificationRequestModel.Content = "Đơn hàng: " + orderID + " đã bị hủy.";
                    notificationRequestModel.CreateBy = customerID;
                    var notifyNV = context.Repositories.NotificationRepository.Create(notificationRequestModel);
                    if (notifyNV < 1)
                        return ApiResponse<int>.ErrorResponse("Thông báo NV thất bại");

                    context.SaveChanges();
                    return ApiResponse<int>.SuccessResponse(1, "Hủy đơn hàng thành công");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ApiResponse<OrderResponseModel> Get(int orderID)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    OrderResponseModel result = context.Repositories.OrderRepository.Get(orderID);
                    if (result == null)
                        return ApiResponse<OrderResponseModel>.ErrorResponse("Tìm đơn hàng thất bại");
                    //tính lại thành tiền + số tiền cột (khi table chưa có giá trị này vì khi thanh toán rồi thì k phải tính lại)

                    //trả về list KM theo từng Product
                    foreach (var item in result.lstProduct)
                    {
                        //var lstPromote = new PriceResponseModel();
                        var lstPromote = context.Repositories.ProductRepository.GetPrices(item.ProductID);
                        item.lstPromote = lstPromote;
                    }

                    return ApiResponse<OrderResponseModel>.SuccessResponse(result, "Tìm đơn hàng thành công");
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public ApiResponse<int> Update(OrderRequestModel item, int orderID)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    //chua lam hold product trong productstock
                    //1. Update table Orders
                    var result = context.Repositories.OrderRepository.Update(item, orderID);
                    if (result < 1)
                        return ApiResponse<int>.ErrorResponse("Cập nhật đơn hàng thất bại");
                    else
                    {
                        //2. Insert table notifications
                        NotificationRequestModel notificationRequestModel = new NotificationRequestModel();

                        switch (item.Status)
                        {
                            case 11: //Sale xác nhận tiền cọc và đợn hàng thành công
                                notificationRequestModel.Status = Parameters.StatusKTNotify; //thông báo mới cho kế toán
                                notificationRequestModel.Content = "Đơn hàng: " + orderID + " cần xử lý.";
                                var notifyKT = context.Repositories.NotificationRepository.Create(notificationRequestModel);
                                if (notifyKT < 1) return ApiResponse<int>.ErrorResponse("Thông báo cho kế toán thất bại");
                                break;
                            case 12: //Sale không liên hệ được với KH (cần thông báo cho KH)
                                notificationRequestModel.Status = Parameters.StatusKHNotify; //thông báo mới cho KH
                                notificationRequestModel.Content = item.CustomerID + "!@#" + "Chúng tôi không liên hệ được với bạn để xác nhận đơn hàng: " + orderID + ". Vui lòng hệ hệ với chúng tôi trong vòng 3 ngày nếu không đơn hàng sẽ bị hủy . Cám ơn!";
                                var notifyKH = context.Repositories.NotificationRepository.Create(notificationRequestModel);
                                if (notifyKH < 1) return ApiResponse<int>.ErrorResponse("Thông báo cho KH thất bại");
                                break;
                            case 13: //số tiền cọc không đúng (kế toán)
                                notificationRequestModel.Status = Parameters.StatusNVNotify; //KT phản hồi: thông báo mới cho sale (add thông báo mới)
                                notificationRequestModel.Content = "Đơn hàng: " + orderID + " - số tiền cột không đúng";
                                var notifyNV = context.Repositories.NotificationRepository.Create(notificationRequestModel);
                                if (notifyNV < 1) return ApiResponse<int>.ErrorResponse("Thông báo cho NV thất bại");
                                break;
                            case 14: //Kế toán xác nhận đủ tiền cọc
                                //chua lam hold product trong productstock
                                notificationRequestModel.Status = Parameters.StatusKhoNotify; //thông báo cho nhân viên kho
                                notificationRequestModel.Content = "Đơn hàng: " + orderID + " cần xử lý";
                                var notifyKho = context.Repositories.NotificationRepository.Create(notificationRequestModel);
                                if (notifyKho < 1) return ApiResponse<int>.ErrorResponse("Thông báo cho NV kho thất bại");
                                break;
                        }

                        //
                        context.SaveChanges();
                        return ApiResponse<int>.SuccessResponse(1, "Cập nhật đơn hàng thành công");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public OrderResponseModel GetlistProduct(int orderID)
        //{
        //    try
        //    {
        //        using (var context = _unitOfWork.Create())
        //        {
        //            ResultModel outModel = new ResultModel();
        //            OrderResponseModel result = context.Repositories.OrderRepository.Get(orderID);

        //            return result;
        //        }
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}
    }
}