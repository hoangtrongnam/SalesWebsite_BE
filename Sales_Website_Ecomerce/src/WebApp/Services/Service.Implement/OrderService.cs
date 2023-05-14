using Models.RequestModel;
using UnitOfWork.Interface;
using Common;
using Service.Interface;

namespace Services
{
    public class OrderServices : IServices<OrderRequestModel, int>
    {
        private IUnitOfWork _unitOfWork;
        public OrderServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResultModel Create(OrderRequestModel model)
        {
            try
            {
                //throw new NotImplementedException();
                ResultModel outModel = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    //1. Insert table Order and OrderProduct
                    var OrderID = context.Repositories.OrderRepository.Create(model);
                    if (OrderID <= 0)
                    {
                        context.DeleteChanges();
                        outModel.Message = "Thêm thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        foreach (var item in model.lstProduct)
                        {
                            //2. Delete Product from Cart
                            context.Repositories.CartRepository.Remove(item.ProductID, model.CartID);

                            //3. Get Product Info
                            var product = context.Repositories.ProductRepository.Get(item.ProductID);
                            ProductRequestModel productRequestModel = new ProductRequestModel();
                            productRequestModel.Quantity = product.Quantity - item.Quantity;
                            if (productRequestModel.Quantity < 0)
                                throw new Exception("số lượng không đử");
                            //4. Update Prodcut Quantity
                            context.Repositories.ProductRepository.Update(productRequestModel, item.ProductID);
                        }
                        //5. Insert table notifications
                        NotificationRequestModel notificationRequestModel = new NotificationRequestModel();
                        notificationRequestModel.Status = 20;
                        notificationRequestModel.Content = "Đơn hàng: " + OrderID + " cần xử lý.";
                        context.Repositories.NotificationRepository.Create(notificationRequestModel);
                        //

                        context.SaveChanges();
                        outModel.Message = "Thêm thành công";
                        outModel.StatusCode = "200";
                    }
                }
                return outModel;
            }
            catch (Exception ex)
            {
               
                throw new Exception(ex.Message);
            }
        }

        public ResultModel Delete(OrderRequestModel model, int ID)
        {
            throw new NotImplementedException();
        }

        public ResultModel Delete(int ID)
        {
            throw new NotImplementedException();
        }

        public ResultModel Get(int ID)
        {
            throw new NotImplementedException();
        }

        public ResultModel Get(int ID, int pageIndex)
        {
            throw new NotImplementedException();
        }

        public ResultModel GetAll(int pageIndex)
        {
            throw new NotImplementedException();
        }

        public ResultModel GetAll()
        {
            throw new NotImplementedException();
        }

        public ResultModel Update(OrderRequestModel model, int ID)
        {
            throw new NotImplementedException();
        }
    }
}
