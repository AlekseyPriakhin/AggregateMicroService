namespace AggregateAndMicroService.Aggregates.Material;

public class ParticipiantId {
  public Guid MaterialId { get; }

  public Guid UserId {get;}

  private ParticipiantId(Guid materialId, Guid userId) {
    MaterialId = materialId;
    UserId = userId;
  }

  public static ParticipiantId Of(Guid materialId, Guid userId) {
    if(materialId == Guid.Empty || userId == Guid.Empty) {
      throw new ArgumentException("Invalid Id");
    }
    return new(materialId, userId);
  }
}

public class Progress {
  public int Value { get; }

  private Progress(int value = 0) {
    Value = value;
  }

  public static Progress Of(int value) {
    if(value < 0 || value > 100) {
      throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 100");
    }

    return new(value);
  }
}

public enum ParticipiantStatuses {
  InProggess = 0,
  Completed = 1
}

public class ParticipiantStatus {
  public ParticipiantStatuses Value { get; }

  private ParticipiantStatus(ParticipiantStatuses value) {
    Value = value;
  }

  public static ParticipiantStatus Of(ParticipiantStatuses status) {
    return new(status);
  }
}