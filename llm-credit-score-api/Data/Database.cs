using System.Text.Json;

namespace llm_credit_score_api.Data
{
    public class Database
    {
        public async Task<T?> QueryAsync<T>(string table)
        {
            try
            {
                var path = Path.Combine("Data", "json", $"{table}.json");
                using var stream = File.OpenRead(path);

                var settings = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };
                return await JsonSerializer.DeserializeAsync<T>(stream, settings);
            }
            catch
            {
                throw;
            }
        }
    }
}
