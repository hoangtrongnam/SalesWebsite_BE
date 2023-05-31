using Microsoft.AspNetCore.Mvc;
using Models.RequestModel.WareHouse;
using Services;
using System.ComponentModel.DataAnnotations;

namespace Client.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiVersion("1.0")]
    public class WareHouseController : ControllerBase
    {
        private readonly IWareHouseServices _wareHouseServices;
        public WareHouseController(IWareHouseServices wareHouseServices)
        {
            _wareHouseServices = wareHouseServices;
        }

        [HttpGet("GetWareHouseById")]
        public async Task<ActionResult> GetWareHouseById([Required] int id)
        {
            var result = _wareHouseServices.GetWareHouseByID(id);
            return Ok(result);
        }
        [HttpPost("CreateWareHouse")]
        public async Task<ActionResult> CreateWareHouse([FromBody] CreateWareHouseRequestModel model)
        {
            var result = _wareHouseServices.CreateWareHouse(model);
            return Ok(result);
        }
    }
}
