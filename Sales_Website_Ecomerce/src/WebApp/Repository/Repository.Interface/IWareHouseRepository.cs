using Models.RequestModel.WareHouse;
using Models.ResponseModels.WareHouse;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IWareHouseRepository: ICreateRepository<CreateWareHouseRequestModel,int>, IReadRepository<WareHouseResponseModel,int>
    {
    }
}
