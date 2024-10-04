using llm_credit_score_api.Data.Interfaces;
using System.Text.Json;

namespace llm_credit_score_api.Data
{
    public class DatabaseService : IDatabaseService
    {
        private static Database? _db;
        private readonly object _dblock = new();

        public async Task<T?> QueryAsync<T>(string table)
        {
            try
            {
                var db = GetInstance();
                return await db.QueryAsync<T>(table);
            }
            catch
            {
                throw;
            }
        }

        private Database GetInstance()
        {
            lock (_dblock)
            {
                if (_db == null)
                {
                    _db = new Database();
                }
            }
            return _db;
        }
    }
}
