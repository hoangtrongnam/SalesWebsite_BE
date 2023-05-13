using Microsoft.AspNetCore.Mvc;
using Models.RequestModel;
using Models.ResponseModels;
using Services;
using System.ComponentModel.DataAnnotations;

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

        //[HttpGet("/FindCategory/{id}")]
        //public ActionResult Get([Required] int id)
        //{
        //    return Ok(_categoryService.Get(id));
        //}

        //[HttpGet("/GetListCategory")]
        //public ActionResult GetALL()
        //{
        //    return Ok(_categoryService.GetAll());
        //}

        [HttpPost("/AddOrder")]
        public ActionResult AddProduct([FromBody] OrderRequestModel cate)
        {
            return Ok(_orderService.Create(cate));
        }

        //[HttpPut("/UpdateCategory")]
        //public ActionResult UpdateProduct([FromBody] CategoryRequestModel cate, [Required] int CategoryID)
        //{
        //    return Ok(_categoryService.Update(cate, CategoryID));
        //}

        //[HttpDelete("/DeleteCategory")]
        //public ActionResult DeleteProduct([Required] int id)
        //{
        //    return Ok(_categoryService.Delete(id));
        //}
    }
}
