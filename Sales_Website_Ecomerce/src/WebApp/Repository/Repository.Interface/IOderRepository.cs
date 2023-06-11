using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interfaces.Actions;

namespace Repository.Interface
{
    public interface IOrderRepository //: IReadRepository<OrderResponseModel, int>, ICreateRepository<OrderRequestModel>, IUpdateRepository<OrderRequestModel, int>, IRemoveRepository<OrderRequestModel, int>
    {
        OrderResponseModel Get(int orderID);
        OrderResponseModel GetOrderInfo(int orderID);
        OrderResponseModel GetLstOrder(int Status); //hỗ trợ hủy đơn hàng đã hết hạng
        OrderResponseModel GetOrderDetail(int orderID, out decimal totalPayment);
        int Create(OrderRequestModel item);
        int Remove(int OrderID);
        int Update(OrderRequestModel item, int orderID);
    }
}