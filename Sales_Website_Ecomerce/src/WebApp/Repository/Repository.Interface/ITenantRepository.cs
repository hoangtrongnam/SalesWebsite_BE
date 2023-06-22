using Models.ResponseModels.Tenant;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface ITenantRepository: IReadRepository<TenantResponseModel, Guid>
    {
    }
}
