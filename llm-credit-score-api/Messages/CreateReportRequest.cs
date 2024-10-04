namespace llm_credit_score_api.Messages
{
    public class CreateReportRequest
    {
        public int CompanyId { get; set; }
        public int TaskId { get; set; }
        public string Content { get; set; }
    }
}
