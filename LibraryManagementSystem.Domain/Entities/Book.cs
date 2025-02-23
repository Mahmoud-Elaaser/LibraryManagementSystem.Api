namespace LibraryManagementSystem.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public string ISBN { get; set; } = string.Empty; // unique identifier assigned to books for identification and cataloging.
        public DateTime PublicationDate { get; set; }
        public bool IsAvailable { get; set; }
        public Author Author { get; set; }
        public Genre Genre { get; set; }
    }
}
