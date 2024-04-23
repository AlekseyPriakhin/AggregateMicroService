namespace AggregateAndMicroService.Domain.DTO;

public record CourseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
