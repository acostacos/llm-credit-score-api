namespace llm_credit_score_api.Messages
{
    public class LLMRequest
    {
        public string? Model { get; set; }
        public List<InputMessage>? Messages { get; set; }
    }

    public class InputMessage
    {
        public string? Role { get; set; }
        public string? Content { get; set; }
    }
}
