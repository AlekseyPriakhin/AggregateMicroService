using System.ComponentModel.DataAnnotations;

namespace AggregateAndMicroService.Aggregates.User;


public class UserId
{
    [Key]
    public Guid Value { get; }

    private UserId() { }
    private UserId(Guid value)
    {
        Value = value;
    }

    public static UserId Of(Guid guid)
    {
        if (guid == Guid.Empty)
        {
            throw new ArgumentException("Invalid Id");
        }

        return new(guid);
    }

    public static implicit operator Guid(UserId id) => id.Value;
}
