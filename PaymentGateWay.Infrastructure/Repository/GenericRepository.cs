using Microsoft.EntityFrameworkCore;
using PaymentGateWay.Application.Common;
using PaymentGateWay.Application.Interfaces;
using PaymentGateWay.Infrastructure.Contexts;

namespace PaymentGateWay.Infrastructure.Repository;

public class GenericRepository<T>: IGenericRepository<T> where T: BaseEntity<int>
{
    private readonly ApplicationDbContext _context;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _context.Set<T>().ToListAsync();

    public async Task<T> CreateAsync(T entity)
    {
        entity.DateUpdated = DateTime.Now;

        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<T> DeleteAsync(T id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null) _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();

        return id;
    }

    public async Task<T?> GetByIdAsync(T id)
        => await _context.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));

    public async Task<T> UpdateAsync(T entity)
    {
        entity.DateUpdated = DateTime.Now;

        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }
}