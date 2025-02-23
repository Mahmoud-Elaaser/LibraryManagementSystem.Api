﻿namespace LibraryManagementSystem.Application.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
    }
}
