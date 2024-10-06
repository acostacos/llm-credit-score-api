using llm_credit_score_api.Messages;

namespace llm_credit_score_api.Services.Interfaces
{
    public interface IReportService
    {
        public Task<GetReportResponse> GetReport(GetReportRequest request);
        public Task<GetReportResponse> GetReport(int id);
        public Task<CreateReportResponse> CreateReport(CreateReportRequest request);
        public Task<GenerateReportResponse> GenerateReport(GenerateReportRequest request);
    }
}
