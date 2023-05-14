using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface INotificationRepository : ICreateRepository<NotificationRequestModel>, IReadRepository<NotificationResponseModel, int>, IUpdateRepository<NotificationRequestModel, int>
    {

    }
}
