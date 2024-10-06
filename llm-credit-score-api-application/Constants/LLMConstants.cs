namespace llm_credit_score_api.Constants
{
    public static class LLMConstants
    {
        public const string Url = "http://localhost:5001/";
        public const string Prompt = @"
            Generate a credit rating report for the company [COMPANY_NAME]. 
            Here are some relevant statistics about them in the format Year: (Name, Value): 
            [STATISTICS]
        ";
    }
}
