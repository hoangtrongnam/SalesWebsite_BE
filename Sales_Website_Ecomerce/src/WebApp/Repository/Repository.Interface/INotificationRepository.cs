using Models.RequestModel;

namespace Repository.Interface
{
    public interface INotificationRepository //: ICreateRepository<NotificationRequestModel>, IReadRepository<NotificationResponseModel, int>, IUpdateRepository<NotificationRequestModel, int>
    {
        int Create(NotificationRequestModel item, Guid notificationID);
        //NotificationResponseModel Get(int role, int pageIndex);
        //int Update(NotificationRequestModel item, int notificationID);
    }
}