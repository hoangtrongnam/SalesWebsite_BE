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
        public ActionResult<ProductResponeModel> Get([Required] int CustomerID, [Required] int pageIndex)
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

        //[HttpPut("/UpdateCart")]
        //public ActionResult UpdateCart([FromBody][Required] CartRequestModel item, [Required] int cartID)
        //{
        //    return Ok(_cartService.Update(item, cartID));
        //}

        [HttpDelete("/DeleteCart")]
        public ActionResult DeleteCart([Required] int cartID)
        {
            return Ok(_cartService.Delete(cartID));
        }
    }
}
