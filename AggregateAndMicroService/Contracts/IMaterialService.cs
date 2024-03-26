namespace AggregateAndMicroService.Contracts;
public interface IMaterialService {
  public void ChangeDuration(Guid id, TimeSpan duration);
}