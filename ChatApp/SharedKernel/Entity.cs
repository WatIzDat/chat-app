namespace SharedKernel;

public abstract class Entity
{
    private readonly List<IDomainEvent> domainEvents = [];

    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity() 
    {
    }

    public Guid Id { get; init; }

    // ToList() makes copy instead of reference
    public List<IDomainEvent> DomainEvents => domainEvents.ToList();

    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        domainEvents.Add(domainEvent);
    }
}
