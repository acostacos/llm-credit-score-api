using llm_credit_score_api.Models;

namespace llm_credit_score_api.Messages
{
    public class GetTaskResponse : BaseResponse
    {
        public IEnumerable<AppTask>? Tasks { get; set; }
    }
}
