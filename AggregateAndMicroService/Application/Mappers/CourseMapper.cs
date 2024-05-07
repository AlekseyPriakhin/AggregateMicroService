using AggregateAndMicroService.Application.DTO.Response;
using AggregateAndMicroService.Domain.Course;

namespace AggregateAndMicroService.Application.Mappers;


public static class CourseMapper
{

    public static ResponseCourseDto ToResponseCourseDto(Course course)
    {
        return new ResponseCourseDto
        {
            Id = course.Id.Value.ToString(),
            Title = course.Title,
            Status = course.Status.Value,
            StagesCount = course.StageCount.Value,
            Description = course.Description,
        };
    }

}
