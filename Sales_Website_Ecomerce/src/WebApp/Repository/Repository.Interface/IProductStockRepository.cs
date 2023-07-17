using Models.RequestModel.ProductStock;
using Models.ResponseModels.ProductStocks;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IProductStockRepository: ICreateRepository<CreateProductStockRepositoryRequestModel, int>
    {
        int HoldProduct(HoldProductRequestModel model);
        ProductStockResponseModel GetHoldProduct(HoldProductRequestModel model);
    }
}
