using AggregateAndMicroService.Application.DTO.Response;
using AggregateAndMicroService.Application.IntegrationEvents;
using AggregateAndMicroService.Domain.Course;

namespace AggregateAndMicroService.Application.Mappers;


public static class CourseMapper
{

    public static CourseResponseDto ToCourseResponseDto(Course course)
    {
        return new CourseResponseDto
        {
            Id = course.Id.Value.ToString(),
            Title = course.Title,
            Status = course.Status.Value,
            StagesCount = course.StageCount.Value,
            Description = course.Description,
            Stages = course.Stages is not null
                ? course.Stages.Select(StageMapper.ToStageResponseDto).ToList()
                : []
        };
    }

}
