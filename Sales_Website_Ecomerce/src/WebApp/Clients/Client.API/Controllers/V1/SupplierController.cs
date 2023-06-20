using Microsoft.AspNetCore.Mvc;
using Models.RequestModel.Supplier;
using Services;
using System.ComponentModel.DataAnnotations;

namespace Client.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiVersion("1.0")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierServices _supplierServices;
        public SupplierController(ISupplierServices supplierServices)
        {
            _supplierServices = supplierServices;
        }

        [HttpGet("GetSupplierById")]
        public async Task<ActionResult> GetSupplierById([Required] Guid id)
        {
            var result = _supplierServices.GetSupplierByID(id);
            return Ok(result);
        }
        [HttpPost("CreateSupplier")]
        public async Task<ActionResult> CreateSupplier([FromBody] CreateSupplierRequestModel model)
        {
            var result = _supplierServices.CreateSupplier(model);
            return Ok(result);
        }
    }
}
