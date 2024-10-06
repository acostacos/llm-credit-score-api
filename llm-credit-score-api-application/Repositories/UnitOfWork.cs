using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;
using llm_credit_score_api_application.Repositories.Interfaces;

namespace llm_credit_score_api.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IAppDbContext _appDbContext;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IRepository<T>)_repositories[typeof(T)];
            }

            if (typeof(T) == typeof(Report))
                _repositories.Add(typeof(T), new ReportRepository(_appDbContext));
            else if (typeof(T) == typeof(AppTask))
                _repositories.Add(typeof(T), new TaskRepository(_appDbContext));
            else
                _repositories.Add(typeof(T), new Repository<T>(_appDbContext));

            return (IRepository<T>) _repositories[typeof(T)];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
