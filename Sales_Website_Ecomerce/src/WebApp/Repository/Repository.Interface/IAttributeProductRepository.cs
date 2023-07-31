using Models.RequestModel.AtributeProduct;
using Models.RequestModel.Product;
using Models.ResponseModels.AtributeProduct;
using Models.ResponseModels.AttributeProduct;
using Models.ResponseModels.Product;

namespace Repository.Interface
{
    public interface IAttributeProductRepository
    {
        List<ImageResponseModel> GetAllImages();
        List<ColorResponseModel> GetColors();
        List<SizeResponseModel> GetSizes();
        SizeResponseModel GetSize(Guid sizeId);
        ColorResponseModel GetColor(Guid colorId);
        ColorSizeRepositoryResponseModel GetColorSizeProduct(Guid productId);
        List<ImageByColorResponseModel> GetImageByColor(Guid productId, Guid colorId);
        int CreateImages(List<ImageRepositoryRequestModel> item);
        int CreateColor(ColorRepositoryRequestModel model);
        int CreateSize(SizeRepositoryRequestModel model);
        int CreateProductColorImage(List<ProductColorImageRepositoryRequestModel> model);
    }
}
