using llm_credit_score_api.Models;

namespace llm_credit_score_api.Messages
{
    public class GenerateReportResponse : BaseResponse
    {
        public Company? Company { get; set; }
        public AppTask? Task { get; set; }
    }
}
