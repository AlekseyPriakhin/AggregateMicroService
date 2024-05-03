using AggregateAndMicroService.Domain.CourseProgress;
using AggregateAndMicroService.Infrastructure;

using MediatR;

using Microsoft.EntityFrameworkCore;

public class UserExcludeFromCourseHandler : INotificationHandler<UserExcludeFromCourse>
{
    private readonly LearningContext _learningContext;

    public UserExcludeFromCourseHandler(LearningContext learningContext)
    {
        _learningContext = learningContext;
    }
    public async Task Handle(UserExcludeFromCourse notification, CancellationToken cancellationToken)
    {
        var course = notification.CourseCompleting;
        var user = await _learningContext.Users.FindAsync(notification.UserId) ?? throw new Exception($"User with id {notification.UserId} not found");
        if (course.Status.Equals(CompleteStatus.Of(CompleteStatuses.InProgress)))
        {
            user.UpdateCourseInProgressCount(false);
        }
        else user.UpdateCompletedCourseCount(false);

        var res = await _learningContext.StageCourseCompletings
                        .Where(e => e.CourseCompletingId == notification.CourseCompleting.Id.Value)
                        .ExecuteDeleteAsync();

        await _learningContext.CourseCompleting
            .Where(e => e.Id.Value == notification.CourseCompleting.Id.Value)
            .ExecuteDeleteAsync();

        return;

    }
}
