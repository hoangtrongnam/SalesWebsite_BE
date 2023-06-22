using AutoMapper;
using Common;
using Models.RequestModel.Supplier;
using Models.ResponseModels.Supplier;
using UnitOfWork.Interface;

namespace Services
{
    public interface ISupplierServices
    {
        ApiResponse<int> CreateSupplier(CreateSupplierRequestModel model);
        ApiResponse<SupplierResponseModel> GetSupplierByID(Guid id);
    }
    public class SupplierServices : ISupplierServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SupplierServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                var codeOld = context.Repositories.CommonRepository.GetCodeGenerate(Parameters.tables["Supplier"].TableName, Parameters.tables["Supplier"].ColumnName);

                var modelMap = _mapper.Map<CreateSupplierRepositoryRequestModel>(model);
                modelMap.SupplierID = Guid.NewGuid();
                modelMap.SupplierCode = GenerateCode.GenCode(codeOld);

                var result = context.Repositories.SupplierRepository.Create(modelMap);
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
        public ApiResponse<SupplierResponseModel> GetSupplierByID(Guid id)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.SupplierRepository.Get(id);
                return ApiResponse<SupplierResponseModel>.SuccessResponse(result);
            }
        }
    }
}
