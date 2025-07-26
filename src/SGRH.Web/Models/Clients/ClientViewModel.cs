// src/SGRH.Web/Models/Clients/ClientViewModel.cs
namespace SGRH.Web.Models.Clients
{
    public class ClientViewModel
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; } // Made nullable as per API response
        public string? LastName { get; set; }  // Made nullable as per API response
        public string Email { get; set; } = string.Empty;
        public string? PasswordHash { get; set; } // Add this property if API returns it
        public int RoleId { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; } // Nullable DateTime
        public int? UpdatedBy { get; set; }   // Nullable int
        public int? DeletedBy { get; set; }   // Nullable int
        public DateTime? DeletedAt { get; set; } // Nullable DateTime
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}