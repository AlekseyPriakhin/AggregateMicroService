using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Domain.CourseProgress;

public class StageCourseCompletingId : ValueObject
{
    public Guid Value { get; private set; }

    private StageCourseCompletingId(Guid value)
    {
        Value = value;
    }

    public static StageCourseCompletingId Of(Guid guid)
    {
        if (guid == Guid.Empty)
        {
            throw new ArgumentException("Invalid Id");
        }

        return new(guid);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
