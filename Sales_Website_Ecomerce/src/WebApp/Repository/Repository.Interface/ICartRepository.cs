using Models.RequestModel;
using Models.RequestModel.Cart;
using Models.ResponseModels.Cart;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface ICartRepository //: IReadRepository<CartResponeModel, int>, ICreateRepository<CartRequestModel>, IUpdateRepository<CartRequestModel, int>, IRemoveRepository<CartRequestModel, int>
    {
        CartResponeModel Get(Guid customerID, int pageIndex = 1);
        Guid GetCartIDByCustomerID(Guid customerID);
        List<CartModel> GetCartProduct(Guid produtID, Guid cartID);
        int GetProductInStock(Guid productID, Guid wareHouseID);
        int InsertCartProduct(CartRequestModel item, Guid cartID, Guid cartProductID);
        int UpdateCartProduct(CartRequestModel item, Guid cartID, int status);
        Guid CreateCart(Guid customerID, Guid cartID);
        int UpdateCart(CartRequestModel item, Guid cartID, int status);
        int Remove(CartRequestModel item, Guid cartID);
    }
}
