using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
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
        private readonly ICommonService _commonService;
        public ProductController(IProductServices productService, ICommonService commonService)
        {
            _productService = productService;
            _commonService = commonService;
        }
        [HttpPost("CreateProduct")]
        public async Task<ActionResult> CreateProduct([FromBody] CreateOnlyProductRequestModel model,[FromHeader] Guid tenantID)
        {
            var result = _productService.CreateProduct(model, tenantID);
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
        public async Task<ActionResult> GetProductById([Required] Guid id)
        {
            var result = _productService.GetProductByID(id);
            return Ok(result);
        }

        [HttpGet("GetImagesByProductId")]
        public async Task<ActionResult> GetImagesByProductId([Required] Guid productId)
        {
            var result = _productService.GetImagesByProductID(productId);
            return Ok(result);
        }

        [HttpGet("GetPricesByProductId")]
        public async Task<ActionResult> GetPricesByProductId([Required] Guid productId)
        {
            var result = _productService.GetPricesByProductID(productId);
            return Ok(result);
        }

        [HttpGet("GetProductByCategory")]
        public async Task<ActionResult> GetProductByCategory([Required] Guid categoryId)
        {
            var result = _productService.GetProductByCategory(categoryId);
            return Ok(result);
        }

        [HttpPut("UpdateProduct")]
        public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductRequestModel model, [Required] Guid id)
        {
            var result = _productService.UpdateProduct(model, id);
            return Ok(result);
        }

        [HttpPost("upload")]
        public async Task<string> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required");
            }

            string rootPath = _commonService.GetConfigValueService((int)Common.Enum.ConfigKey.KeyPath);

            string uploadPath = Path.Combine(rootPath, "uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath.Replace(rootPath,"");
        }
    }
}
