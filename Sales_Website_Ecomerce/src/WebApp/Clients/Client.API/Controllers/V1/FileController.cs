using Common;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModel.Product;
using Services;

namespace Client.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiVersion("1.0")]
    public class FileController : ControllerBase
    {
        private readonly ICommonService _commonService;
        private readonly IAttributeProductService _atributeProductService;


        public FileController(ICommonService commonService, IAttributeProductService atributeProductService)
        {
            _commonService = commonService;
            _atributeProductService = atributeProductService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            if (files.Count == 0 || files == null)
            {
                return Ok(ApiResponse<List<string>>.ErrorResponse("File is required"));
            }
            var listPath = new List<string>();

            foreach (var item in files)
            {
                string rootPath = _commonService.GetConfigValueService((int)Common.Enum.ConfigKey.KeyPath);

                string uploadPath = Path.Combine(rootPath, "uploads");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string fileName = $"{Guid.NewGuid()}_{item.FileName}";
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                }
                listPath.Add(filePath.Replace(rootPath, "").Replace("\\", "/"));
                //
                var imageModel = new ImageRequestModel();
                imageModel.SortOrder = 0;
                imageModel.Name = fileName;
                imageModel.Url = filePath;
                imageModel.Description = fileName;
                imageModel.Type = item.ContentType;
                _atributeProductService.CreateSingleImage(imageModel);
            }

            return Ok(ApiResponse<List<string>>.SuccessResponse(listPath));
        }
    }
}
