using System.Text.Json;

namespace llm_credit_score_api.Services.Interfaces
{
    public interface IMessageService
    {
        public Task<T> PostAsync<T>(string url, object body, JsonSerializerOptions? options = null);
    }
}
