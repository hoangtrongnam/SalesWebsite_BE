using Models.RequestModel;
using Repository.Interface.Actions;

namespace Repository.Interface
{
    public interface IAccountRepository: ISignInRepository<SignInRequestModel,bool>
    {
    }
}
