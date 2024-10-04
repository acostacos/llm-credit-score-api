using llm_credit_score_api.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace llm_credit_score_api.Services
{
    public class MessageService : IMessageService
    {
        private static readonly HttpClient _httpClient = new();
        private readonly ILogger<MessageService> _logger;
        
        public MessageService(ILogger<MessageService> logger)
        {
            _logger = logger;
        }

        public async Task PostAsync(string url, object body)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(body);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
