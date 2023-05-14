using Models.RequestModel;
using UnitOfWork.Interface;
using Common;
using Service.Interface;

namespace Services
{
    public class NotificationServices : IServices<NotificationRequestModel, int>
    {
        private IUnitOfWork _unitOfWork;
        public NotificationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResultModel Create(NotificationRequestModel item)
        {
            try
            {
                ResultModel outModel = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    //1. Insert table Order and OrderProduct
                    var OrderID = context.Repositories.NotificationRepository.Create(item);
                    if (OrderID <= 0)
                    {
                        outModel.Message = "Thêm thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        outModel.Message = "Thêm thành công";
                        outModel.StatusCode = "200";
                    }
                    context.SaveChanges();
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

        public ResultModel Delete(NotificationRequestModel model, int ID)
        {
            throw new NotImplementedException();
        }

        public ResultModel Get(int ID)
        {
            throw new NotImplementedException();
        }

        public ResultModel Get(int role, int pageIndex)
        {
            try
            {
                ResultModel outModel = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.NotificationRepository.Get(role, pageIndex);
                    if (result.lstNotification.Count <= 0)
                    {
                        outModel.Message = "Tìm tất thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        outModel.Message = "Tìm tất thành công";
                        outModel.StatusCode = "200";
                        outModel.DATA = result;
                    }
                }
                return outModel;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public ResultModel GetAll(int pageIndex)
        {
            throw new NotImplementedException();
        }

        public ResultModel GetAll()
        {
            throw new NotImplementedException();
        }

        public ResultModel Update(NotificationRequestModel model, int notificationID)
        {
            try
            {
                ResultModel res = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.NotificationRepository.Update(model, notificationID);
                    if (result <= 0)
                    {
                        context.DeleteChanges();
                        res.Message = "Sửa thất bại";
                        res.StatusCode = "999";
                    }
                    else
                    {
                        context.SaveChanges();
                        res.Message = "Sửa thành công";
                        res.StatusCode = "200";
                    }
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
