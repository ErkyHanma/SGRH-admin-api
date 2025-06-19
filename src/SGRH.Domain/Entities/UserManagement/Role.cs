using SGRH.Domain.Base;

namespace SGRH.Domain.Entities.UserManagement
{
    public class Role : AuditEntity
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
    }

}