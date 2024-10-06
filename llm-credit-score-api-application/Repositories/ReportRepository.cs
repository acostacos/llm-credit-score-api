using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Models;
using llm_credit_score_api_application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace llm_credit_score_api.Repositories
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository(IAppDbContext context) : base(context)
        {
        }

        public async Task<List<Report>> GetReportsAsync(int pageNum, int pageSize)
        {
            var query = pageSize > 0 ? Query(pageNum, pageSize) : Query();
            return await query
                .Include(x => x.Company)
                .Include(x => x.Task)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();
        }

        public async Task<Report?> GetReportAsync(int id)
        {
            return await Query(x => x.ReportId == id)
                .Include(x => x.Company)
                .Include(x => x.Task)
                .FirstOrDefaultAsync();
        }

        public async Task<Report?> GetReportByTaskIdAsync(int taskId)
        {
            return await Query(x => x.ReportId == taskId)
                .Include(x => x.Company)
                .Include(x => x.Task)
                .FirstOrDefaultAsync();
        }
    }
}
