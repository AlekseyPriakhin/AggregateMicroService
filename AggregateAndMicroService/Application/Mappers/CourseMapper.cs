using AggregateAndMicroService.Application.DTO;
using AggregateAndMicroService.Domain.Course;

namespace AggregateAndMicroService.Application.Mappers;


public static class CourseMapper
{

    public static OutCourseDto ToOutCourseDto(Course course)
    {
        return new OutCourseDto
        {
            Id = course.Id.Value.ToString(),
            Title = course.Title,
            Status = course.Status.Value,
            StagesCount = course.StageCount.Value,
            Description = course.Description,
        };
    }

}
