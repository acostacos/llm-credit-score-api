using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Models;

namespace llm_credit_score_api.Repository.Interfaces
{
    public class TaskRepository : ITaskRepository
    {
        private IDatabaseService _databaseService;

        public TaskRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        
        public List<ApiTask>? Get()
        {
            try
            {
                var tasks = _databaseService.QueryAsync<List<Company>>("tasks").Result;
                if (tasks == null)
                {
                    throw new Exception("Unable to access data");
                }
                return tasks;
            }
            catch
            {
                throw;
            }
        }

        public List<ApiTask>? Get()
        {
            try
            {
                var tasks = _databaseService.QueryAsync<List<Company>>("tasks").Result;
                if (tasks == null)
                {
                    throw new Exception("Unable to access data");
                }
                return tasks;
            }
            catch
            {
                throw;
            }
        }
    }
}
