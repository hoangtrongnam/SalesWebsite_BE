using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface ICategoryRepository : IReadRepository<CategoryResponseModel, int>, ICreateRepository<CategoryRequestModel>, IUpdateRepository<CategoryRequestModel, int>, IRemoveRepository<CategoryRequestModel, int>
    {
    }
}
