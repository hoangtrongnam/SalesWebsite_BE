using UnitOfWork.Interface;
using Common;
using Models.RequestModel.Product;
using Models.ResponseModels.Product;
using AutoMapper;

namespace Services
{
    public interface IProductServices
    {
        ApiResponse<int> CreateProduct(CreateOnlyProductRequestModel model, Guid TenanlID);
        ApiResponse<int> CreateImages(List<ImageRequestModel> listImage);
        ApiResponse<int> CreatePrices(List<PriceRequestModel> listPrice);
        ApiResponse<ProductResponseModel> GetProductByID(Guid id);
        ApiResponse<List<ImageResponseModel>> GetImagesByProductID(Guid ProductID);
        ApiResponse<List<PriceResponseModel>> GetPricesByProductID(Guid ProductID);
        ApiResponse<List<ProductResponseModel>> GetProductByCategory(Guid CategoryId);
        ApiResponse<int> UpdateProduct(UpdateProductRequestModel model, Guid ProductID);
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
                var imagesRepository = new List<ImageRepositoryRequestModel>();
                _mapper.Map(listImage, imagesRepository);

                var codeGenOld = context.Repositories.CommonRepository.GetCodeGenerate(Parameters.tables["Image"].TableName, Parameters.tables["Image"].ColumnName); 

                for(int i = 0; i < listImage.Count; i++)
                {
                    codeGenOld = GenerateCode.GenCode(codeGenOld);
                    imagesRepository[i].ImageCode = codeGenOld;
                    imagesRepository[i].ImageID = Guid.NewGuid();

                    var product = context.Repositories.ProductRepository.Get(listImage[i].ProductID);
                    if (product == null)
                        return ApiResponse<int>.ErrorResponse($"Product {listImage[i].ProductID} does not exists.");
                } 

                //Create multiple image 
                var result = context.Repositories.ProductRepository.CreateImages(imagesRepository);
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
                var pricesRepository = new List<PriceRepositoryRequestModel>();
                _mapper.Map(listPrice, pricesRepository);

                var codeGenOld = context.Repositories.CommonRepository.GetCodeGenerate(Parameters.tables["Price"].TableName, Parameters.tables["Price"].ColumnName);

                for (int i = 0; i < listPrice.Count; i++)
                {
                    codeGenOld = GenerateCode.GenCode(codeGenOld);
                    pricesRepository[i].PriceCode = codeGenOld;
                    pricesRepository[i].PriceID = Guid.NewGuid();

                    var product = context.Repositories.ProductRepository.Get(listPrice[i].ProductID);
                    if (product == null)
                        return ApiResponse<int>.ErrorResponse($"Product {listPrice[i].ProductID} does not exists.");
                }

                //Create multiple Price 
                var result = context.Repositories.ProductRepository.CreatePrices(pricesRepository);
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
        public ApiResponse<int> CreateProduct(CreateOnlyProductRequestModel model, Guid TenanlID)
        {
            using(var context = _unitOfWork.Create())
            {
                var codeOld = context.Repositories.CommonRepository.GetCodeGenerate(Parameters.tables["Product"].TableName, Parameters.tables["Product"].ColumnName);

                var modelMap = _mapper.Map<CreateOnlyProductRepositoryRequestModel>(model);
                modelMap.ProductID = Guid.NewGuid();
                modelMap.ProductCode = GenerateCode.GenCode(codeOld);

                var category = context.Repositories.CategoryRepository.Get(model.CategoryID);
                if (category == null)
                    return ApiResponse<int>.ErrorResponse("Category Doest not Exists");

                var result = context.Repositories.ProductRepository.Create(modelMap, TenanlID);
                if(result <= 0)
                    return ApiResponse<int>.ErrorResponse("Create Product Fail");

                context.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// get list image by product
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public ApiResponse<List<ImageResponseModel>> GetImagesByProductID(Guid ProductID)
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
        public ApiResponse<List<PriceResponseModel>> GetPricesByProductID(Guid ProductID)
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
        public ApiResponse<ProductResponseModel> GetProductByID(Guid id)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.ProductRepository.Get(id);
                return ApiResponse<ProductResponseModel>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// get product by category
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ApiResponse<List<ProductResponseModel>> GetProductByCategory(Guid CategoryId)
        {
            using (var context = _unitOfWork.Create())
            {
                var products = context.Repositories.ProductRepository.GetProductCategory(CategoryId);
                return ApiResponse<List<ProductResponseModel>>.SuccessResponse(products);
            }
        }
        /// <summary>
        /// Update product service
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public ApiResponse<int> UpdateProduct(UpdateProductRequestModel model, Guid ProductID)
        {
            using (var context = _unitOfWork.Create())
            {
                var product = context.Repositories.ProductRepository.Get(ProductID);
                if (product == null)
                    return ApiResponse<int>.ErrorResponse("Product does not exist.");

                var result = context.Repositories.ProductRepository.Update(model, ProductID);
                if(result <= 0)
                    return ApiResponse<int>.ErrorResponse("Update product fail.");

                context.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }
    }
}
