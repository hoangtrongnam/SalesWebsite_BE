using Models.RequestModel.Product;
using Models.ResponseModels.Product;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IProductRepository: ICreateRepository<CreateOnlyProductRequestModel, int>
        ,IReadRepository<ProductResponseModel,int>, IUpdateRepository<UpdateProductRequestModel,int>
    {
        List<ImageResponseModel> GetImages(int ProductID);
        List<PriceResponseModel> GetPrices(int ProductID);
        int CreatePrices(List<PriceRequestModel> item);
        int CreateImages(List<ImageRequestModel> item);
        List<ProductResponseModel> GetProductCategory(int CategoryID);
        List<ProductResponseModel> GetProducts(int tenantId);
    }
}
