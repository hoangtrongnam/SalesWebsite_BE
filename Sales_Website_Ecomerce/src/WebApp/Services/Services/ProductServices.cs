using UnitOfWork.Interface;
using Common;
using Models.RequestModel.Product;
using Models.ResponseModels.Product;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Services
{
    public interface IProductServices
    {
        ApiResponse<int> CreateProduct(CreateOnlyProductRequestModel model, Guid tenanlID);
        ApiResponse<int> CreateImages(List<ImageRequestModel> listImage);
        ApiResponse<int> CreatePrices(List<PriceRequestModel> listPrice);
        ApiResponse<ProductResponseModel> GetProductByID(Guid id);
        ApiResponse<List<ImageResponseModel>> GetImagesByProductID(Guid productID);
        ApiResponse<List<PriceResponseModel>> GetPricesByProductID(Guid productID);
        ApiResponse<List<ProductResponseModel>> GetProductByCategory(Guid categoryId);
        ApiResponse<int> UpdateProduct(UpdateProductRequestModel model, Guid id);
        ApiResponse<PageableObject<ProductResponseModel>> GetProducts(FilterProductByConditionRequestModel model, Guid tenanId);
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

                for (int i = 0; i < listImage.Count; i++)
                {
                    codeGenOld = GenerateCode.GenCode(codeGenOld);
                    imagesRepository[i].ImageCode = codeGenOld;
                    imagesRepository[i].ImageID = Guid.NewGuid();
                    imagesRepository[i].CreateBy = Parameters.CreateBy;

                    var product = context.Repositories.ProductRepository.Get(listImage[i].ProductId);
                    if (product == null)
                        return ApiResponse<int>.ErrorResponse($"Product {listImage[i].ProductId} does not exists.");
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
                    pricesRepository[i].CreateBy = Parameters.CreateBy;

                    var product = context.Repositories.ProductRepository.Get(listPrice[i].ProductId);
                    if (product == null)
                        return ApiResponse<int>.ErrorResponse($"Product {listPrice[i].ProductId} does not exists.");
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
        public ApiResponse<int> CreateProduct(CreateOnlyProductRequestModel model, Guid tenanlID)
        {
            using (var context = _unitOfWork.Create())
            {
                var codeOld = context.Repositories.CommonRepository.GetCodeGenerate(Parameters.tables["Product"].TableName, Parameters.tables["Product"].ColumnName);

                var modelMap = _mapper.Map<CreateOnlyProductRepositoryRequestModel>(model);
                modelMap.ProductID = Guid.NewGuid();
                modelMap.ProductCode = GenerateCode.GenCode(codeOld);

                var category = context.Repositories.CategoryRepository.Get(model.CategoryId);
                if (category == null)
                    return ApiResponse<int>.ErrorResponse("Category Doest not Exists");

                var result = context.Repositories.ProductRepository.Create(modelMap, tenanlID);
                if (result <= 0)
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
        public ApiResponse<List<ImageResponseModel>> GetImagesByProductID(Guid productID)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.ProductRepository.GetImages(productID);
                return ApiResponse<List<ImageResponseModel>>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// get list price by product
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public ApiResponse<List<PriceResponseModel>> GetPricesByProductID(Guid productID)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.ProductRepository.GetPrices(productID);
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
        public ApiResponse<List<ProductResponseModel>> GetProductByCategory(Guid categoryId)
        {
            using (var context = _unitOfWork.Create())
            {
                var products = context.Repositories.ProductRepository.GetProductCategory(categoryId);
                return ApiResponse<List<ProductResponseModel>>.SuccessResponse(products);
            }
        }
        /// <summary>
        /// Update product service
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public ApiResponse<int> UpdateProduct(UpdateProductRequestModel model, Guid productID)
        {
            using (var context = _unitOfWork.Create())
            {
                var product = context.Repositories.ProductRepository.Get(productID);
                if (product == null)
                    return ApiResponse<int>.ErrorResponse("Product does not exist.");

                var result = context.Repositories.ProductRepository.Update(model, productID);
                if (result <= 0)
                    return ApiResponse<int>.ErrorResponse("Update product fail.");

                context.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }

        public ApiResponse<PageableObject<ProductResponseModel>> GetProducts(FilterProductByConditionRequestModel model, Guid tenanId)
        {
            var products = new List<ProductResponseModel>();
            var result = new PageableObject<ProductResponseModel>();
            using (var context = _unitOfWork.Create())
            {
                products = context.Repositories.ProductRepository.GetProducts(tenanId);
                if (products.Any())
                {
                    products = products.Where(p =>
                                            (!model.ProductId.HasValue || p.ProductID == model.ProductId) &&
                                            (string.IsNullOrEmpty(model.ProductCode) || p.ProductCode == model.ProductCode) &&
                                            (!model.CategoryId.HasValue || p.CategoryID == model.CategoryId) &&
                                            (string.IsNullOrEmpty(model.Name) || p.Name.Contains(model.Name)) &&
                                            (!model.Status.HasValue || p.Status == model.Status)
                                        ).ToList();

                    int totalItems = products.Count();
                    int totalPages = 1;

                    bool applyPagination = model.PageNumber.HasValue && model.PageSize.HasValue;
                    if (applyPagination)
                    {
                        totalPages = (int)Math.Ceiling((double)totalItems / (model.PageSize ??= totalItems));
                        products = products.Skip(((model.PageNumber ??= 1) - 1) * (model.PageSize ??= totalItems)).Take(model.PageSize ??= totalItems).ToList();
                    }

                    products.ForEach(p =>
                    {
                        p.Images = context.Repositories.ProductRepository.GetImages(p.ProductID).OrderBy(i => i.ImageID).Take(1).ToList();
                    });

                    //Set result return
                    result.TotalRecord = totalItems;
                    result.TotalPage = totalPages;
                    result.Elements = products;
                }
            }

            return ApiResponse<PageableObject<ProductResponseModel>>.SuccessResponse(result);
        }
    }
}
