namespace LibraryManagementSystem.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        public DateTime OrderDate { get; set; }
    }
}
