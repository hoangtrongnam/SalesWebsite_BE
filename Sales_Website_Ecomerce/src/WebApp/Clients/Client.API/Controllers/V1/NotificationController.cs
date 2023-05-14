using Client.API.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModel;
using Models.ResponseModels;
using Services;
using System.ComponentModel.DataAnnotations;

namespace Client.API.Controllers.V1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiVersion("1.0")]
    [CustomException]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationServices _notificationService;

        public NotificationController(NotificationServices notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("CreateNotification")]
        public ActionResult AddNotification([FromBody] NotificationRequestModel item)
        {
            return Ok(_notificationService.Create(item));
        }

        [HttpGet("GetNotificationById")]
        public ActionResult<NotificationResponseModel> Get([Required] int role, [Required] int pageIndex)
        {
            //Note: 1: sale, 2: accountant, 3: warehouse staff, 4: customer
            return Ok(_notificationService.Get(role, pageIndex));
        }

        [HttpPut("UpdateNotification")]
        public ActionResult UpdatetNotification([Required] int role, [Required] int notificationID)
        {
            //Note: 1: sale, 2: accountant, 3: warehouse staff, 4: customer
            NotificationRequestModel model = new NotificationRequestModel();
            if (role == 1) model.Status = 21;
            else if (role == 2) model.Status = 23;
            else if (role == 3) model.Status = 24;
            else if (role == 4) model.Status = 25;
            return Ok(_notificationService.Update(model, notificationID));
        }
    }
}
