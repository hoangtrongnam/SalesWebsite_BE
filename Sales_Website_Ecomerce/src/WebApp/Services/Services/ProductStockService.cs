using Common;
using Models.RequestModel.ProductStock;
using UnitOfWork.Interface;

namespace Services
{
    public interface IProductStockService
    {
        ApiResponse<int> CreateProductStock(CreateProductStockRequestModel model);
    }
    public class ProductStockService : IProductStockService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductStockService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Create ProductStock Service
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResponse<int> CreateProductStock(CreateProductStockRequestModel model)
        {
            using (var contex = _unitOfWork.Create())
            {
                var product = contex.Repositories.ProductRepository.Get(model.ProductID);
                if (product == null)
                    return ApiResponse<int>.ErrorResponse("Product does not exists.");
                
                var supplier = contex.Repositories.SupplierRepository.Get(model.SupplierID);
                if (supplier == null)
                    return ApiResponse<int>.ErrorResponse("Supplier does not exists.");

                var warehouse = contex.Repositories.WareHouseRepository.Get(model.WareHouseID);
                if (warehouse == null)
                    return ApiResponse<int>.ErrorResponse("WareHouse does not exists.");

                var result = contex.Repositories.ProductStockRepository.Create(model);
                if(result <= 0)
                    return ApiResponse<int>.ErrorResponse("Create ProductStock fail.");
                contex.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }
    }
}
