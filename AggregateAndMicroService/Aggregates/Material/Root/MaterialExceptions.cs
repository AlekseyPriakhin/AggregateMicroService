using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Aggregates.Material;

public class CreateWithArchivedStatusException : BadRequestException {
  public CreateWithArchivedStatusException() : base("Cannot create material with archived status") { }
}

public class DurationRequiredException : BadRequestException {
  public DurationRequiredException(string type) : base($"Duration is required for {type}") { }
}

public class NotFoundException: CustomException {
  public NotFoundException(Guid guid): base($"Материал с таким Id - {guid} не существует",System.Net.HttpStatusCode.NotFound) { }
}
