namespace SharedAbstractions.Interfaces;

public interface ICrudRepository<TId, TEntity> where TEntity : IEntityBase<TId>
{
    Task<IList<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TId id);
    Task<TId> CreateAsync(TEntity entity);
    Task<TId> UpdateAsync(TEntity entity);
    Task DeleteAsync(TId id);
}