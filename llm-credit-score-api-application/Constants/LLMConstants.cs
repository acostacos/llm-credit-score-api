namespace llm_credit_score_api.Constants
{
    public static class LLMConstants
    {
        public const string Url = "http://localhost:5001/";
        public const string Model = "gpt-4o";
        public const string Prompt = @"
            You are a credit rating agent with sharp observation and critical thinking skills. You are proficient in processing
            financial information and generating accurate credit reports from that.

            Generate a credit rating report for the company [COMPANY_NAME]. 
            Here are some relevant statistics about them in the format Year: (Name, Value): 
            [STATISTICS]
        ";
    }
}
