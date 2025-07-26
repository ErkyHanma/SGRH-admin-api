namespace SGRH.Web.Models
{
    public class BaseResponse<T>
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public T? data { get; set; }

    }
}
