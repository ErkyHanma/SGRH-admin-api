namespace SGRH.Domain.Base
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public dynamic? Data { get; set; }
        public OperationResult() { }



    }
}
