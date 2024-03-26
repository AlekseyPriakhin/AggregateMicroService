namespace AggregateAndMicroService.Common;

public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; set; }
    public bool IsDeleted { get; set; }
}

// Aggregates
public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{
    
}


// ValueObjects
public abstract class ValueObject
{
    /* protected static bool EqualOperator(ValueObject left, ValueObject right)
    {
        if (left is null || right is null)
        {
            return false;
        }

        return left is null || left.Equals(right);
    }

    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
    {
        return !EqualOperator(left, right);
    }
    */
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
        => GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
}