using Common;
using Models.RequestModel.Supplier;
using Models.ResponseModels.Supplier;
using Models.ResponseModels.WareHouse;
using UnitOfWork.Interface;

namespace Services
{
    public interface ISupplierServices
    {
        ApiResponse<int> CreateSupplier(CreateSupplierRequestModel model);
        ApiResponse<SupplierResponseModel> GetSupplierByID(int id);
    }
    public class SupplierServices : ISupplierServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public SupplierServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Create Supplier
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResponse<int> CreateSupplier(CreateSupplierRequestModel model)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.SupplierRepository.Create(model);
                if (result <= 0)
                    return ApiResponse<int>.ErrorResponse("Create Supplier Fail.");
                context.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Get Supplier by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApiResponse<SupplierResponseModel> GetSupplierByID(int id)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.SupplierRepository.Get(id);
                return ApiResponse<SupplierResponseModel>.SuccessResponse(result);
            }
        }
    }
}
