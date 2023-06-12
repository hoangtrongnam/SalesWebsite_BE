using Microsoft.AspNetCore.Mvc;
using Models.RequestModel.Product;
using Services;
using System.ComponentModel.DataAnnotations;

namespace Client.API.Controllers.V1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiVersion("1.0")]
    //[CustomException]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;

        public ProductController(IProductServices productService)
        {
            _productService = productService;
        }
        [HttpPost("CreateProduct")]
        public async Task<ActionResult> CreateProduct([FromBody] CreateOnlyProductRequestModel model)
        {
            var result = _productService.CreateProduct(model);
            return Ok(result);
        }

        [HttpPost("CreateImages")]
        public async Task<ActionResult> CreateImages([FromBody] List<ImageRequestModel> model)
        {
            var result = _productService.CreateImages(model);
            return Ok(result);
        }

        [HttpPost("CreatePrices")]
        public async Task<ActionResult> CreatePrices([FromBody] List<PriceRequestModel> model)
        {
            var result = _productService.CreatePrices(model);
            return Ok(result);
        }

        [HttpGet("GetProductById")]
        //[Authorize]
        public async Task<ActionResult> GetProductById([Required] int id)
        {
            var result = _productService.GetProductByID(id);
            return Ok(result);
        }

        [HttpGet("GetImagesByProductId")]
        public async Task<ActionResult> GetImagesByProductId([Required] int ProductId)
        {
            var result = _productService.GetImagesByProductID(ProductId);
            return Ok(result);
        }

        [HttpGet("GetPricesByProductId")]
        public async Task<ActionResult> GetPricesByProductId([Required] int ProductId)
        {
            var result = _productService.GetPricesByProductID(ProductId);
            return Ok(result);
        }

        [HttpGet("GetProductByCategory")]
        public async Task<ActionResult> GetProductByCategory([Required] int CategoryId)
        {
            var result = _productService.GetProductByCategory(CategoryId);
            return Ok(result);
        }

        [HttpPut("UpdateProduct")]
        public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductRequestModel model, [Required] int ProductId)
        {
            var result = _productService.UpdateProduct(model, ProductId);
            return Ok(result);
        }
    }
}
