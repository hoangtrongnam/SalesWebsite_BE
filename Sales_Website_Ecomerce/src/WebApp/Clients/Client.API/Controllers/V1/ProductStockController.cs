﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModel.ProductStock;
using Models.RequestModel.WareHouse;
using Services;

namespace Client.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiVersion("1.0")]
    public class ProductStockController : ControllerBase
    {
        private readonly IProductStockService _productStockService;
        public ProductStockController(IProductStockService productStockService)
        {
            _productStockService = productStockService;
        }
        [HttpPost("CreateProductStock")]
        public async Task<ActionResult> CreateProductStock([FromBody] CreateProductStockRequestModel model)
        {
            var result = _productStockService.CreateProductStock(model);
            return Ok(result);
        }
    }
}