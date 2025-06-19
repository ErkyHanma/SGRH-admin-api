namespace SGRH.Domain.Base
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public OperationResult() { }

        public OperationResult(bool isSuccess, string message, T? data = default)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static OperationResult<T> Success(string message, T? data = default)
        {
            return new OperationResult<T>(true, message, data);
        }

        public static OperationResult<T> Failure(string message)
        {
            return new OperationResult<T>(false, message);
        }
    }
}


