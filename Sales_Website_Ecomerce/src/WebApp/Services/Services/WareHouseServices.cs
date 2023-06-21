using AutoMapper;
using Common;
using Models.RequestModel.Supplier;
using Models.RequestModel.WareHouse;
using Models.ResponseModels.WareHouse;
using UnitOfWork.Interface;

namespace Services
{
    public interface IWareHouseServices
    {
        ApiResponse<int> CreateWareHouse(CreateWareHouseRequestModel model);
        ApiResponse<WareHouseResponseModel> GetWareHouseByID(Guid id);
    }
    public class WareHouseServices : IWareHouseServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WareHouseServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                var codeOld = context.Repositories.CommonRepository.GetCodeGenerate(Parameters.tables["WareHouse"].TableName, Parameters.tables["WareHouse"].ColumnName);

                var modelMap = _mapper.Map<CreateWareHouseRepositoryRequestModel>(model);
                modelMap.WareHouseID = Guid.NewGuid();
                modelMap.WareHouseCode = GenerateCode.GenCode(codeOld);

                var result = context.Repositories.WareHouseRepository.Create(modelMap);
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
        public ApiResponse<WareHouseResponseModel> GetWareHouseByID(Guid id)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.WareHouseRepository.Get(id);
                return ApiResponse<WareHouseResponseModel>.SuccessResponse(result);
            }
        }
    }
}
