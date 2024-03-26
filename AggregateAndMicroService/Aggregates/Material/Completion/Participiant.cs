using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Aggregates.Material;

public class Participiant: Aggregate<ParticipiantId> {

  public ParticipiantStatus Status { get; private set; } 
  public Progress Progress { get; private set; } 

  public static Participiant Create(ParticipiantId id, ParticipiantStatus status, Progress progress) {
    return new() {
      Id = id,
      Status = status,
      Progress = progress
    };
  }

  public void UpdateProgress(int value) {
    Progress = Progress.Of(value);
  }

  public void Complete() {
    Status = ParticipiantStatus.Of(ParticipiantStatuses.Completed);
  }
}