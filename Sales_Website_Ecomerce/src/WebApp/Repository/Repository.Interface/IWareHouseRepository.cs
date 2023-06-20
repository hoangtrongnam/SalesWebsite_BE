using Models.RequestModel.WareHouse;
using Models.ResponseModels.WareHouse;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IWareHouseRepository: ICreateRepository<CreateWareHouseRepositoryRequestModel, int>, IReadRepository<WareHouseResponseModel,Guid>
    {
    }
}
