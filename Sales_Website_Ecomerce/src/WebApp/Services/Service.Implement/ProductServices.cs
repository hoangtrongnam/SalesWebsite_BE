using Models.RequestModel;
using UnitOfWork.Interface;
using Common;
using Service.Interface;

namespace Services
{
    public class ProductServices : IServices<ProductRequestModel, int>
    {
        private IUnitOfWork _unitOfWork;

        public ProductServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResultModel Create(ProductRequestModel item)
        {
            try
            {
                //throw new NotImplementedException();
                ResultModel outModel = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.ProductRepository.Create(item);
                    if (result == 0)
                    {
                        outModel.Message = "Thêm thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
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

        public ResultModel Delete(int id)
        {
            //throw new NotImplementedException();
            try
            {
                ResultModel outModel = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.ProductRepository.Remove(id);
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

        public ResultModel Delete(ProductRequestModel model, int ID)
        {
            throw new NotImplementedException();
        }

        public ResultModel Get(int id)
        {
            try
            {
                ResultModel outModel = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.ProductRepository.Get(id);
                    if (string.IsNullOrEmpty(result.ProductID))
                    {
                        outModel.Message = "Tìm sản phấm thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        outModel.Message = "Tìm sản phấm thành công";
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

        public ResultModel Get(int ID, int pageIndex)
        {
            throw new NotImplementedException();
        }

        public ResultModel GetAll(int pageIndex)
        {
            try
            {
                ResultModel outModel = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.ProductRepository.GetAll(pageIndex);
                    if (result.Count == 0)
                    {
                        outModel.Message = "Tìm tất cả sản phấm thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        outModel.Message = "Tìm tất cả sản phấm thành công";
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

        public ResultModel GetAll()
        {
            throw new NotImplementedException();
        }

        public ResultModel Update(ProductRequestModel item, int productID)
        {
            try
            {
                ResultModel res = new ResultModel();
                using (var context = _unitOfWork.Create())
                {
                    var result = context.Repositories.ProductRepository.Update(item, productID);
                    if (result == 0)
                    {
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
