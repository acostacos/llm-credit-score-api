using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repository.Interfaces;

namespace llm_credit_score_api.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private IDatabaseService _databaseService;

        public CompanyRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        
        public Company? Get(int id)
        {
            try
            {
                var companies = _databaseService.QueryAsync<List<Company>>("company_metadata").Result;
                if (companies == null)
                {
                    throw new Exception("Unable to access data");
                }
                return companies.FirstOrDefault(x => x.CompanyId == id);
            }
            catch
            {
                throw;
            }
        }
    }
}
