using llm_credit_score_api.Constants;
using llm_credit_score_api.Messages;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;
using llm_credit_score_api.Services.Interfaces;
using llm_credit_score_api_application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
                var reportRepo = (IReportRepository)_unitOfWork.GetRepository<Report>();
                var reports = await reportRepo.GetReportsAsync(request.PageNum, request.PageSize);
                return new GetReportResponse() { Reports = reports };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new GetReportResponse() { Error = ex.Message };
            }
        }

        public async Task<GetReportResponse> GetReport(int id)
        {
            try
            {
                var reportRepo = (IReportRepository)_unitOfWork.GetRepository<Report>();
                var report = await reportRepo.GetReportAsync(id);
                var reports = report != null ? new List<Report>() { report } : [];
                return new GetReportResponse() { Reports = reports };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new GetReportResponse() { Error = ex.Message };
            }
        }

        public async Task<CreateReportResponse> CreateReport(CreateReportRequest request)
        {
            try
            {
                var reportRepo = (IReportRepository)_unitOfWork.GetRepository<Report>();
                var existing = await reportRepo.GetReportByTaskIdAsync(request.TaskId);
                if (existing != null)
                {
                    throw new Exception("Report for task already exists");
                }

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
                return new CreateReportResponse() { Error = ex.Message };
            }
        }

        public async Task<GenerateReportResponse> GenerateReport(GenerateReportRequest request)
        {
            try
            {
                var companyRepo = _unitOfWork.GetRepository<Company>();
                var company = await companyRepo.GetByIdAsync(request.CompanyId) ?? throw new Exception("Invalid company passed");

                var createTaskRequest = new CreateTaskRequest() {
                    TaskKey = TaskKey.GenerateReport,
                    CompanyId = request.CompanyId,
                };
                var response = await _taskService.CreateTask(createTaskRequest);
                var newTask = response.Task ?? throw new Exception(response.Error ?? "Task not created");

                return new GenerateReportResponse() { Task = newTask, Company = company };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new GenerateReportResponse() { Error = ex.Message };
            }
        }
    }
}
