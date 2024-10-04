using llm_credit_score_api.Constants;
using llm_credit_score_api.Messages;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;
using llm_credit_score_api.Services.Interfaces;
using System.Text.Json;

namespace llm_credit_score_api.Services
{
    public class GeneratorService : IGeneratorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService; 
        private readonly IReportService _reportService;
        private readonly ILogger<GeneratorService> _logger;

        public GeneratorService(IUnitOfWork unitOfWork, IMessageService messageService, IReportService reportService, ILogger<GeneratorService> logger)
        {
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _reportService = reportService;
            _logger = logger;
        }

        public IEnumerable<AppTask> GetQueuedTasks()
        {
            try
            {
                var taskRepo = _unitOfWork.GetRepository<AppTask>();
                var queuedTasks = taskRepo.Find(x => x.Status == TaskStat.Queued);
                if (queuedTasks == null)
                {
                    throw new Exception("Error retrieving queued tasks");
                }
                return queuedTasks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task GenerateReport(int taskId)
        {
            AppTask? task = null;
            try
            {
                var taskRepo = _unitOfWork.GetRepository<AppTask>();
                var companyRepo = _unitOfWork.GetRepository<Company>();

                task = taskRepo.Find(x => x.TaskId == taskId)?.FirstOrDefault();
                if (task == null)
                {
                    throw new Exception("Cannot find task");
                }

                task.Status = TaskStat.InProgress;
                await _unitOfWork.SaveChangesAsync();

                var company = companyRepo.Find(x => x.CompanyId == task.CompanyId)?.FirstOrDefault();
                if (company == null)
                {
                    throw new Exception("Invalid company passed");
                }

                // TODO: Aggregate information into data
                var prompt = "";

                var report = await GetLLMResponse(prompt);

                var createReportReq = new CreateReportRequest()
                {
                    CompanyId = company.CompanyId,
                    TaskId = task.TaskId,
                    Content = report,
                };
                var response = await _reportService.CreateReport(createReportReq);
                if (response.Exception != null)
                {
                    throw response.Exception;
                }
                if (response.Report == null)
                {
                    throw new Exception("Report not created");
                }
            }
            catch (Exception ex)
            {
                if (task != null)
                {
                    task.Status = TaskStat.Error;
                }

                _logger.LogError(ex.Message);
                throw;
            }
        }

        private async Task<string> GetLLMResponse(string prompt)
        {
            var body = new LLMRequest();
            var jsonDecodeOpt = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
            var response = await _messageService.PostAsync<LLMResponse>(LLMConstants.Url, body, jsonDecodeOpt);
            var message = response.Choices.FirstOrDefault(x => x.Message.Role == "assistant")?.Message.Content;
            return message ?? "";
        }
    }
}
