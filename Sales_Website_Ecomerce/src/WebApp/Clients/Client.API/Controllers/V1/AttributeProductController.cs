using Common;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModel.AtributeProduct;
using Models.RequestModel.Product;
using Models.ResponseModels.AtributeProduct;
using Models.ResponseModels.AttributeProduct;
using Models.ResponseModels.Product;
using Services;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;

namespace Client.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiVersion("1.0")]
    public class AttributeProductController : ControllerBase
    {
        private readonly IAttributeProductService _atributeProductService;

        public AttributeProductController(IAttributeProductService atributeProductService)
        {
            _atributeProductService = atributeProductService;
        }
        
        [HttpPost("CreateImages")]
        public async Task<ActionResult> CreateImages([FromBody] List<ImageRequestModel> model)
        {
            var result = _atributeProductService.CreateImages(model);
            return Ok(result);
        }
        [HttpPost("CreateColor")]
        public async Task<ActionResult> CreateColor([FromBody] ColorRepositoryRequestModel model)
        {
            var result = _atributeProductService.CreateColor(model);
            return Ok(result);
        }
        [HttpPost("CreateSize")]
        public async Task<ActionResult> CreateSize([FromBody] SizeRepositoryRequestModel model)
        {
            var result = _atributeProductService.CreateSize(model);
            return Ok(result);
        }
        [HttpGet("GetAllImages")]
        [ProducesResponseType(typeof(ApiResponse<List<ImageResponseModel>>), 200)]
        public async Task<ActionResult> GetAllImages()
        {
            var result = _atributeProductService.GetAllImages();
            return Ok(result);
        }
        [HttpGet("GetColors")]
        [ProducesResponseType(typeof(ApiResponse<List<ColorResponseModel>>), 200)]
        public async Task<ActionResult> GetColors()
        {
            var result = _atributeProductService.GetColors();
            return Ok(result);
        }
        [HttpGet("GetSizes")]
        [ProducesResponseType(typeof(ApiResponse<List<SizeResponseModel>>), 200)]
        public async Task<ActionResult> GetSizes()
        {
            var result = _atributeProductService.GetSizes();
            return Ok(result);
        }
        [HttpGet("GetColorSizeProduct/{productId}")]
        [ProducesResponseType(typeof(ApiResponse<ColorSizeResponseModel>), 200)]
        public async Task<ActionResult> GetColorSizeProduct([Required] Guid productId)
        {
            var result = _atributeProductService.GetColorSizeProduct(productId);
            return Ok(result);
        }
        [HttpGet("GetImageByColor/{productId}/{colorId}")]
        [ProducesResponseType(typeof(ApiResponse<List<ImageByColorResponseModel>>), 200)]
        public async Task<ActionResult> GetColorSizeProduct([Required] Guid productId, [Required] Guid colorId)
        {
            var result = _atributeProductService.GetImageByColor(productId, colorId);
            return Ok(result);
        }
    }
}
