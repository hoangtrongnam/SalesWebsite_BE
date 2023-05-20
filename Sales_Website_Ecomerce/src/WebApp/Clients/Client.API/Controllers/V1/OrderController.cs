﻿using Microsoft.AspNetCore.Mvc;
using Models.RequestModel;
using Models.ResponseModels;
using Services;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Client.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderServices _orderService;

        public OrderController(OrderServices orderServices)
        {
            _orderService = orderServices;
        }

        [HttpGet("/FindOrder/{OrderID}")]
        public ActionResult Get([Required] int OrderID)
        {
            return Ok(_orderService.Get(OrderID));
        }

        //[HttpGet("/GetListCategory")]
        //public ActionResult GetALL()
        //{
        //    return Ok(_categoryService.GetAll());
        //}

        [HttpPost("/AddOrder")]
        public ActionResult AddOrder([FromBody] OrderRequestModel item)
        {
            return Ok(_orderService.Create(item));
        }

        [HttpPut("/UpdateOrder")]
        public ActionResult UpdateOrder([FromBody] OrderRequestModel item, [Required] int OrderID)
        {
            /*
            10	Có dơn hàng mới cần xác nhận
            11	Sale xác nhận tiền cọc và đợn hàng thành công
            12	Sale không liên hệ được với KH
            13	số tiền cọc không đúng (kế toán)
            14	Kế toán xác nhận đủ tiền cọc
             	Sale hủy Đơn hàng 
             */

            return Ok(_orderService.Update(item, OrderID));
        }

        [HttpDelete("/DeleteOrder")]
        public ActionResult DeleteOrder([Required] int OrderID)
        {
            OrderResponseModel orderResponseModel = _orderService.GetlistProduct(OrderID);
            OrderRequestModel orderRequestModel = new OrderRequestModel();
            var lstProduct = new List<ProductModel>();
            foreach (var item in orderResponseModel.lstProduct)
            {
                var product = new ProductModel();
                product.ProductID = item.ProductID;
                product.Quantity = item.Quantity;
                lstProduct.Add(product);
            }
            orderRequestModel.lstProduct = lstProduct;
            return Ok(_orderService.Delete(orderRequestModel, OrderID));
        }
    }
}
