namespace LibraryManagementSystem.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
