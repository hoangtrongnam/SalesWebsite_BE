using Models.RequestModel;
using UnitOfWork.Interface;
using Common;
using Models.ResponseModels;
using Service.Interface;

namespace Services
{
    public class CategoryServices : IServices<CategoryRequestModel, int>
    {
        private IUnitOfWork _unitOfWork;

        public CategoryServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResultModel Create(CategoryRequestModel item)
        {
            //throw new NotImplementedException();
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    ResultModel outModel = new ResultModel();
                    var result = context.Repositories.CategoryRepository.Create(item);
                    if (result == 0)
                    {
                        //Console.WriteLine("Add thất bại");
                        outModel.Message = "Thêm thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        context.SaveChanges();
                        //Console.WriteLine("Đã Add {0} bản ghi.", rowsAffected);
                        outModel.Message = "Thêm thành công";
                        outModel.StatusCode = "200";
                    }
                    return outModel;
                }
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
                using (var context = _unitOfWork.Create())
                {
                    ResultModel outModel = new ResultModel();
                    var result = context.Repositories.CategoryRepository.Remove(id);
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
                    return outModel;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ResultModel Delete(CategoryRequestModel model, int cartID)
        {
            throw new NotImplementedException();
        }

        public ResultModel Get(int id)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    ResultModel outModel = new ResultModel();
                    CategoryResponseModel result = context.Repositories.CategoryRepository.Get(id);
                    if (string.IsNullOrEmpty(result.Name))
                    {
                        outModel.Message = "tìm sản phẩm thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        outModel.Message = "tìm sản phẩm thành công";
                        outModel.StatusCode = "200";
                        outModel.DATA = result.Name;
                    }
                    return outModel;
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public ResultModel Get(int id, int pageIndex)
        {
            throw new NotImplementedException();
        }

        public ResultModel GetAll()
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    ResultModel outModel = new ResultModel();
                    List<CategoryResponseModel> result = context.Repositories.CategoryRepository.GetAll();
                    if (result.Count ==0)
                    {
                        outModel.Message = "tìm danh sách sản phẩm thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        outModel.Message = "tìm danh sáchsản phẩm thành công";
                        outModel.StatusCode = "200";
                        outModel.DATA = result;
                    }
                    return outModel;
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public ResultModel GetAll(int pageIndex)
        {
            throw new NotImplementedException();
        }

        public ResultModel Update(CategoryRequestModel item, int CategoryID)
        {
            try
            {
                using (var context = _unitOfWork.Create())
                {
                    ResultModel outModel = new ResultModel();
                    var result = context.Repositories.CategoryRepository.Update(item, CategoryID);
                    if (result == 0)
                    {
                        outModel.Message = "Sửa thất bại";
                        outModel.StatusCode = "999";
                    }
                    else
                    {
                        context.SaveChanges();
                        outModel.Message = "Sửa thành công";
                        outModel.StatusCode = "200";
                    }
                    return outModel;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
