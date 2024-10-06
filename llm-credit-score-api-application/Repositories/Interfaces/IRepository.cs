using System.Linq.Expressions;

namespace llm_credit_score_api.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public void Add(T entity);
        public IQueryable<T> Query();
        public IQueryable<T> Query(int pageNum, int pageSize);
        public IQueryable<T> Query(Expression<Func<T, bool>> expression);
        public Task<T?> GetByIdAsync(int id);
    }
}
