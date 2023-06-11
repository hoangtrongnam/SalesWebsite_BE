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
        //ApiResponse<int> Delete(OrderRequestModel model, int OrderID);
        //ApiResponse<int> Get(int orderID);
        //ApiResponse<int> Update(OrderRequestModel item, int orderID);
        //OrderResponseModel GetlistProduct(int orderID);
    }
    public class OrderServices : IOrderServices
    {
        private IUnitOfWork _unitOfWork;
        public OrderServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                            //2. Delete Product from Cart
                            CartRequestModel cartModel = new CartRequestModel();
                            cartModel.ProdutID = item.ProductID;

                            context.Repositories.CartRepository.Remove(cartModel, model.CartID);

                            ////3. Get Product Info
                            //var product = context.Repositories.ProductRepository.Get(item.ProductID);
                            //ProductRequestModel productRequestModel = new ProductRequestModel();
                            //productRequestModel.Quantity = product.Quantity - item.Quantity;
                            //if (productRequestModel.Quantity < 0)
                            //    throw new Exception("số lượng không đử");
                            ////4. Update Prodcut Quantity
                            //context.Repositories.ProductRepository.Update(productRequestModel, item.ProductID);
                        }
                        //3. Insert table notifications (NV)
                        NotificationRequestModel notificationRequestModel = new NotificationRequestModel();
                        notificationRequestModel.Status = Parameters.StatusNVNotify;
                        notificationRequestModel.CreateBy = model.CustomerID;
                        notificationRequestModel.Content = "Đơn hàng: " + OrderID + " cần xử lý.";
                        context.Repositories.NotificationRepository.Create(notificationRequestModel);

                        //4. Insert table notifications (KH)
                        notificationRequestModel.Status = Parameters.StatusKHNotify;
                        notificationRequestModel.CreateBy = model.CustomerID;
                        notificationRequestModel.Content = "Đơn hàng: " + OrderID + " đặt thành công.";
                        context.Repositories.NotificationRepository.Create(notificationRequestModel);
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

        //public ApiResponse<int> Delete(OrderRequestModel model, int OrderID)
        //{
        //    try
        //    {
        //        ResultModel outModel = new ResultModel();
        //        using (var context = _unitOfWork.Create())
        //        {
        //            //1. Delete Orders and OrderProducts
        //            var step1 = context.Repositories.OrderRepository.Remove(OrderID);
        //            //2. Revert Quantity Product
        //            foreach (var item in model.lstProduct)
        //            {
        //                //2.1. Get Product
        //                var product = context.Repositories.ProductRepository.Get(item.ProductID);
        //                ProductRequestModel productRequestModel = new ProductRequestModel();
        //                productRequestModel.Quantity = product.Quantity + item.Quantity;

        //                //2.2. Update Product Quantity
        //                context.Repositories.ProductRepository.Update(productRequestModel, item.ProductID);
        //            }

        //            //3.1 Insert table notifications (KH)
        //            NotificationRequestModel notificationRequestModel = new NotificationRequestModel();
        //            notificationRequestModel.Status = 24;
        //            notificationRequestModel.Content = "Đơn hàng: " + OrderID + " đã bị hủy";
        //            var step3 = context.Repositories.NotificationRepository.Create(notificationRequestModel);

        //            //3.2. Insert table notifications (NV)
        //            notificationRequestModel.Status = 20;
        //            notificationRequestModel.Content = "Đơn hàng: " + OrderID + " đã bị hủy.";
        //            context.Repositories.NotificationRepository.Create(notificationRequestModel);

        //            if (step1 <= 0 || step3 <= 0)
        //            {
        //                outModel.Message = "Xóa thất bại";
        //                outModel.StatusCode = "999";
        //            }
        //            else
        //            {
        //                context.SaveChanges();
        //                outModel.Message = "Xóa thành công";
        //                outModel.StatusCode = "200";
        //            }
        //        }
        //        return outModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public ApiResponse<int> Get(int orderID)
        //{
        //    try
        //    {
        //        using (var context = _unitOfWork.Create())
        //        {
        //            ResultModel outModel = new ResultModel();
        //            OrderResponseModel result = context.Repositories.OrderRepository.Get(orderID);
        //            if (string.IsNullOrEmpty(result.TotalPayment.ToString()))
        //            {
        //                outModel.Message = "tìm sản phẩm thất bại";
        //                outModel.StatusCode = "999";
        //                outModel.DATA = result;
        //            }
        //            else
        //            {
        //                outModel.Message = "tìm sản phẩm thành công";
        //                outModel.StatusCode = "200";
        //                outModel.DATA = result;
        //            }
        //            return outModel;
        //        }
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}
        //public ApiResponse<int> Update(OrderRequestModel item, int orderID)
        //{
        //    try
        //    {
        //        ResultModel outModel = new ResultModel();
        //        using (var context = _unitOfWork.Create())
        //        {
        //            //1. Update table Orders
        //            var result = context.Repositories.OrderRepository.Update(item, orderID);
        //            if (result <= 0)
        //            {
        //                context.DeleteChanges();
        //                outModel.Message = "Sửa thất bại";
        //                outModel.StatusCode = "999";
        //            }
        //            else
        //            {
        //                //5. Insert table notifications
        //                NotificationRequestModel notificationRequestModel = new NotificationRequestModel();

        //                switch (item.Status)
        //                {
        //                    case 11: //Sale xác nhận tiền cọc và đợn hàng thành công
        //                        notificationRequestModel.Status = 22; //thông báo mới cho kế toán
        //                        notificationRequestModel.Content = "Đơn hàng: " + orderID + " cần xử lý.";
        //                        context.Repositories.NotificationRepository.Create(notificationRequestModel);
        //                        break;
        //                    case 12: //Sale không liên hệ được với KH (cần thông báo cho KH)
        //                        notificationRequestModel.Status = 26; //thông báo mới cho KH
        //                        notificationRequestModel.Content = item.CustomerID + "!@#" + "Chúng tôi không liên hệ được với bạn để xác nhận đơn hàng: " + orderID + ". Vui lòng hệ hệ với chúng tôi trong vòng 3 ngày nếu không đơn hàng sẽ bị hủy . Cám ơn!";
        //                        context.Repositories.NotificationRepository.Create(notificationRequestModel);
        //                        break;
        //                    case 13: //số tiền cọc không đúng (kế toán)
        //                        notificationRequestModel.Status = 20; //KT phản hồi: thông báo mới cho sale (add thông báo mới)
        //                        notificationRequestModel.Content = "Đơn hàng: " + orderID + " - số tiền cột không đúng";
        //                        context.Repositories.NotificationRepository.Create(notificationRequestModel);
        //                        break;
        //                    case 14: //Kế toán xác nhận đủ tiền cọc
        //                        notificationRequestModel.Status = 24; //thông báo cho nhân viên kho
        //                        notificationRequestModel.Content = "Đơn hàng: " + orderID + " cần xử lý";
        //                        context.Repositories.NotificationRepository.Create(notificationRequestModel);
        //                        break;
        //                }

        //                //
        //                context.SaveChanges();
        //                outModel.Message = "Sửa thành công";
        //                outModel.StatusCode = "200";
        //            }
        //        }
        //        return outModel;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }
        //}

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