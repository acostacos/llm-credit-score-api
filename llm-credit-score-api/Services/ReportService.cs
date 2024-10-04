using llm_credit_score_api.Constants;
using llm_credit_score_api.Messages;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;
using llm_credit_score_api.Services.Interfaces;

namespace llm_credit_score_api.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskService _taskService;
        private readonly ILogger<ReportService> _logger;

        public ReportService(IUnitOfWork unitOfWork, ITaskService taskService, ILogger<ReportService> logger)
        {
            _unitOfWork = unitOfWork;
            _taskService = taskService;
            _logger = logger;
        }

        public GetReportResponse GetReport(GetReportRequest request)
        {
            Console.WriteLine("view test");
            return new GetReportResponse();
        }

        public async Task<GenerateReportResponse> GenerateReport(GenerateReportRequest request)
        {
            try
            {
                var companyRepo = _unitOfWork.GetRepository<Company>();
                var company = companyRepo.Find(x => x.CompanyId == request.CompanyId)?.FirstOrDefault();
                if (company == null)
                {
                    throw new Exception("Invalid company passed");
                }

                var createTaskRequest = new CreateTaskRequest() {
                    TaskKey = TaskKey.GenerateReport,
                    CompanyId = request.CompanyId,
                };
                var response = await _taskService.CreateTask(createTaskRequest);
                if (response.Exception != null)
                {
                    throw response.Exception;
                }
                if (response.Task == null)
                {
                    throw new Exception("Task not created");
                }
                return new GenerateReportResponse() { Task = response.Task, Company = company };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new GenerateReportResponse() { Exception = ex };
            }
        }
    }
}
