using Models.RequestModel.Category;
using Models.ResponseModels.Category;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface ICategoryRepository : IReadRepository<CategoryResponseModel, Guid>, IUpdateRepository<UpdateCategoryRequestModel, Guid>, IRemoveRepository<Guid>
    {
        CategoryResponseModel Create(CreateCategoryRepositoryRequestModel model, Guid tenantID);
        List<CategoryResponseModel> GetAllCategory(Guid tenantID);
        List<CategoryResponseModel> GetChildCategoysById(Guid categoryID);
        List<StatusResponseModel> GetStatus(string key);
    }
}
