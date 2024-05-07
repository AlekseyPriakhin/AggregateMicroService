using System.Text.Json.Serialization;

using AggregateAndMicroService.Application.DTO.Request;

namespace AggregateAndMicroService.Application.DTO.Response;


//Interfaces
public interface IResponse<T>
{
    T Data { get; init; }
}

/* public interface IResponseList<T> : IResponse<List<T>>
{
    Pagination Pagination { get; init; }
} */

//Records

public record Pagination
{
    public int Page { get; init; } = 1;
    public int PerPage { get; init; } = 10;
    public int Count { get; init; } = 0;
    public int TotalPages { get; init; } = 0;

    public static Pagination Default => new();
}

public record Meta
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Pagination? Pagination { get; init; }
}

public record Response<T>
{
    public T Data { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Meta? Meta { get; init; }
}

/* public record ResponseList<T> : IResponseList<T>
{
    public List<T> Data { get; init; } = [];
    public Pagination Pagination { get; init; } = Pagination.Default;
} */

public static class PaginationService
{
    private static int GetTotalPage(int total, int count) => total % count > 0
        ? total / count + 1
        : total / count;

    private static int GetTotalPages<T>(List<T> list, int count)
    {

        return 1;
    }

    public static Pagination GetPagination<T, U>(IEnumerable<T> items, U dto, int total) where U : PaginationRequestDTO
    {
        return new Pagination
        {
            Page = dto.Page,
            PerPage = dto.PerPage,
            Count = items.Count(),
            TotalPages = GetTotalPage(total, dto.PerPage),
        };
    }

    public static Pagination GetPagination<T>(IEnumerable<T> items, int page, int perPage, int total)
    {
        return new Pagination
        {
            Page = page,
            PerPage = perPage,
            Count = items.Count(),
            TotalPages = GetTotalPage(total, perPage),
        };
    }

    private static int GetPage(int start, int count) => start / count + 1;
}


public static class ResponseBuilder
{
    public static Response<T> CreateResponse<T>(T response)
    {
        return new Response<T>
        {
            Data = response,
        };
    }

    public static Response<List<T>> CreateResponse<T>(List<T> response, Pagination pagination)
    {
        return new Response<List<T>>
        {
            Data = response,
            Meta = new Meta
            {
                Pagination = pagination,
            }
        };
    }

}
