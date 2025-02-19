﻿namespace LibraryManagementSystem.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public Order Order { get; set; }
        public Book Book { get; set; }
    }
}
