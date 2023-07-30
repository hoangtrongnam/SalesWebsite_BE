using AutoMapper;
using Common;
using Models.RequestModel.AtributeProduct;
using Models.RequestModel.Product;
using Models.ResponseModels.AtributeProduct;
using Models.ResponseModels.Product;
using UnitOfWork.Interface;

namespace Services
{
    public interface IAtributeProductService
    {
        ApiResponse<List<ImageResponseModel>> GetAllImages();
        ApiResponse<List<ColorResponseModel>> GetColors();
        ApiResponse<List<SizeResponseModel>> GetSizes();
        ApiResponse<int> CreateImages(List<ImageRequestModel> listImage);
        ApiResponse<int> CreateColor(ColorRepositoryRequestModel model);
        ApiResponse<int> CreateSize(SizeRepositoryRequestModel model);
    }

    public class AtributeProductService : IAtributeProductService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AtributeProductService(IUnitOfWork unitOfWork, IMapper mapper)
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
    }
}
