using llm_credit_score_api.Models;

namespace llm_credit_score_api.Repository.Interfaces
{
    public interface ICompanyRepository
    {
        public Company? Get(int id);
    }
}
