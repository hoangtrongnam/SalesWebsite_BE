using Models.RequestModel.Product;
using Models.ResponseModels.Product;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IProductRepository: IReadRepository<ProductResponseModel,Guid>, IUpdateRepository<UpdateProductRequestModel, Guid>
    {
        int Create(CreateOnlyProductRepositoryRequestModel model, Guid tenantID);
        List<ImageResponseModel> GetImagesProduct(Guid productId);
        List<PriceResponseModel> GetPrices(Guid productID);
        int CreatePrices(List<PriceRepositoryRequestModel> item);     
        List<ProductResponseModel> GetProductCategory(Guid categoryID);
        List<ProductResponseModel> GetProducts(Guid tenantId);
    }
}
