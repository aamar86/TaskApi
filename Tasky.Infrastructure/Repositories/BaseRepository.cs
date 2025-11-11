using Microsoft.EntityFrameworkCore;
using Tasky.Application.Interfaces;
using Tasky.Infrastructure.Persistence;

namespace Tasky.Infrastructure.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly TaskyDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(TaskyDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct) => await _dbSet.FindAsync(new object?[] { id }, ct);
    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct) => await _dbSet.AsNoTracking().ToListAsync(ct);
    public async Task AddAsync(TEntity entity, CancellationToken ct) => await _dbSet.AddAsync(entity, ct);
    public void Update(TEntity entity) => _dbSet.Update(entity);
    public void Delete(TEntity entity) => _dbSet.Remove(entity);
    public Task<int> SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}
