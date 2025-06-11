namespace SGRH.Domain.Base
{
    public abstract class Person : AuditEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public int RoleId { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

    }

}
