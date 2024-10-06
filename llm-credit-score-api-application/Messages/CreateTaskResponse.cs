using llm_credit_score_api.Models;

namespace llm_credit_score_api.Messages
{
    public class CreateTaskResponse : BaseResponse
    {
        public AppTask? Task { get; set; }
    }
}
