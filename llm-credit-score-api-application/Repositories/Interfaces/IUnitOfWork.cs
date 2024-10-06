namespace llm_credit_score_api.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public IRepository<T> GetRepository<T>() where T : class;
        public Task<int> SaveChangesAsync();
    }
}
