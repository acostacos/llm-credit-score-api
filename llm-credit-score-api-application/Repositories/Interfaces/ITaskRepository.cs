using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;

namespace llm_credit_score_api_application.Repositories.Interfaces
{
    public interface ITaskRepository : IRepository<AppTask>
    {
        public Task<List<AppTask>> GetTasksAsync(int pageNum, int pageSize);
        public Task<AppTask?> GetTaskAsync(int id);
    }
}
