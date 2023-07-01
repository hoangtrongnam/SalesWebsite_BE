using UnitOfWork.Interface;
using Common;
using Models.RequestModel.Category;
using AutoMapper;
using Models.ResponseModels.Category;

namespace Services
{
    public interface ICategoryServices
    {
        ApiResponse<CategoryResponseModel> CreateCategory(CreateCategoryRequestModel model, Guid tenantID);
        ApiResponse<CategoryResponseModel> GetCategoryByID(Guid id);
        ApiResponse<List<CategoryResponseModel>> GetChildCategoryByCategoyId(Guid categotyID);
        ApiResponse<List<CategoryResponseModel>> GetAllCategory(Guid tenantID);
        ApiResponse<int> UpdateCategoryByID(UpdateCategoryRequestModel item, Guid id);
        ApiResponse<int> RemoveCategoryByID(Guid id);
        ApiResponse<List<StatusResponseModel>> GetStatus(string key);
    }
    public class CategoryServices : ICategoryServices
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
      
        /// <summary>
        /// Get category by ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResponse<CategoryResponseModel> GetCategoryByID(Guid id)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.CategoryRepository.Get(id);
                if (result == null)
                {
                    return ApiResponse<CategoryResponseModel>.ErrorResponse("Category is not found");
                }
                return ApiResponse<CategoryResponseModel>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// GetCategoryByTenantParent service
        /// </summary>
        /// <param name="TenantId"></param>
        /// <param name="Parent"></param>
        /// <returns></returns>
        public ApiResponse<List<CategoryResponseModel>> GetChildCategoryByCategoyId(Guid categotyID)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.CategoryRepository.GetChildCategoysById(categotyID);

                if (result == null)
                    return ApiResponse<List<CategoryResponseModel>>.ErrorResponse("No Data");

                return ApiResponse<List<CategoryResponseModel>>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Get All category service
        /// </summary>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public ApiResponse<List<CategoryResponseModel>> GetAllCategory(Guid tenantID)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.CategoryRepository.GetAllCategory(tenantID);

                if (result == null)
                    return ApiResponse<List<CategoryResponseModel>>.ErrorResponse("No Data");

                return ApiResponse<List<CategoryResponseModel>>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// CreateCategory Service
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResponse<CategoryResponseModel> CreateCategory(CreateCategoryRequestModel model, Guid tenantID)
        {
            using (var context = _unitOfWork.Create())
            {
                var codeOld = context.Repositories.CommonRepository.GetCodeGenerate(Parameters.tables["CategoryProduct"].TableName, Parameters.tables["CategoryProduct"].ColumnName);
                var modelMap = _mapper.Map<CreateCategoryRepositoryRequestModel>(model);
                modelMap.CategoryID = Guid.NewGuid();
                modelMap.CategoryCode = GenerateCode.GenCode(codeOld);

                var tenant = context.Repositories.TenantRepository.Get(tenantID);
                if (tenant == null)
                    return ApiResponse<CategoryResponseModel>.ErrorResponse("Tenant Doest not Exists");

                var result = context.Repositories.CategoryRepository.Create(modelMap, tenantID);
                context.SaveChanges();

                if (result == null)
                    return ApiResponse<CategoryResponseModel>.ErrorResponse("Create Categoty Fail");

                return ApiResponse<CategoryResponseModel>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Update category service
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApiResponse<int> UpdateCategoryByID(UpdateCategoryRequestModel item, Guid categotyId)
        {
            using (var context = _unitOfWork.Create())
            {
                var category = context.Repositories.CategoryRepository.Get(categotyId);
                if (category == null)
                    return ApiResponse<int>.ErrorResponse($"Category {item.Name} doesn't exists.");
                
                var result = context.Repositories.CategoryRepository.Update(item, categotyId);
                if(result <= 0)
                    return ApiResponse<int>.ErrorResponse("Update category fail.");

                context.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Remove Category service
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApiResponse<int> RemoveCategoryByID(Guid id)
        {
            using (var context = _unitOfWork.Create())
            {
                var category = context.Repositories.CategoryRepository.Get(id);
                if (category == null)
                    return ApiResponse<int>.ErrorResponse($"Category doesn't exists.");

                var products = context.Repositories.ProductRepository.GetProductCategory(id);
                if (products.Any())
                    return ApiResponse<int>.ErrorResponse("Catalog has products.");

                var result = context.Repositories.CategoryRepository.Remove(id);
                if (result <= 0)
                    return ApiResponse<int>.ErrorResponse("Remove category fail.");

                context.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Get status service
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ApiResponse<List<StatusResponseModel>> GetStatus(string key)
        {
            using (var context = _unitOfWork.Create())
            {
                var rerult = context.Repositories.CategoryRepository.GetStatus(key);
                return ApiResponse<List<StatusResponseModel>>.SuccessResponse(rerult);
            };
        }
    }
}
