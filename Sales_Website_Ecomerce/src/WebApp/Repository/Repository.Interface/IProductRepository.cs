using Models.RequestModel.Product;
using Models.ResponseModels.Product;
using Repository.Interfaces.Actions;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IProductRepository: IReadRepository<ProductResponseModel,Guid>, IUpdateRepository<UpdateProductRequestModel, Guid>
    {
        int Create(CreateOnlyProductRepositoryRequestModel model, Guid tenantID);
        List<ImageResponseModel> GetImages(Guid productID);
        List<PriceResponseModel> GetPrices(Guid productID);
        int CreatePrices(List<PriceRepositoryRequestModel> item);
        int CreateImages(List<ImageRepositoryRequestModel> item);
        List<ProductResponseModel> GetProductCategory(Guid categoryID);
        List<ProductResponseModel> GetProducts(Guid tenantId);
    }
}
