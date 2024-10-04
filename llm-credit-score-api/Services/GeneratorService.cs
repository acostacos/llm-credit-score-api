using llm_credit_score_api.Constants;
using llm_credit_score_api.Messages;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;
using llm_credit_score_api.Services.Interfaces;

namespace llm_credit_score_api.Services
{
    public class GeneratorService : IGeneratorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService; 
        private readonly ILogger<GeneratorService> _logger;

        public GeneratorService(IUnitOfWork unitOfWork, IMessageService messageService, ILogger<GeneratorService> logger)
        {
            _unitOfWork = unitOfWork;
            _messageService = messageService;
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

                // Aggregate information into data

                // Send API Request

                // Save Report to Database
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

        private async void SendAPIRequest()
        {
            // Do Exponential Backoff
            var body = new LLMRequest();
            var response = await _messageService.PostAsync<LLMResponse>(LLMConstants.Url, body);
        }
    }
}
