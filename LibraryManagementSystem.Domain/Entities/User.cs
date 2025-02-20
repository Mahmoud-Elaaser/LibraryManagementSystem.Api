using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime MembershipDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        //public string RefreshToken { get; set; }
        //public DateTime RefreshTokenExpiryTime { get; set; }


        public ICollection<Borrowing> Borrowings { get; set; } = new HashSet<Borrowing>();

    }
}
