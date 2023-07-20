using AutoMapper;
using Common;
using Models.RequestModel.ProductStock;
using UnitOfWork.Interface;

namespace Services
{
    public interface IProductStockService
    {
        ApiResponse<int> CreateProductStock(CreateProductStockRequestModel model);
        ApiResponse<int> HoldProduct(HoldProductRequestModel model);
    }
    public class ProductStockService : IProductStockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductStockService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        /// Create ProductStock Service
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResponse<int> CreateProductStock(CreateProductStockRequestModel model)
        {
            using (var context = _unitOfWork.Create())
            {
                var product = context.Repositories.ProductRepository.Get(model.ProductID);
                if (product == null)
                    return ApiResponse<int>.ErrorResponse("Product does not exists.");

                var supplier = context.Repositories.SupplierRepository.Get(model.SupplierID);
                if (supplier == null)
                    return ApiResponse<int>.ErrorResponse("Supplier does not exists.");

                var warehouse = context.Repositories.WareHouseRepository.Get(model.WareHouseID);
                if (warehouse == null)
                    return ApiResponse<int>.ErrorResponse("WareHouse does not exists.");

                var codeOld = context.Repositories.CommonRepository.GetCodeGenerate(Parameters.tables["ProductStock"].TableName, Parameters.tables["ProductStock"].ColumnName);
                var modelMap = _mapper.Map<CreateProductStockRepositoryRequestModel>(model);
                modelMap.ProductStockID = Guid.NewGuid();
                modelMap.ProductStockCode = GenerateCode.GenCode(codeOld);

                var result = context.Repositories.ProductStockRepository.Create(modelMap);
                if(result <= 0)
                    return ApiResponse<int>.ErrorResponse("Create ProductStock fail.");
                context.SaveChanges();

                return ApiResponse<int>.SuccessResponse(result);
            }
        }

        /// <summary>
        /// Update table ProductStock (Hold Product)
        /// SangNguyen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResponse<int> HoldProduct(HoldProductRequestModel model)
        {
            using (var contex = _unitOfWork.Create())
            {
                var result = contex.Repositories.ProductStockRepository.HoldProduct(model);
                if (result < 1)
                    return ApiResponse<int>.ErrorResponse("Hold Product fail.");

                contex.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }
    }
}
