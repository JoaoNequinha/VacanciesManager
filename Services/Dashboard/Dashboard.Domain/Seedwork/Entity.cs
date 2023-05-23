namespace Dashboard.Domain.Seedwork;

public abstract class Entity : IEntity
{
    public Guid Id { get; init; }
}
