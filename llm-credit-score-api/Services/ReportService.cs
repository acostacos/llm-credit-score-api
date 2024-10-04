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

        public async Task<GetReportResponse> GetReport(GetReportRequest request)
        {
            try
            {
                var reportRepo = _unitOfWork.GetRepository<Report>();
                var reports = await reportRepo.GetAllAsync();
                return new GetReportResponse() { Reports = reports };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new GetReportResponse() { Exception = ex };
            }
        }

        public async Task<CreateReportResponse> CreateReport(CreateReportRequest request)
        {
            try
            {
                var reportRepo = _unitOfWork.GetRepository<Report>();
                var report = new Report()
                {
                    CompanyId = request.CompanyId,
                    TaskId = request.TaskId,
                    Content = request.Content,
                    CreateDate = DateTime.Now,
                };
                reportRepo.Add(report);
                await _unitOfWork.SaveChangesAsync();

                return new CreateReportResponse() { Report = report };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new CreateReportResponse() { Exception = ex };
            }
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
