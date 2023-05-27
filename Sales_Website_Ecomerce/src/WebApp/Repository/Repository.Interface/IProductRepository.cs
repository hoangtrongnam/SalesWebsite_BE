using Models.RequestModel.Product;
using Models.ResponseModels.Product;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IProductRepository: ICreateRepository<CreateOnlyProductRequestModel, int>,ICreateRepository<List<ImageRequestModel>, int>, ICreateRepository<List<PriceRequestModel>, int>
        ,IReadRepository<ProductResponseModel,int>
    {
        List<ImageResponseModel> GetImages(int ProductID);
        List<PriceResponseModel> GetPrices(int ProductID);
    }
}
