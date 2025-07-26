// src/SGRH.Web/Models/OperationResult.cs
namespace SGRH.Web.Models
{
    // Generic version for when the API returns data
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; } // Can be null if only success/failure is returned

        // Helper static methods for convenience
        public static OperationResult<T> Success(string message = "Operation successful.", T? data = default)
        {
            return new OperationResult<T> { IsSuccess = true, Message = message, Data = data };
        }

        public static OperationResult<T> Failure(string message = "Operation failed.")
        {
            return new OperationResult<T> { IsSuccess = false, Message = message };
        }
    }

    // Non-generic version for when the API only returns success/message
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // Helper static methods for convenience
        public static OperationResult Success(string message = "Operation successful.")
        {
            return new OperationResult { IsSuccess = true, Message = message };
        }

        public static OperationResult Failure(string message = "Operation failed.")
        {
            return new OperationResult { IsSuccess = false, Message = message };
        }
    }
}