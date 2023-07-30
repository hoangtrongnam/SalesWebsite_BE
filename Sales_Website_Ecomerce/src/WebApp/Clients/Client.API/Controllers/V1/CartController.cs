using Microsoft.AspNetCore.Mvc;
using Models.RequestModel;
using Models.RequestModel.Cart;
using Models.ResponseModels;
using Services;
using System.ComponentModel.DataAnnotations;

namespace Client.API.Controllers
{ 
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartService;

        public CartController(ICartServices cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// hiển thị cart theo mã KH
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("FindCart/{customerId}")]
        public ActionResult<ProductResponeModel> Get([Required] Guid customerId, [Required] int pageIndex)
        {
            return Ok(_cartService.Get(customerId, pageIndex));
        }

        ////[HttpGet("/GetListProduct")]
        ////public ActionResult<ProductResponeModel> GetALL([Required] int pageIndex)
        ////{
        ////    return Ok(_productService.GetAll(pageIndex));
        ////}

        /// <summary>
        /// thêm sản phẩm vào giỏ hàng (nếu KH đã có giỏ hàng sẽ hiển thị thông báo, nếu KH đồng ý gọi API: AddProductExisted)
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        [HttpPost("AddCart")]
        public ActionResult AddCart([FromBody][Required] CartRequestModel cart)
        {
            return Ok(_cartService.Create(cart));
        }

        /// <summary>
        /// thêm sản phẩm khi KH đã có giỏ hàng
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        [HttpPost("AddProductExisted")]
        public ActionResult AddProductExisted([FromBody][Required] CartRequestModel cart)
        {
            return Ok(_cartService.AddProductExisted(cart));
        }

        /// <summary>
        /// UpdateCart
        /// </summary>
        /// <remarks>
        /// Update:
        /// + chỉ xóa 1 cartproduct từ cart và k xóa cart (xóa thì gọi API: DeleteCart)
        /// + update số lượng
        /// </remarks>
        /// <param name="item"></param>
        /// <param name="cartID"></param>
        /// <returns></returns>
        [HttpPut("UpdateCart")]
        public ActionResult UpdateCart([FromBody][Required] CartRequestModel item, [Required] Guid cartId)
        {
            return Ok(_cartService.Update(item, cartId));
        }

        /// <summary>
        /// DeleteCart (update cart và cartproduct)
        /// </summary>
        /// <param name="cartID"></param>
        /// <returns></returns>
        [HttpDelete("DeleteCart")]
        public ActionResult DeleteCart([Required] Guid cartId)
        {
            return Ok(_cartService.Delete(cartId));
        }
    }
}
