namespace LibraryManagementSystem.Application.DTOs
{
    public class ResponseDto<T>
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public T Data { get; set; }
    }
}
