namespace AggregateAndMicroService.Common;

public class Result<T> {
  public T? Value { get; private set; }
  public bool IsFailure { get; init; }
  public bool IsSuccess { get; init; }
  public string? ErrorMessage { get; private set; }  

  private Result() { }

  public static Result<T> Success(T value) {
    return new() {
      Value = value,
      IsSuccess = true,
      IsFailure = false
    };
  }

  public static Result<T> Failure(string message) {
    return new() {
      ErrorMessage = message,
      IsSuccess = false,
      IsFailure = true
    };
  } 

}

public enum ResultStatus {
  Success = 0,
  Error = 1,
}
