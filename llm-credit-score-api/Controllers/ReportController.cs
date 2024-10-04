using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace llm_credit_score_api.Controllers
{
    [Route("api/report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [HttpGet("view")]
        public IEnumerable<string> ViewReport()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("view/{id}")]
        public string ViewReport(int id)
        {
            return "value";
        }

        [HttpPost]
        public void GenerateReport([FromBody] string value)
        {
        }
    }
}
