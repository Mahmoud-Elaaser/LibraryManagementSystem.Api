namespace LibraryManagementSystem.Infrastructure.Specification
{
    public class BookSpecParams
    {
        public string Search { get; set; }
        public int? GenreId { get; set; }
        public int? AuthorId { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
    }

}
