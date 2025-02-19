namespace LibraryManagementSystem.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime MembershipDate { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Borrowing> Borrowings { get; set; }
    }
}
