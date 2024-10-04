using llm_credit_score_api.Constants;
using llm_credit_score_api.Messages;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;
using llm_credit_score_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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
                var query = request.PageSize > 0 ? reportRepo.Query(request.PageNum, request.PageSize) : reportRepo.Query();
                var reports = await query
                    .Include(x => x.Company)
                    .Include(x => x.Task)
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();
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
                var reportRepo = _unitOfWork.GetRepository<Report>();
                var report = await reportRepo.Query(x => x.ReportId == id)
                    .Include(x => x.Company)
                    .Include(x => x.Task)
                    .FirstOrDefaultAsync();
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
