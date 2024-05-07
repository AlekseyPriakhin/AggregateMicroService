using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AggregateAndMicroService.Application.DTO.Response;

public record StageResponseDto
{
    public required string Id { get; init; }

    public required string Title { get; init; }

    public required string Type { get; init; }

    public TimeSpan Duration { get; init; }
    public required string CourseId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Previous { get; init; }
}
