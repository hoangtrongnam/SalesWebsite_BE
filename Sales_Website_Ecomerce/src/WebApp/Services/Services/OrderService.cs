using Models.RequestModel;
using UnitOfWork.Interface;
using Common;
using Models.ResponseModels;
using Models.RequestModel.Cart;
using Models.RequestModel.Orders;
using Models.RequestModel.ProductStock;

namespace Services
{
    public interface IOrderServices
    {
        ApiResponse<int> Create(OrderRequestModel model);
        ApiResponse<int> Delete(Guid orderID, Guid customerID);
        ApiResponse<OrderResponseModel> Get(Guid orderID);
        ApiResponse<int> Update(OrderCommonRequest item, Guid orderID);
        //decimal TotalPayment(int quantity, decimal price, string PromotePrice, string PromotPercent, int isActive);
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
                    //1. Validate input
                    foreach (var item in model.lstProduct)
                    {
                        //1.2. Check SL đủ không
                        if (!cartServices.QuantityValid(item.Quantity, 0, item.ProductID, item.WareHouseID, context))
                            return ApiResponse<int>.ErrorResponse("số lượng order lớn hơn số lượng trong kho");//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)

                        CartRequestModel cartModel = new CartRequestModel();
                        cartModel.ProdutID = item.ProductID;
                        //var removecart = context.Repositories.CartRepository.Remove(cartModel, model.CartID);                      

                        //1.3. Update status table cart_product
                        int updateCartProdct = context.Repositories.CartRepository.UpdateCartProduct(cartModel, model.CartID, Parameters.StatusDeleteCartProduct);
                        if (updateCartProdct < 1)
                            return ApiResponse<int>.ErrorResponse("Xóa 1 sản phẩm thất bại. (SP cthe bị xóa)");

                        ////3. Get Product Info
                        //var product = context.Repositories.ProductRepository.Get(item.ProductID);
                        //ProductRequestModel productRequestModel = new ProductRequestModel();
                        //productRequestModel.Quantity = product.Quantity - item.Quantity;
                        //if (productRequestModel.Quantity < 0)
                        //    throw new Exception("số lượng không đử");
                        ////4. Update Prodcut Quantity
                        //context.Repositories.ProductRepository.Update(productRequestModel, item.ProductID);
                    }

                    //2. Check còn product trong giỏ hàng không
                    int numberProductsInCart = context.Repositories.CartRepository.GetNumberProductsInCart(model.CartID);
                    if (numberProductsInCart == 0)
                    {
                        //2.1. delete Cart
                        int effectRow = context.Repositories.CartRepository.UpdateCart(model.CartID, Parameters.StatusDeleteCart);
                        if (effectRow < 0)
                            return ApiResponse<int>.ErrorResponse("Xóa giỏ hàng thất bại");
                    }

                    //3. Insert table Order and OrderProduct
                    //Guid cartID = Guid.NewGuid();
                    //var OrderID = context.Repositories.OrderRepository.Create(model);

                    //3.1 Insert into table Order
                    Guid orderID = context.Repositories.OrderRepository.InsertOrder(model.CustomerID, Parameters.StatusOrderInsert, Guid.NewGuid()); //New Order (StatusOrderInsert: co dơn hàng mới cần sale xác nhận))

                    //3.2 insert into table OrderProduct
                    int insertOrderProdcut = context.Repositories.OrderRepository.InsertOrderProduct(model.lstProduct, orderID);
                    if (insertOrderProdcut < 1 || orderID == Guid.Empty)
                        return ApiResponse<int>.ErrorResponse("Đặt hàng thất bại"); //Insert into table Order and OrderProduct Success
                    else
                    {
                        //4. Insert table notifications (NV)
                        NotificationRequestModel notificationRequestModel = new NotificationRequestModel();
                        notificationRequestModel.Status = Parameters.StatusNVNotify;
                        notificationRequestModel.CreateBy = model.CustomerID;
                        notificationRequestModel.Content = "Đơn hàng: " + orderID + " cần xử lý.";
                        var notifyNV = context.Repositories.NotificationRepository.Create(notificationRequestModel, Guid.NewGuid());
                        if (notifyNV < 1)
                            return ApiResponse<int>.ErrorResponse("Thông báo NV thất bại");

                        //5. Insert table notifications (KH)
                        notificationRequestModel.Status = Parameters.StatusKHNotify;
                        notificationRequestModel.CreateBy = model.CustomerID;
                        notificationRequestModel.Content = "Đơn hàng: " + orderID + " đặt thành công.";
                        var notifyKH = context.Repositories.NotificationRepository.Create(notificationRequestModel, Guid.NewGuid());
                        if (notifyKH < 1)
                            return ApiResponse<int>.ErrorResponse("Thông báo KH thất bại");

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

        public ApiResponse<int> Delete(Guid orderID, Guid customerID)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    //1. Delete Orders (chỉ cần xóa order vì xóa đơn hàng là xóa tất cả product trong order)
                    OrderCommonRequest model = new OrderCommonRequest();
                    model.Status = Parameters.StatusOrderDelete;
                    model.Note = "Nhân viên hủy đơn hàng";
                    var deleteOrder = context.Repositories.OrderRepository.Update(model, orderID, 0);
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
                    var notifyKH = context.Repositories.NotificationRepository.Create(notificationRequestModel, Guid.NewGuid());
                    if (notifyKH < 1)
                        return ApiResponse<int>.ErrorResponse("Thông báo KH thất bại");

                    //3.2. Insert table notifications (NV)
                    notificationRequestModel.Status = Parameters.StatusNVNotify;
                    notificationRequestModel.Content = "Đơn hàng: " + orderID + " đã bị hủy.";
                    notificationRequestModel.CreateBy = customerID;
                    var notifyNV = context.Repositories.NotificationRepository.Create(notificationRequestModel, Guid.NewGuid());
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

        public ApiResponse<OrderResponseModel> Get(Guid orderID)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    decimal totalPrice = 0; //giá tiền 1 sp
                    DateTime currentTime = DateTime.Now;
                    OrderResponseModel result = context.Repositories.OrderRepository.Get(orderID);
                    if (result == null)
                        return ApiResponse<OrderResponseModel>.ErrorResponse("Tìm đơn hàng thất bại");
                    //tính lại thành tiền + số tiền cột (khi table chưa có giá trị này vì khi thanh toán rồi thì k phải tính lại)

                    //trả về list KM theo từng Product
                    foreach (var item in result.lstProduct)
                    {
                        //get list promote
                        var lstPromote = context.Repositories.ProductRepository.GetPrices(item.ProductID);
                        item.lstPromote = lstPromote;

                        //get 1 promote (expire and PromoteID)
                        var promote = context.Repositories.OrderRepository.GetPromote(item.PromoteID);

                        if (result.TotalPayment == 0)
                        {
                            //get promote
                            //PriceResponseModel? promote = lstPromote.Count > 0 ? lstPromote.Where(s => s.PriceID == item.PromoteID).FirstOrDefault() : null;

                            if (promote != null) // có KM
                                totalPrice += TotalPrice(item.Quantity, item.Price, promote.PromotePrice, promote.PromotPercent, promote.IsActive);
                            else totalPrice += item.Quantity * item.Price; //không có KM
                        }
                    }

                    if (result.TotalPayment == 0) result.TotalPayment = totalPrice;

                    return ApiResponse<OrderResponseModel>.SuccessResponse(result, "Tìm đơn hàng thành công");
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public ApiResponse<int> Update(OrderCommonRequest item, Guid orderID)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    //chua lam hold product trong productstock
                    //1. Update table Orders
                    decimal totalPayment = 0;

                    OrderResponseModel orderResponseModel = context.Repositories.OrderRepository.Get(orderID);
                    if (orderResponseModel == null)
                        return ApiResponse<int>.ErrorResponse("Tìm đơn hàng thất bại");

                    if (item.Status == 11) // tính totalPayment để cho sale tính tiền cọc và lưu xuống db
                    {
                        decimal totalPrice = 0; //giá tiền 1 sp
                        DateTime currentTime = DateTime.Now;

                        //trả về list KM theo từng Product
                        foreach (var product in orderResponseModel.lstProduct)
                        {
                            //KT SL trong kho đủ không
                            if (!cartServices.QuantityValid(product.Quantity, 0, product.ProductID, product.WarehouseID, context))
                                return ApiResponse<int>.ErrorResponse("số lượng order lớn hơn số lượng trong kho");//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)
                            
                            //get 1 promote (expire and PromoteID)
                            var promote = context.Repositories.OrderRepository.GetPromote(product.PromoteID);

                            if (orderResponseModel.TotalPayment == 0)
                            {
                                if (promote != null) // có KM
                                    totalPrice += TotalPrice(product.Quantity, product.Price, promote.PromotePrice, promote.PromotPercent, promote.IsActive);
                                else totalPrice += product.Quantity * product.Price; //không có KM
                            }
                        }

                        if (orderResponseModel.TotalPayment == 0) totalPayment = totalPrice;
                    }


                    var result = item.Status == 11 ? context.Repositories.OrderRepository.Update(item, orderID, totalPayment) : context.Repositories.OrderRepository.Update(item, orderID, 0);
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
                                var notifyKT = context.Repositories.NotificationRepository.Create(notificationRequestModel, Guid.NewGuid());
                                if (notifyKT < 1) return ApiResponse<int>.ErrorResponse("Thông báo cho kế toán thất bại");
                                break;
                            case 12: //Sale không liên hệ được với KH (cần thông báo cho KH)
                                notificationRequestModel.Status = Parameters.StatusKHNotify; //thông báo mới cho KH
                                notificationRequestModel.Content = item.CustomerID + "!@#" + "Chúng tôi không liên hệ được với bạn để xác nhận đơn hàng: " + orderID + ". Vui lòng hệ hệ với chúng tôi trong vòng 3 ngày nếu không đơn hàng sẽ bị hủy . Cám ơn!";
                                var notifyKH = context.Repositories.NotificationRepository.Create(notificationRequestModel, Guid.NewGuid());
                                if (notifyKH < 1) return ApiResponse<int>.ErrorResponse("Thông báo cho KH thất bại");
                                break;
                            case 13: //số tiền cọc không đúng (kế toán)
                                notificationRequestModel.Status = Parameters.StatusNVNotify; //KT phản hồi: thông báo mới cho sale (add thông báo mới)
                                notificationRequestModel.Content = "Đơn hàng: " + orderID + " - số tiền cột không đúng";
                                var notifyNV = context.Repositories.NotificationRepository.Create(notificationRequestModel, Guid.NewGuid());
                                if (notifyNV < 1) return ApiResponse<int>.ErrorResponse("Thông báo cho NV thất bại");
                                break;
                            case 14: //Kế toán xác nhận đủ tiền cọc
                                #region Hold product trong productstock
                                foreach (var product in orderResponseModel.lstProduct)
                                {
                                    //KT SL trong kho đủ không
                                    if (!cartServices.QuantityValid(product.Quantity, 0, product.ProductID, product.WarehouseID, context))
                                        return ApiResponse<int>.ErrorResponse("số lượng order lớn hơn số lượng trong kho");//số lượng order lớn hơn số lượng trong kho (validate luôn input đầu vào)

                                    HoldProductRequestModel model = new HoldProductRequestModel();
                                    model.OrderID = orderID;
                                    model.ProductID = product.ProductID;
                                    model.HoldNumber = product.Quantity;
                                    model.ExfactoryPrice = product.ExfactoryPrice;
                                    model.WareHouseID = product.WarehouseID;
                                    int holdProduct = context.Repositories.ProductStockRepository.HoldProduct(model);
                                    if (holdProduct < 1) return ApiResponse<int>.ErrorResponse("Giữ hàng cho KH thất bại");
                                }
                                #endregion

                                notificationRequestModel.Status = Parameters.StatusKhoNotify; //thông báo cho nhân viên kho
                                notificationRequestModel.Content = "Đơn hàng: " + orderID + " cần xử lý";
                                var notifyKho = context.Repositories.NotificationRepository.Create(notificationRequestModel, Guid.NewGuid());
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

        public decimal TotalPrice(int quantity, decimal price, string promotePrice, string promotPercent, int isActive)
        {
            //0: Lấy % --- 1: Lấy giá giảm
            return isActive == 1 ? quantity * price - decimal.Parse(promotePrice) : quantity * price * (1 - (decimal.Parse(promotPercent) / 100));
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