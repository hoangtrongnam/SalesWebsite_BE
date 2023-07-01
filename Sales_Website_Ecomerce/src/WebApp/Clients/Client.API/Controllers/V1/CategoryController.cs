using Client.API.Exceptions;
using Client.API.Utils;
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
        private readonly RequestUtils _requestUtils;

        public CategoryController(ICategoryServices categoryServices, RequestUtils requestUtils)
        {
            _categoryService = categoryServices;
            _requestUtils = requestUtils;
        }

        [HttpGet("GetCategoryByID")]
        public async Task<ActionResult> GetCategoryByID([Required] Guid id)
        {
            var result = _categoryService.GetCategoryByID(id);
            return Ok(result);
        }
        [HttpGet("GetAllCategory")]
        public async Task<ActionResult> GetAllCategory()
        {
            var tenantId = _requestUtils.GetTenantId();
            var result = _categoryService.GetAllCategory(Guid.Parse(tenantId));
            return Ok(result);
        }
        [HttpGet("GetChildCategory")]
        public async Task<ActionResult> GetChildCategory([Required] Guid id)
        {
            var result = _categoryService.GetChildCategoryByCategoyId(id);
            return Ok(result);
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryRequestModel model)
        {
            var tenantId = _requestUtils.GetTenantId();
            var result = _categoryService.CreateCategory(model, Guid.Parse(tenantId));
            return Ok(result);
        }

        [HttpPut("UpdateCategory")]
        public async Task<ActionResult> UpdateCategory([FromBody] UpdateCategoryRequestModel model, [Required] Guid id)
        {
            var result = _categoryService.UpdateCategoryByID(model, id);
            return Ok(result);
        }

        [HttpDelete("RemoveCategory")]
        public async Task<ActionResult> RemoveCategory([Required] Guid id)
        {
            var result = _categoryService.RemoveCategoryByID(id);
            return Ok(result);
        }
        
        [HttpGet("GetStatus")]
        public async Task<ActionResult> GetStatus([Required] string key)
        {
            var result = _categoryService.GetStatus(key);
            return Ok(result);
        }

    }
}
