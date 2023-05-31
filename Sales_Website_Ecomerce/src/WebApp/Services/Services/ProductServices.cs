using UnitOfWork.Interface;
using Common;
using Models.RequestModel.Product;
using Models.RequestModel.Category;
using Models.ResponseModels.Product;
using AutoMapper;

namespace Services
{
    public interface IProductServices
    {
        ApiResponse<int> CreateProduct(CreateOnlyProductRequestModel model);
        ApiResponse<int> CreateImages(List<ImageRequestModel> listImage);
        ApiResponse<int> CreatePrices(List<PriceRequestModel> listPrice);
        ApiResponse<ProductResponseModel> GetProductByID(int id);
        ApiResponse<List<ImageResponseModel>> GetImagesByProductID(int ProductID);
        ApiResponse<List<PriceResponseModel>> GetPricesByProductID(int ProductID);
    }
    public class ProductServices : IProductServices
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        /// Create multiple images
        /// </summary>
        /// <param name="listImage"></param>
        /// <returns></returns>
        public ApiResponse<int> CreateImages(List<ImageRequestModel> listImage)
        {
            using (var context = _unitOfWork.Create())
            {
                foreach(var item in listImage)
                {
                    var product = context.Repositories.ProductRepository.Get(item.ProductID);
                    if(product == null)
                        return ApiResponse<int>.ErrorResponse($"Product {item.ProductID} does not exists.");
                }
                
                //Create multiple image 
                var result = context.Repositories.ProductRepository.CreateImages(listImage);
                if (result <= 0)
                    return ApiResponse<int>.ErrorResponse("Create images Fail");

                context.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Create multiple prices
        /// </summary>
        /// <param name="listPrice"></param>
        /// <returns></returns>
        public ApiResponse<int> CreatePrices(List<PriceRequestModel> listPrice)
        {
            using (var context = _unitOfWork.Create())
            {
                foreach (var item in listPrice)
                {
                    var product = context.Repositories.ProductRepository.Get(item.ProductID);
                    if (product == null)
                        return ApiResponse<int>.ErrorResponse($"Product {item.ProductID} does not exists.");
                }

                //Create multiple Price 
                var result = context.Repositories.ProductRepository.CreatePrices(listPrice);
                if (result <= 0)
                    return ApiResponse<int>.ErrorResponse("Create prices Fail");

                context.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResponse<int> CreateProduct(CreateOnlyProductRequestModel model)
        {
            using(var context = _unitOfWork.Create())
            {
                var getCategotyByIDModel = new GetCategoryCommonRequestModel()
                {
                    ID = model.CategoryID,
                    TenantID = model.TenantID
                };
                var category = context.Repositories.CategoryRepository.GetByCondition(getCategotyByIDModel);
                if(category == null)
                    return ApiResponse<int>.ErrorResponse("Category Doest not Exists");
                
                //Insert Only Prroduct
                var productID = context.Repositories.ProductRepository.Create(model);
                if(productID <= 0)
                    return ApiResponse<int>.ErrorResponse("Create Product Fail");

                context.SaveChanges();
                return ApiResponse<int>.SuccessResponse(productID);
            }
        }
        /// <summary>
        /// get list image by product
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public ApiResponse<List<ImageResponseModel>> GetImagesByProductID(int ProductID)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.ProductRepository.GetImages(ProductID);
                return ApiResponse<List<ImageResponseModel>>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// get list price by product
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public ApiResponse<List<PriceResponseModel>> GetPricesByProductID(int ProductID)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.ProductRepository.GetPrices(ProductID);
                return ApiResponse<List<PriceResponseModel>>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApiResponse<ProductResponseModel> GetProductByID(int id)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.ProductRepository.Get(id);
                var images = context.Repositories.ProductRepository.GetImages(id);
                var prices = context.Repositories.ProductRepository.GetPrices(id);

                if (images.Any())
                {
                    var image = images.OrderByDescending(obj => obj.ID).FirstOrDefault();
                    _mapper.Map(image, result);
                }
                if (prices.Any())
                {
                    var price = prices.OrderByDescending(obj => obj.ID).FirstOrDefault();
                    _mapper.Map(price, result);
                }
                
                return ApiResponse<ProductResponseModel>.SuccessResponse(result);
            }
        }
    }
}
