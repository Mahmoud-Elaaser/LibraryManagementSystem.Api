using LibraryManagementSystem.Domain.Enums;

namespace LibraryManagementSystem.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public OrderStatus Status { get; set; }
    }
}
