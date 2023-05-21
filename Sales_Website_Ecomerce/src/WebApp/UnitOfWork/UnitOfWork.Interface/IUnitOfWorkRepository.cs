using Repository.Interface;

namespace UnitOfWork.Interface
{
    public interface IUnitOfWorkRepository
    {
        IProductRepository ProductRepository { get; }
        IAccountRepository AccountRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICartRepository CartRepository { get; }
        IOrderRepository OrderRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IJobScheduleRepository JobScheduleRepository { get; }
    }
}
