namespace LibraryManagementSystem.Application.DTOs
{
    public class Result
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
