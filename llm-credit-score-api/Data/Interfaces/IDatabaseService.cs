namespace llm_credit_score_api.Data.Interfaces
{
    public interface IDatabaseService
    {
        public Task<T?> QueryAsync<T>(string table);
    }
}
