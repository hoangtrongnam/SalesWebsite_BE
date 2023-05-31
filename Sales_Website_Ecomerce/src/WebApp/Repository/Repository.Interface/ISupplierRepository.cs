using Models.RequestModel.Supplier;
using Models.ResponseModels.Supplier;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface ISupplierRepository: ICreateRepository<CreateSupplierRequestModel, int>, IReadRepository<SupplierResponseModel,int>
    {
    }
}
