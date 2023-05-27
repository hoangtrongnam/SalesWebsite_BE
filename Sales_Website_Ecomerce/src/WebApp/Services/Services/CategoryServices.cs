using UnitOfWork.Interface;
using Common;
using Models.RequestModel.Category;
using AutoMapper;
using Models.ResponseModels.Category;

namespace Services
{
    public interface ICategoryServices
    {
        ApiResponse<CategoryResponseModel> CreateCategory(CreateCategoryRequestModel model);
        ApiResponse<CategoryResponseModel> GetCategoryByID(GetCategoryByID_ParentTenantRequestModel model);
        ApiResponse<List<CategoryResponseModel>> GetCategoryByTenantParent(int TenantId, int Parent);
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
        /// CreateCategory Service
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResponse<CategoryResponseModel> CreateCategory(CreateCategoryRequestModel model)
        {
            using (var context = _unitOfWork.Create())
            {
                var modelGetCategoryByName = new GetCategoryCommonRequestModel()
                {
                    Name = model.Name,
                    TenantID = model.TenantID,
                    Parent = model.Parent
                };
                var tenant = context.Repositories.TenantRepository.Get(model.TenantID);
                if(tenant == null)
                    return ApiResponse<CategoryResponseModel>.ErrorResponse("Tenant Doest not Exists");

                var category = context.Repositories.CategoryRepository.Get(modelGetCategoryByName);
                if(category != null)
                    return ApiResponse<CategoryResponseModel>.ErrorResponse("Category Already Exists");

                var result = context.Repositories.CategoryRepository.Create(model);
                context.SaveChanges();

                if(result == null)
                    return ApiResponse<CategoryResponseModel>.ErrorResponse("Create Categoty Fail");
                
                return ApiResponse<CategoryResponseModel>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Get category by ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResponse<CategoryResponseModel> GetCategoryByID(GetCategoryByID_ParentTenantRequestModel model)
        {
            var modelMap = _mapper.Map<GetCategoryCommonRequestModel>(model);
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.CategoryRepository.Get(modelMap);
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
        public ApiResponse<List<CategoryResponseModel>> GetCategoryByTenantParent(int TenantId, int Parent)
        {
            using (var context = _unitOfWork.Create())
            {
                var modelGetCategoryTenantParent = new GetCategoryCommonRequestModel()
                {
                    TenantID = TenantId,
                    Parent = Parent
                };
                var result = context.Repositories.CategoryRepository.GetAll(modelGetCategoryTenantParent);

                if (result == null)
                    return ApiResponse<List<CategoryResponseModel>>.ErrorResponse("No Data");

                return ApiResponse<List<CategoryResponseModel>>.SuccessResponse(result);
            }
        }
    }
}
