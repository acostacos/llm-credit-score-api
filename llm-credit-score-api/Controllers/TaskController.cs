using llm_credit_score_api.Messages;
using llm_credit_score_api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace llm_credit_score_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("get")]
        public IActionResult GetTask([FromQuery] GetTaskRequest request)
        {
            var response = _taskService.GetTask(request);
            if (response.Exception != null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
