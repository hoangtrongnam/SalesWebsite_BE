using Models.RequestModel.Orders;
using Models.ResponseModels;
using Models.ResponseModels.Product;

namespace Repository.Interface
{
    public interface IOrderRepository //: IReadRepository<OrderResponseModel, int>, ICreateRepository<OrderRequestModel>, IUpdateRepository<OrderRequestModel, int>, IRemoveRepository<OrderRequestModel, int>
    {
        OrderResponseModel Get(Guid orderID);
        OrderResponseModel GetOrderInfo(Guid orderID);
        //OrderResponseModel GetLstOrder(int Status); //hỗ trợ hủy đơn hàng đã hết hạng
        OrderResponseModel GetOrderDetail(Guid orderID);
        //int Create(OrderRequestModel item);
        //int Remove(int OrderID);
        int Update(OrderCommonRequest item, Guid orderID, decimal totalPayment);
        Guid InsertOrder(Guid customerID, int status, Guid ordreID);
        int InsertOrderProduct(List<ProductModel> lstProduct, Guid orderID);
        PriceResponseModel GetPromote(Guid PromoteID);
    }
}