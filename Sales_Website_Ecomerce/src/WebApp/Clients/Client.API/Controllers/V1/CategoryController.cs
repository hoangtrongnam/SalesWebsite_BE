using Microsoft.AspNetCore.Mvc;
using Models.RequestModel.Category;
using Services;
using System.ComponentModel.DataAnnotations;

namespace Client.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryService;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryService = categoryServices;
        }

        [HttpGet("GetCategoryByCondition")]
        public async Task<ActionResult> GetCategoryByCondition([FromQuery] GetCategoryByID_ParentTenantRequestModel model)
        {
            var result = _categoryService.GetCategoryByID(model);
            return Ok(result);
        }

        [HttpGet("GetCategotyTenantParent")]
        public async Task<ActionResult> GetCategotyTenantParent([Required] int TenantID, [Required] int Parent)
        {
            var result = _categoryService.GetCategoryByTenantParent(TenantID, Parent);
            return Ok(result);
        }
        
        [HttpPost("CreateCategory")]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryRequestModel model)
        {
            var result = _categoryService.CreateCategory(model);
            return Ok(result);
        }
    }
}
