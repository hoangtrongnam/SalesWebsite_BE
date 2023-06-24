using Microsoft.AspNetCore.Mvc;
using Models.RequestModel;
using Models.RequestModel.Cart;
using Models.ResponseModels;
using Services;
using System.ComponentModel.DataAnnotations;

namespace Client.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartService;

        public CartController(ICartServices cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("/FindCart/{CustomerID}")]
        public ActionResult<ProductResponeModel> Get([Required] Guid CustomerID, [Required] int pageIndex)
        {
            return Ok(_cartService.Get(CustomerID, pageIndex));
        }

        ////[HttpGet("/GetListProduct")]
        ////public ActionResult<ProductResponeModel> GetALL([Required] int pageIndex)
        ////{
        ////    return Ok(_productService.GetAll(pageIndex));
        ////}

        [HttpPost("/AddCart")]
        public ActionResult AddCart([FromBody][Required] CartRequestModel cart)
        {
            return Ok(_cartService.Create(cart));
        }

        [HttpPost("/AddProductExisted")]// khi giỏ hang đã có product
        public ActionResult AddProductExisted([FromBody][Required] CartRequestModel cart)
        {
            return Ok(_cartService.AddProductExisted(cart));
        }

        //update 
            //+ chỉ xóa 1 cartproduct từ cart và k xóa cart
            //+ update số lượng
        [HttpPut("/UpdateCart")]
        public ActionResult UpdateCart([FromBody][Required] CartRequestModel item, [Required] Guid cartID)
        {
            return Ok(_cartService.Update(item, cartID));
        }
        //xóa là update cart và cartproduct
        [HttpDelete("/DeleteCart")]
        public ActionResult DeleteCart([Required] Guid cartID)
        {
            return Ok(_cartService.Delete(cartID));
        }
    }
}
