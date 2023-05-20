using Client.API.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModel;
using Models.ResponseModels;
using Services;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

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
           
            switch (role)
            {
                case 1:
                    model.Status = 21;
                    break;
                case 2:
                    model.Status = 23;
                    break;
                case 3:
                    model.Status = 25;
                    break;
                case 4:
                    model.Status = 27;
                    break;
            }

            return Ok(_notificationService.Update(model, notificationID));
        }
    }
}
