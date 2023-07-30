using Models.RequestModel.AtributeProduct;
using Models.RequestModel.Product;
using Models.ResponseModels.AtributeProduct;
using Models.ResponseModels.Product;

namespace Repository.Interface
{
    public interface IAtributeProductRepository
    {
        List<ImageResponseModel> GetAllImages();
        List<ColorResponseModel> GetColors();
        List<SizeResponseModel> GetSizes();
        int CreateImages(List<ImageRepositoryRequestModel> item);
        int CreateColor(ColorRepositoryRequestModel model);
        int CreateSize(SizeRepositoryRequestModel model);
        int CreateProductColorImage(List<ProductColorImageRepositoryRequestModel> model);
    }
}
