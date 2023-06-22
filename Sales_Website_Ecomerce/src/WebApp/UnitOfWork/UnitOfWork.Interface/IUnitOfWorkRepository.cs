using Repository.Interface;

namespace UnitOfWork.Interface
{
    public interface IUnitOfWorkRepository
    {
        IProductRepository ProductRepository { get; }
        IAccountRepository AccountRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICartRepository CartRepository { get; }
        ITenantRepository TenantRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        IWareHouseRepository WareHouseRepository { get; }
        IProductStockRepository ProductStockRepository { get; }
        IOrderRepository OrderRepository { get; }
        INotificationRepository NotificationRepository { get; }
        ICommonRepository CommonRepository { get; }
        //IJobScheduleRepository JobScheduleRepository { get; }

    }
}
