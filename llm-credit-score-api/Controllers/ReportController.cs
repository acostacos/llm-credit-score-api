using llm_credit_score_api.Messages;
using llm_credit_score_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace llm_credit_score_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("get")]
        public IActionResult GetReport([FromQuery] GetReportRequest request)
        {
            var response = _reportService.GetReport(request);
            if (response.Exception != null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateReport([FromBody] GenerateReportRequest request)
        {
            var response = await _reportService.GenerateReport(request);
            if (response.Exception != null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}

