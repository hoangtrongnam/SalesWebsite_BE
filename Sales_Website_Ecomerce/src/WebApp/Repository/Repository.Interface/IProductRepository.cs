using Models.RequestModel;
using Models.RequestModel.Product;
using Models.ResponseModels;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IProductRepository: ICreateRepository<CreateProductRequestModel, int>
    {

    }
}
