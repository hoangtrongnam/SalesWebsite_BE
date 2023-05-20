using Microsoft.AspNetCore.Mvc;
using Models.RequestModel;
using Services;
using System.ComponentModel.DataAnnotations;

namespace Client.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountService;

        public AccountController(IAccountServices accountServices)
        {
            _accountService = accountServices;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInRequestModel model)
        {
            var result = _accountService.SignIn(model);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }
            return Ok(result);
        }
        [HttpPut("UpdateInfoUser")]
        public async Task<IActionResult> UpdateInfoUser(UpdateInfoUserRequestModel model, [Required]int Id)
        {
            var result = _accountService.UpdateInfoUser(model,Id);
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterRequestModel model)
        {
            var result = _accountService.UserRegister(model);
            return Ok(result);
        }

        [HttpGet("GetUserByUserName")]
        public ActionResult GetUserByUserName([Required] string UserName)
        {
            return Ok(_accountService.GetUserByUserName(UserName));
        }

        [HttpPost("ChangePassword")]
        public ActionResult ChangePassword(ChangePasswordRequestModel model)
        {
            return Ok(_accountService.ChangePassword(model));
        }
    }
}
