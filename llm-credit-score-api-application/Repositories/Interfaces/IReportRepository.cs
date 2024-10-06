using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;

namespace llm_credit_score_api_application.Repositories.Interfaces
{
    public interface IReportRepository : IRepository<Report>
    {
        public Task<List<Report>> GetReportsAsync(int pageNum, int pageSize);
        public Task<Report?> GetReportAsync(int id);
        public Task<Report?> GetReportByTaskIdAsync(int taskId);
    }
}
