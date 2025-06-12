using SGRH.Domain.Base;

namespace SGRH.Domain.Entities.User_management
{
    public class Role : AuditEntity
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
    }

}