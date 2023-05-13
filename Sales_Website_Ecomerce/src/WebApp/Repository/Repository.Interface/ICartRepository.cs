using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface ICartRepository : IReadRepository<CartResponeModel, int>, ICreateRepository<CartRequestModel>, IUpdateRepository<CartRequestModel, int>, IRemoveRepository<int, int>
    {

    }
}
