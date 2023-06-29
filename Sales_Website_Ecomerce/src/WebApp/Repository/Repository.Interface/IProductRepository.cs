using Models.RequestModel.Product;
using Models.ResponseModels.Product;
using Repository.Interfaces.Actions;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IProductRepository: IReadRepository<ProductResponseModel,Guid>, IUpdateRepository<UpdateProductRequestModel, Guid>
    {
        int Create(CreateOnlyProductRepositoryRequestModel model, Guid TenantID);
        List<ImageResponseModel> GetImages(Guid ProductID);
        List<PriceResponseModel> GetPrices(Guid ProductID);
        int CreatePrices(List<PriceRepositoryRequestModel> item);
        int CreateImages(List<ImageRepositoryRequestModel> item);
        List<ProductResponseModel> GetProductCategory(Guid CategoryID);
        List<ProductResponseModel> GetProducts(string tenantId);
    }
}
