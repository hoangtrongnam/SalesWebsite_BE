using Models.RequestModel.ProductStock;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IProductStockRepository: ICreateRepository<CreateProductStockRepositoryRequestModel, int>
    {
        int HoldProduct(HoldProductRequestModel model);
    }
}
