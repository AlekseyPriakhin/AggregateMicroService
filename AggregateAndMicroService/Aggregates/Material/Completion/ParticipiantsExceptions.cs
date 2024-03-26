using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Aggregates.Material;

public class AlreadyParticipiantException: ConflictException {
  public AlreadyParticipiantException() : base("Такая запись уже существует") {}
} 