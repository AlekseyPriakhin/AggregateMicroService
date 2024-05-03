using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Domain.User;

public class User : Aggregate<UserId>
{
    public string Name { get; private set; }

    public int CompletedCoursesCount { get; private set; }

    public int CourseInProgressCount { get; private set; }

    //public virtual ICollection<CourseCompleting> CourseCompletings { get; private set; }
    private User() { }

    public static User Create(string name)
    {
        return new User
        {
            Id = UserId.Of(Guid.NewGuid()),
            Name = name,
            CompletedCoursesCount = 0,
            CourseInProgressCount = 0
        };
    }

    public void UpdateCompletedCourseCount(bool isIncrease)
    {
        if (isIncrease)
        {
            CompletedCoursesCount++;
            UpdateCourseInProgressCount(false);

        }
        else CompletedCoursesCount--;
    }


    // Можно ли сюда передать CourseCompleting 
    public void UpdateCourseInProgressCount(bool isIncrease)
    {
        if (isIncrease) CompletedCoursesCount++;
        else CompletedCoursesCount--;
    }

}
