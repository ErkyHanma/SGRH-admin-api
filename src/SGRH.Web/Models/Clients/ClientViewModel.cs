// File: C:\Users\ander\Source\Repos\SGRH-admin-api\src\SGRH.Web\Models\Clients\ClientViewModel.cs

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // Es crucial incluir este "using"
using System;

namespace SGRH.Web.Models.Clients
{
    // Este modelo de vista se usa para mostrar la lista de clientes y los detalles.
    // Ha sido actualizado para que coincida con las propiedades de la clase "Client" que viene de la API.
    public class ClientViewModel
    {
        // Usamos el atributo [JsonPropertyName("userId")] para mapear la propiedad "userId" del JSON a "Id" en nuestro modelo.
        [JsonPropertyName("userId")]
        public int Id { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("createdBy")]
        public int CreatedBy { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("updatedBy")]
        public int? UpdatedBy { get; set; }

        // Propiedades de la clase base "Person"
        [Required]
        [Display(Name = "First Name")]
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [Phone]
        [Display(Name = "Phone")]
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [Display(Name = "Address")]
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        // No necesitamos la propiedad "UserId" porque el "userId" de la API se mapea a nuestra propiedad "Id".
        // [Display(Name = "User ID")]
        // public string? UserId { get; set; }

        [Display(Name = "Is Active")]
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [Display(Name = "Is Deleted")]
        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }

        // Propiedades de la clase "Client"
        [Display(Name = "Role ID")]
        [JsonPropertyName("roleId")]
        public int RoleId { get; set; }

        [JsonPropertyName("passwordHash")]
        public string? PasswordHash { get; set; }
    }
}
