using AggregateAndMicroService.Application.DTO.Response;
using AggregateAndMicroService.Domain.User;

namespace AggregateAndMicroService.Application.Mappers;

public static class UserMapper
{
    public static UserResponseDto ToUserResponseDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id.Value.ToString(),
            Name = user.Name,
            CompletedCoursesCount = user.CompletedCoursesCount,
            CourseInProgressCount = user.CourseInProgressCount
        };
    }
}
