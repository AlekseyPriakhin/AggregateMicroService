using AggregateAndMicroService.Aggregates.Material;
using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Aggregates.User;

public class User : Aggregate<UserId>
{
    public string Name { get; set; }

    public virtual ICollection<Participiant> Participiants { get; set; }

}