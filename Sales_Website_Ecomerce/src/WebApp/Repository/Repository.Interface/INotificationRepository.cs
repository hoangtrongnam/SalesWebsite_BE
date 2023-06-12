using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface INotificationRepository //: ICreateRepository<NotificationRequestModel>, IReadRepository<NotificationResponseModel, int>, IUpdateRepository<NotificationRequestModel, int>
    {
        int Create(NotificationRequestModel item);
        //NotificationResponseModel Get(int role, int pageIndex);
        //int Update(NotificationRequestModel item, int notificationID);
    }
}