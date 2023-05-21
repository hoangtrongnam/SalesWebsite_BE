using Microsoft.AspNetCore.Mvc;
using Services;
using System.ComponentModel.DataAnnotations;

namespace Client.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobScheduleController : ControllerBase
    {
        private readonly JobScheduleServices _jobScheduleService;

        public JobScheduleController(JobScheduleServices jobScheduleService)
        {
            _jobScheduleService = jobScheduleService;
        }

        //[HttpGet("/FindOrder/{OrderID}")]
        //public ActionResult Get([Required] int OrderID)
        //{
        //    return Ok(_orderService.Get(OrderID));
        //}

        //[HttpGet("/GetListCategory")]
        //public ActionResult GetALL()
        //{
        //    return Ok(_categoryService.GetAll());
        //}

        [HttpPost("/RunJobShedule")]
        public async Task<IActionResult> RunJobShecdule([Required] string nameJob)
        {
            return Ok(_jobScheduleService.RunJob(nameJob));
        }

        [HttpDelete("/StopJobShecdule")]
        public ActionResult StopJobShecdule([Required] string nameJob)
        {
            return Ok(_jobScheduleService.StopJob(nameJob));
        }
    }
}
