namespace ShortPaper_API.Helper
{
    public class ServiceResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public int httpStatusCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
