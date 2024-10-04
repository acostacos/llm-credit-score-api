using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;

namespace llm_credit_score_api.Repositories
{
    public class TaskRepository : Repository<AppTask>, IRepository<AppTask>
    {
        public TaskRepository(IAppDbContext context) : base(context)
        {
        }
    }
}
