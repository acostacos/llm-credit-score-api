using llm_credit_score_api.Constants;
using llm_credit_score_api.Messages;
using llm_credit_score_api.Services.Interfaces;

namespace llm_credit_score_api.Services
{
    public class ReportService : IReportService
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<ReportService> _logger;

        public GetReportResponse GetReport(GetReportRequest request)
        {
            Console.WriteLine("view test");
            return new GetReportResponse();
        }

        public GenerateReportResponse GenerateReport(GenerateReportRequest request)
        {
            try
            {
                var createTaskRequest = new CreateTaskRequest() { TaskKey = TaskKey.GenerateReport };
                var response = _taskService.CreateTask(createTaskRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new GenerateReportResponse() { Exception = ex };
            }
        }
    }
}
