using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IJobScheduleRepository : ICreateRepository<JobRequestModel>
    {
    }
}
