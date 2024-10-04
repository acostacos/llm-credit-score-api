using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Repositories.Interfaces;
using System.Linq.Expressions;

namespace llm_credit_score_api.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IAppDbContext _context;
        public Repository(IAppDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsQueryable();
        }

        public IQueryable<T> Query(int pageNum, int pageSize)
        {
            var pn = pageNum > 0 ? pageNum : 1;
            var ps = pageSize > 0 ? pageSize : 10;

            return _context.Set<T>().Skip((pn-1) * ps).Take(ps);
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }
}
