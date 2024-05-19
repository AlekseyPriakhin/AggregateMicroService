using AggregateAndMicroService.Domain.Course;

using MediatR;

namespace AggregateAndMicroService.Application.IntegrationEvents;

public class CourseStatusChangeIntegrationEvent : INotification
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Status { get; init; }
    public required int StagesCount { get; init; }
    public string? Description { get; init; }
    //public List<StageResponseDto> Stages { get; init; } = [];
}


public class StageResponseDto
{
    public required string Id { get; init; }

    public required string Title { get; init; }

    public required string Type { get; init; }

    public TimeSpan Duration { get; init; }
    public required string CourseId { get; init; }

    public string? Previous { get; init; }
}