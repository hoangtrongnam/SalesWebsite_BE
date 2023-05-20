using Models.ResponseModels.Tenant;
using Repository.Interfaces.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ITenantRepository: IReadRepository<TenantResponseModel, int>
    {
    }
}
