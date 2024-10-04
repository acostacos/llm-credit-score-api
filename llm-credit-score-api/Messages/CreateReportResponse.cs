using llm_credit_score_api.Models;

namespace llm_credit_score_api.Messages
{
    public class CreateReportResponse : BaseResponse
    {
        public Report Report { get; set; }
    }
}
