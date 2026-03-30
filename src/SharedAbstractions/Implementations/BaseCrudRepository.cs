using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SharedAbstractions.Interfaces;

namespace SharedAbstractions.Implementations;

public class BaseCrudRepository<TId, TEntity, TContext>(TContext context) : ICrudRepository<TId, TEntity>
    where TEntity : class, IEntityBase<TId>
    where TContext : DbContext
{
    private readonly TContext _context = context;

    public virtual async Task<IList<TEntity>> GetAllAsync()
    {
        try
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to retrieve all entities: {ex.Message}");
        }
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        try
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while trying to retrieve Entity by ID: {ex.Message}");
        }
    }

    public virtual async Task<TId> CreateAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
        catch (DbUpdateException e)
        {
            throw new Exception($"Database error while adding new {typeof(TEntity).Name}: {e.InnerException?.Message ?? e.Message}");
        }
        catch (ValidationException e)
        {
            throw new Exception($"Validation failed while adding {typeof(TEntity).Name}: {e.Message}");
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to add new {typeof(TEntity).Name}: {e.Message}");
        }
    }

    public virtual async Task<TId> UpdateAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
        catch (DbUpdateException e)
        {
            throw new Exception($"Database error while updating {typeof(TEntity).Name}: {e.InnerException?.Message ?? e.Message}");
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to update {typeof(TEntity).Name}: {e.Message}");
        }
    }

    public virtual async Task DeleteAsync(TId id)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        catch (DbUpdateException e)
        {
            throw new Exception($"Database error while deleting {typeof(TEntity).Name} with ID {id}: {e.InnerException?.Message ?? e.Message}");
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to delete {typeof(TEntity).Name} with ID {id}: {e.Message}");
        }
    }
}