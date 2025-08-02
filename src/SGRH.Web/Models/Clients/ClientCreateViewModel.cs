// src/SGRH.Web/Models/Clients/ClientCreateViewModel.cs
using System; // Agregado para DateTime
using System.ComponentModel.DataAnnotations; // Para atributos de validación

namespace SGRH.Web.Models.Clients
{
    public class ClientCreateViewModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100, ErrorMessage = "First Name cannot exceed 100 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100, ErrorMessage = "Last Name cannot exceed 100 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string PasswordHash { get; set; } = string.Empty;

        public int RoleId { get; set; }

        [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters.")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; } = string.Empty;

        
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        // -----------------------------------------------------------
    }
}