using System.Linq.Expressions;

namespace llm_credit_score_api.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public void Add(T entity);
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        public Task<IEnumerable<T>> GetAllAsync();
        public IEnumerable<T> GetAll();
        public T? GetById(int id);
        public void Remove(T entity);
    }
}
