/// <summary>
/// Standard result object representing success or failure of an operation.
/// </summary>
namespace Application.Common.Models
{
    public class Result
    {
        /// <summary>
        /// Indicates whether the operation succeeded.
        /// </summary>
        public bool IsSuccess { get; init; }
        /// <summary>
        /// Error message if the operation failed.
        /// </summary>
        public string? Error { get; init; }
        /// <summary>
        /// Convenience property for !IsSuccess.
        /// </summary>
        public bool IsFailure => !IsSuccess;
        /// <summary>
        /// Create a successful result instance.
        /// </summary>
        public static Result Success() => new() { IsSuccess = true };
        /// <summary>
        /// Create a failed result instance with an error message.
        /// </summary>
        public static Result Failure(string error) => new() { IsSuccess = false, Error = error };
    }

    /// <summary>
/// Result type that returns a value on success.
/// </summary>
    public class Result<T> : Result
    {
        /// <summary>
        /// Value returned when operation succeeds.
        /// </summary>
        public T? Value { get; init; }

        /// <summary>
        /// Create a successful result with a return value.
        /// </summary>
        public static Result<T> Success(T value) =>
            new() { IsSuccess = true, Value = value };

        /// <summary>
        /// Create a failed result with a return type.
        /// </summary>
        public new static Result<T> Failure(string error) =>
            new() { IsSuccess = false, Error = error };
    }
}

