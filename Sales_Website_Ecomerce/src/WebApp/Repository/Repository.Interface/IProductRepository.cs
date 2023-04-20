using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IProductRepository :IReadRepository<ProductResponeModel, int>, ICreateRepository<ProductRequestModel>, IRemoveRepository<ProductRequestModel,int>, IUpdateRepository<ProductRequestModel, int>
    {

    }
}
