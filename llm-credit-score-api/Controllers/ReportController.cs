using llm_credit_score_api.Messages;
using llm_credit_score_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace llm_credit_score_api.Controllers
{
    [Route("api/report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private IViewReportService _viewReportService;
        private IGenerateReportService _generateReportService;

        public ReportController(IViewReportService viewReportService, IGenerateReportService generateReportService)
        {
            _viewReportService = viewReportService;
            _generateReportService = generateReportService;
        }

        [HttpGet("view")]
        public IActionResult ViewReports()
        {
            var response = _viewReportService.ViewReports();
            if (response.Exception != null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("generate")]
        public IActionResult GenerateReport([FromBody] GenerateReportRequest request)
        {
            var response = _generateReportService.GenerateReport(request);
            if (response.Exception != null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}

