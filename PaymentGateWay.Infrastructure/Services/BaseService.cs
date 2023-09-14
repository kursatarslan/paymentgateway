using Microsoft.EntityFrameworkCore;
using PaymentGateWay.Application.Interfaces;
using PaymentGateWay.Infrastructure.Contexts;

namespace PaymentGateWay.Infrastructure.Services;

public partial class BaseService<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    internal readonly DbSet<T> _table;

    public BaseService(ApplicationDbContext context)
    {
        _context = context;
        _table = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _table.ToListAsync();
    }

    public async Task<T> GetByIdAsync(T id)
    {
        return await _table.FindAsync(id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _table.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _table.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<T> DeleteAsync(T id)
    {
        var entity = await GetByIdAsync(id);
        _table.Remove(entity);
        await _context.SaveChangesAsync();

        return entity;
    }
}