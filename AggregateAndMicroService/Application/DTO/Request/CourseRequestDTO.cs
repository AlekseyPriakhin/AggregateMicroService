using System.Diagnostics.CodeAnalysis;

using AggregateAndMicroService.Domain.Course;

namespace AggregateAndMicroService.Application.DTO.Request;


public record CourseRequestDTO : PaginationRequestDTO
{

}


public record UpdateCourseStatusDTO
{
    public Statuses Status { get; init; }
}
