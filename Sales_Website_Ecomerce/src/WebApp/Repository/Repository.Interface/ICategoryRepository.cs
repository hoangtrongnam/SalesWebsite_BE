using Models.RequestModel.Category;
using Models.ResponseModels.Category;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface ICategoryRepository : IReadRepository<CategoryResponseModel, Guid>, IUpdateRepository<UpdateCategoryRequestModel, Guid>, IRemoveRepository<Guid>
    {
        CategoryResponseModel Create(CreateCategoryRepositoryRequestModel model, Guid TenantID);
        List<CategoryResponseModel> GetAllCategory(Guid TenantID);
        List<CategoryResponseModel> GetChildCategoysById(Guid CategoryID);
        List<StatusResponseModel> GetStatus(string key);
    }
}
