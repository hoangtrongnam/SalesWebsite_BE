using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IAccountRepository: ICreateRepository<UserRegisterRequestModel, int>, IUpdateRepository<UpdateUserCommonRequestModel, int>, IReadRepository<UserResponseModel, string>
    {
    }
}
