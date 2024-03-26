using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Aggregates.Material;

public class CreateWithArchivedStatusException : BadRequestException {
  public CreateWithArchivedStatusException() : base("Cannot create material with archived status") { }
}

public class DurationRequiredException : BadRequestException {
  public DurationRequiredException(string type) : base($"Duration is required for {type}") { }
}