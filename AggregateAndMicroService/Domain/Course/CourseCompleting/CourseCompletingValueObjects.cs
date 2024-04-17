using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Domain.Course;

public class CourseCompletingId : ValueObject
{
    public Guid Value { get; private set; }

    private CourseCompletingId(Guid value)
    {
        Value = value;
    }

    public static CourseCompletingId Of(Guid guid)
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

public enum CompleteStatuses
{
    InProgress = 0,
    Completed = 1
}

public class CompleteStatus : ValueObject
{
    public CompleteStatuses Value { get; }


    private CompleteStatus() { }
    private CompleteStatus(CompleteStatuses value)
    {
        Value = value;
    }

    public static CompleteStatus Of(CompleteStatuses status)
    {
        return new(status);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public class Progress : ValueObject
{
    public int Value { get; }

    private Progress() { Value = 0; }
    private Progress(int value = 0)
    {
        Value = value;
    }

    public static Progress Of(int value)
    {
        if (value < 0 || value > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 100");
        }

        return new(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public class StagesCountData : ValueObject
{
    public int TotalStages { get; }

    public int CompletedStages { get; }


    private StagesCountData() { }
    private StagesCountData(int totalStages, int completedStages)
    {
        TotalStages = totalStages;
        CompletedStages = completedStages;
    }

    public static StagesCountData Of(int totalStages, int completedStages)
    {
        return new(totalStages, completedStages);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TotalStages;
        yield return CompletedStages;
    }
}
