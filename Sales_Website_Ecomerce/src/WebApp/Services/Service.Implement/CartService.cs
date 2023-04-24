using Models.RequestModel;
using UnitOfWork.Interface;
using Common;
using Service.Interface;

namespace Services
{    
    public class CartServices : IServices<CartRequestModel, int>    
    {
        private IUnitOfWork _unitOfWork;

        public CartServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResultModel Create(CartRequestModel item)
        {
            try
            {
                //throw new NotImplementedException();
                ResultModel outModel = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.CartRepository.Create(item);
                    if (result == 0)
                    {
                        context.DeleteChanges();
                        outModel.Message = "Thêm Cart thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        context.SaveChanges();
                        outModel.Message = "Thêm Cart thành công";
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

        public ResultModel Delete(CartRequestModel model, int cartID)
        {
            //throw new NotImplementedException();
            try
            {
                ResultModel outModel = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.CartRepository.Remove(model, cartID);
                    if (result == 0)
                    {
                        outModel.Message = "Xóa thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        context.SaveChanges();
                        outModel.Message = "Xóa thành công";
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

        public ResultModel Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel Get(int customerID, int pageIndex)
        {
            try
            {
                ResultModel outModel = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.CartRepository.Get(customerID, pageIndex);
                    if (result.CartID == 0)
                    {
                        outModel.Message = "Tìm giỏ hàng thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        outModel.Message = "Tìm giỏ hàng thành công";
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

        public ResultModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel GetAll()
        {
            throw new NotImplementedException();
        }

        public ResultModel GetAll(int pageIndex)
        {
            throw new NotImplementedException();
        }

        //public ResultModel GetAll(int pageIndex)
        //{
        //    try
        //    {
        //        ResultModel outModel = new ResultModel();
        //        using (var context = _unitOfWork.Create())
        //        {
        //            var result = context.Repositories.ProductRepository.GetAll(pageIndex);
        //            if (result.Count == 0)
        //            {
        //                outModel.Message = "Tìm tất cả sản phấm thất bại";
        //                outModel.StatusCode = "999";
        //            }
        //            else
        //            {
        //                outModel.Message = "Tìm tất cả sản phấm thành công";
        //                outModel.StatusCode = "200";
        //                outModel.DATA = result;
        //            }
        //        }
        //        return outModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public ResultModel Update(CartRequestModel item, int cartID)
        {
            try
            {
                ResultModel res = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.CartRepository.Update(item, cartID);
                    if (result == 0)
                    {
                        res.Message = "Sửa giỏ hàng thất bại";
                        res.StatusCode = "999";
                    }
                    else
                    {
                        context.SaveChanges();
                        res.Message = "Sửa giỏ hàng thành công";
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
