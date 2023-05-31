using Common;
using Models.RequestModel.WareHouse;
using Models.ResponseModels.Supplier;
using Models.ResponseModels.WareHouse;
using System.Reflection;
using UnitOfWork.Interface;

namespace Services
{
    public interface IWareHouseServices
    {
        ApiResponse<int> CreateWareHouse(CreateWareHouseRequestModel model);
        ApiResponse<WareHouseResponseModel> GetWareHouseByID(int id);
    }
    public class WareHouseServices : IWareHouseServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public WareHouseServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Create WareHouse
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResponse<int> CreateWareHouse(CreateWareHouseRequestModel model)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.WareHouseRepository.Create(model);
                if (result <= 0)
                    return ApiResponse<int>.ErrorResponse("Create WareHouse Fail.");
                context.SaveChanges();
                return ApiResponse<int>.SuccessResponse(result);
            }
        }
        /// <summary>
        /// Get WareHouse by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApiResponse<WareHouseResponseModel> GetWareHouseByID(int id)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.WareHouseRepository.Get(id);
                return ApiResponse<WareHouseResponseModel>.SuccessResponse(result);
            }
        }
    }
}
