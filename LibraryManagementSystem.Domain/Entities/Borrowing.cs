namespace LibraryManagementSystem.Domain.Entities
{
    public class Borrowing
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }

        // Navigation properties
        public Book Book { get; set; }
        public User User { get; set; }
    }
}
