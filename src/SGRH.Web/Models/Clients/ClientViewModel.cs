// File: C:\Users\ander\Source\Repos\SGRH-admin-api\src\SGRH.Web\Models\Clients\ClientViewModel.cs

using System.ComponentModel.DataAnnotations;
using System;

namespace SGRH.Web.Models.Clients
{
    // Este modelo de vista se usa para mostrar la lista de clientes y los detalles.
    // Ha sido actualizado para que coincida con las propiedades de la clase "Client" que viene de la API.
    public class ClientViewModel
    {
        // Propiedades de la clase base "BaseEntity"
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        // Propiedades de la clase base "Person"
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Phone]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "User ID")]
        public string? UserId { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        // Propiedades de la clase "Client"
        [Display(Name = "Role ID")]
        public int RoleId { get; set; }
        public string? PasswordHash { get; set; }
    }
}
