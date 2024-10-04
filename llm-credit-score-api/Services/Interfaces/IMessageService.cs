namespace llm_credit_score_api.Services.Interfaces
{
    public interface IMessageService
    {
        public Task PostAsync(string url, object body);
    }
}
