using llm_credit_score_api.Messages;

namespace llm_credit_score_api.Services.Interfaces
{
    public interface IReportService
    {
        public GetReportResponse GetReport(GetReportRequest request);
        public GenerateReportResponse GenerateReport(GenerateReportRequest request);
    }
}
