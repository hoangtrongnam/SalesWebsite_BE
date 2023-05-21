using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IOrderRepository : IReadRepository<OrderResponseModel, int>, ICreateRepository<OrderRequestModel>, IUpdateRepository<OrderRequestModel, int>, IRemoveRepository<OrderRequestModel, int>
    {
        OrderResponseModel GetLstOrder(int item);//hỗ trợ hủy đơn hàng đã hết hạng
    }
}
