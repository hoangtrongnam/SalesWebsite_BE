using Repository.Implement;
using Repository.Interface;
using System.Data.SqlClient;
using UnitOfWork.Interface;

namespace UnitOfWork.Database
{
    public class UnitOfWorkDatabaseRepository : IUnitOfWorkRepository
    {
        public IProductRepository ProductRepository { get; }
        public IAccountRepository AccountRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public ICartRepository CartRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IJobScheduleRepository JobScheduleRepository { get; }
        public UnitOfWorkDatabaseRepository(SqlConnection context, SqlTransaction transaction)
        {          
            ProductRepository = new ProductRepository(context, transaction);
            AccountRepository = new AccountRepository(context, transaction);
            CategoryRepository = new CategoryRepository(context, transaction);
            CartRepository = new CartRepository(context, transaction);
            OrderRepository = new OrderRepository(context, transaction);
            NotificationRepository = new NotificationRepository(context, transaction);
            JobScheduleRepository = new JobScheduleRepository(context, transaction);
        }
    }
}
