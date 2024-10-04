using llm_credit_score_api.Constants;

namespace llm_credit_score_api.Messages
{
    public class CreateTaskRequest
    {
        public required string TaskKey { get; set; }
        public int CompanyId { get; set; }
    }
}
