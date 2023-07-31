using AutoMapper;
using Common;
using Models.RequestModel.AtributeProduct;
using Models.RequestModel.Product;
using Models.ResponseModels.AtributeProduct;
using Models.ResponseModels.Product;
using System.Drawing;
using UnitOfWork.Interface;

namespace Services
{
    public interface IAttributeProductService
    {
        ApiResponse<List<ImageResponseModel>> GetAllImages();
        ApiResponse<List<ColorResponseModel>> GetColors();
        ApiResponse<List<SizeResponseModel>> GetSizes();
        ApiResponse<int> CreateImages(List<ImageRequestModel> listImage);
        ApiResponse<int> CreateColor(ColorRepositoryRequestModel model);
        ApiResponse<int> CreateSize(SizeRepositoryRequestModel model);
        ApiResponse<ColorSizeResponseModel> GetColorSizeProduct(Guid productId);
    }

    public class AttributeProductService : IAttributeProductService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AttributeProductService(IUnitOfWork unitOfWork, IMapper mapper)
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
                }

                //Create multiple image 
                var result = context.Repositories.AtributeProductRepository.CreateImages(imagesRepository);
                if (result <= 0)
                    return ApiResponse<int>.ErrorResponse("Create images Fail");

                context.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Get all images service
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ApiResponse<List<ImageResponseModel>> GetAllImages()
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.AtributeProductRepository.GetAllImages();
                return ApiResponse<List<ImageResponseModel>>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Create Color service
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResponse<int> CreateColor(ColorRepositoryRequestModel model)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.AtributeProductRepository.CreateColor(model);
                if (result <= 0)
                    return ApiResponse<int>.ErrorResponse("Create color fail");

                context.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Create Size service
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResponse<int> CreateSize(SizeRepositoryRequestModel model)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.AtributeProductRepository.CreateSize(model);
                if (result <= 0)
                    return ApiResponse<int>.ErrorResponse("Create size fail");

                context.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Get Colors service
        /// </summary>
        /// <returns></returns>
        public ApiResponse<List<ColorResponseModel>> GetColors()
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.AtributeProductRepository.GetColors();
                return ApiResponse<List<ColorResponseModel>>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Get Size service
        /// </summary>
        /// <returns></returns>
        public ApiResponse<List<SizeResponseModel>> GetSizes()
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.AtributeProductRepository.GetSizes();
                return ApiResponse<List<SizeResponseModel>>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// GetColorSizeProduct Service
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ApiResponse<ColorSizeResponseModel> GetColorSizeProduct(Guid productId)
        {
            using (var context = _unitOfWork.Create())
            {
                var data = context.Repositories.AtributeProductRepository.GetColorSizeProduct(productId);
                
                var listColor = data.colors.GroupBy(c => new { c.ColorID, c.ColorCode, c.Name, c.Description })
                    .Select(group => new ColorModel
                    {
                        ColorID = group.Key.ColorID,
                        ColorCode = group.Key.ColorCode,
                        Name = group.Key.Name,
                        Description = group.Key.Description,
                        TotalStock = group.Sum(c => c.TotalStock),
                        Sizes = group.Select(c=> new OnlySize { SizeID = c.SizeID, Value = c.Value}).ToList()

                    }).ToList();

                var listSize = data.sizes.GroupBy(s => new { s.SizeID, s.Value, s.Description })
                    .Select(group => new SizeModel
                    {
                        SizeID = group.Key.SizeID,
                        Value = group.Key.Value,
                        Description = group.Key.Description,
                        TotalStock = group.Sum(s => s.TotalStock),
                        Colors = group.Select(c => new OnlyColor 
                        { 
                            ColorID = c.ColorID,
                            ColorCode = c.ColorCode,
                            Name = c.Name
                        }).ToList()
                    }).ToList();

                return ApiResponse<ColorSizeResponseModel>.SuccessResponse(new ColorSizeResponseModel
                {
                    listColor = listColor,
                    listSize = listSize
                });
            }
        }
    }
}
