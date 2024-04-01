using AggregateAndMicroService.Aggregates.Course;
using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Aggregates.User;

public class User : Aggregate<UserId>
{
    public string Name { get; private set; }

    public virtual ICollection<CourseCompleting> CourseCompletings { get; private set; }

}