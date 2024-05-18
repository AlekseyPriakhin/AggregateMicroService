namespace AggregateAndMicroService.Application.DTO.Request;

public record PaginationRequestDTO
{
    public int Page { get; init; } = 1;

    public int PerPage { get; init; } = 10;
}
