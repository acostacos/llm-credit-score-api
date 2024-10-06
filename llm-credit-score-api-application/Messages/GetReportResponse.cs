using llm_credit_score_api.Models;

namespace llm_credit_score_api.Messages
{
    public class GetReportResponse : BaseResponse
    {
        public IEnumerable<Report>? Reports { get; set; }
    }
}
