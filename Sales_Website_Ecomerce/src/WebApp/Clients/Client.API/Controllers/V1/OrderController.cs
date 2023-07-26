using Microsoft.AspNetCore.Mvc;
using Models.RequestModel.Orders;
using Services;
using System.ComponentModel.DataAnnotations;

namespace Client.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class OrderController : ControllerBase
    {
        private readonly OrderServices _orderService;

        public OrderController(OrderServices orderServices)
        {
            _orderService = orderServices;
        }

        [HttpGet]
        public ActionResult Get([Required] Guid orderId)
        {
            return Ok(_orderService.Get(orderId));
        }

        [HttpPost("AddOrder")]
        public ActionResult AddOrder([FromBody] OrderRequestModel item)
        {
            return Ok(_orderService.Create(item));
        }

        [HttpPut("UpdateOrder")]
        public ActionResult UpdateOrder([FromBody] OrderCommonRequest item, [Required] Guid OrderId)
        {
            //Sẽ làm update thông tin product trong giỏ hàng sau
            /*
            10	Có dơn hàng mới cần xác nhận
            11	Sale xác nhận tiền cọc và đợn hàng thành công
            12	Sale không liên hệ được với KH
            13	số tiền cọc không đúng (kế toán)
            14	Kế toán xác nhận đủ tiền cọc
             	Sale hủy Đơn hàng 
             */

            return Ok(_orderService.Update(item, OrderId));
        }

        [HttpDelete("DeleteOrder")]
        public ActionResult DeleteOrder([Required] Guid orderId, [Required] Guid customerId)
        {
            //OrderResponseModel orderResponseModel = _orderService.GetlistProduct(OrderID);
            //OrderRequestModel orderRequestModel = new OrderRequestModel();
            //var lstProduct = new List<ProductModel>();
            //foreach (var item in orderResponseModel.lstProduct)
            //{
            //    var product = new ProductModel();
            //    product.ProductID = item.ProductID;
            //    product.Quantity = item.Quantity;
            //    lstProduct.Add(product);
            //}
            //orderRequestModel.lstProduct = lstProduct;
            return Ok(_orderService.Delete(orderId, customerId));
        }
    }
}