using llm_credit_score_api.Models;

namespace llm_credit_score_api.Services.Interfaces
{
    public interface IGeneratorService
    {
        public IEnumerable<AppTask> GetQueuedTasks();
        public Task GenerateReport(int taskId);
    }
}
