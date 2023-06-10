using Models.RequestModel;
using Models.RequestModel.Cart;
using Models.ResponseModels.Cart;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface ICartRepository //: IReadRepository<CartResponeModel, int>, ICreateRepository<CartRequestModel>, IUpdateRepository<CartRequestModel, int>, IRemoveRepository<CartRequestModel, int>
    {
        CartResponeModel Get(int customerID, int pageIndex = 1);
        int GetCartIDByCustomerID(int customerID);
        List<CartModel> GetCartProduct(int produtID, int cartID);
        int UpdateCartProduct(CartRequestModel item, int cartID);
        int GetProductInStock(int productID, int wareHouseID);
        int InsertCartProduct(CartRequestModel item, int cartID);
        int CreateCart(int customerID);
    }
}
