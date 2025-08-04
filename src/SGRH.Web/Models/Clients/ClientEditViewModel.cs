<<<<<<< Updated upstream
﻿// src/SGRH.Web/Models/Clients/ClientEditViewModel.cs
using System; // Necesario para DateTime y DateTime?
=======
﻿// Archivo: SGRH.Web/Models/Clients/ClientEditViewModel.cs
>>>>>>> Stashed changes
using System.ComponentModel.DataAnnotations;

namespace SGRH.Web.Models.Clients
{
    public class ClientEditViewModel
    {
<<<<<<< Updated upstream
        [Required] // El ID de usuario es esencial para identificar el cliente a editar
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100, ErrorMessage = "First Name cannot exceed 100 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100, ErrorMessage = "Last Name cannot exceed 100 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;

        // PasswordHash: Hacemos que sea nullable (string?) en el ViewModel
        // porque no siempre se va a cambiar la contraseña al editar otros datos.
        // Tu API de actualización debería manejar que este campo pueda ser nulo si no se envía.
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage = "Role is required.")] // RoleId es típicamente requerido
        public int RoleId { get; set; }

        [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters.")]
        public string? Phone { get; set; } // Puede ser nullable si tu API lo permite

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; } // Puede ser nullable si tu API lo permite

        // --- Propiedades de auditoría y estado (que estaban faltando) ---
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        // -----------------------------------------------------------------
=======
        [Required]
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? UserId { get; set; }
        public bool IsActive { get; set; }
>>>>>>> Stashed changes
    }
}
