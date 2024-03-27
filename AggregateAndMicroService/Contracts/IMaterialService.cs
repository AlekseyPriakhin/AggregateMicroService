namespace AggregateAndMicroService.Contracts;
public interface IMaterialService {
  public Task<bool> ChangeDuration(Guid id, TimeSpan duration);
}