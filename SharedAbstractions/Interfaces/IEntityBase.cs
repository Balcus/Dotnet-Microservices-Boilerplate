namespace SharedAbstractions.Interfaces;

public interface IEntityBase<TId>
{
    public TId Id { get; set; }
}