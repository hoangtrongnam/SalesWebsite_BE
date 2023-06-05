using Models.RequestModel;
using Models.ResponseModels.Cart;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface ICartRepository //: IReadRepository<CartResponeModel, int>, ICreateRepository<CartRequestModel>, IUpdateRepository<CartRequestModel, int>, IRemoveRepository<CartRequestModel, int>
    {
        CartResponeModel Get(int customerID, int pageIndex = 1);
    }
}
