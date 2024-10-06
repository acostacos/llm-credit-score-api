using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Models;
using llm_credit_score_api_application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace llm_credit_score_api.Repositories
{
    public class TaskRepository : Repository<AppTask>, ITaskRepository
    {
        public TaskRepository(IAppDbContext context) : base(context)
        {
        }

        public async Task<List<AppTask>> GetTasksAsync(int pageNum, int pageSize)
        {
            var query = pageSize > 0 ? Query(pageNum, pageSize) : Query();
            return await query
                .Include(x => x.Report)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();
        }

        public async Task<AppTask?> GetTaskAsync(int id)
        {
            return await Query(x => x.TaskId == id)
                .Include(x => x.Report)
                .FirstOrDefaultAsync();
        }
    }
}
