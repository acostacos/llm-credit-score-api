using Microsoft.EntityFrameworkCore;

namespace llm_credit_score_api.Data.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public void Dispose();
    }
}
