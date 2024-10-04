using llm_credit_score_api.Messages;

namespace llm_credit_score_api.Services.Interfaces
{
    public interface IGenerateReportService
    {
        public GenerateReportResponse GenerateReport(GenerateReportRequest request);
    }
}
