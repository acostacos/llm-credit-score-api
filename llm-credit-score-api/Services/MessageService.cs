using llm_credit_score_api.Services.Interfaces;
using Polly;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace llm_credit_score_api.Services
{
    public class MessageService : IMessageService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MessageService> _logger;
        
        public MessageService(IHttpClientFactory httpClientFactory, ILogger<MessageService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<T> PostAsync<T>(string url, object body, JsonSerializerOptions? options = null)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(body);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                using HttpClient client = _httpClientFactory.CreateClient();
                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

                var responseData = await retryPolicy.ExecuteAsync<string>(async () =>
                {
                    var response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                });
                    
                var json = JsonSerializer.Deserialize<T>(responseData, options ?? new JsonSerializerOptions());
                if (json == null)
                {
                    throw new Exception("Error parsing JSON response");
                }

                return json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
