
namespace Models
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}