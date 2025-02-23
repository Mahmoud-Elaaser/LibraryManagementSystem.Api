namespace LibraryManagementSystem.Application.DTOs.Auth
{
    public class UpdateRoleRequest
    {
        public string UserId { get; set; }
        public string OldRoleName { get; set; }
        public string NewRoleName { get; set; }
    }
}
