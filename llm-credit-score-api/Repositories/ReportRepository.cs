using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;

namespace llm_credit_score_api.Repositories
{
    public class ReportRepository : Repository<Report>, IRepository<Report>
    {
        public ReportRepository(IAppDbContext context) : base(context)
        {
        }
    }
}
