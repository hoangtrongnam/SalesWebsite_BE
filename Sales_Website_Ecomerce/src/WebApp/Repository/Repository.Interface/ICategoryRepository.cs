using Models.RequestModel.Category;
using Models.ResponseModels;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface ICategoryRepository : ICreateRepository<CreateCategoryRequestModel, CategoryResponseModel>, IReadRepository<CategoryResponseModel, GetCategoryCommonRequestModel>
    {
    }
}
