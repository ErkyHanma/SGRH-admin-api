<<<<<<< Updated upstream
﻿// src/SGRH.Web/Models/Clients/ClientCreateViewModel.cs
using System; // Agregado para DateTime
using System.ComponentModel.DataAnnotations; // Para atributos de validación
=======
﻿// File: C:\Users\ander\Source\Repos\SGRH-admin-api\src\SGRH.Web\ViewModels\ClientCreateViewModel.cs
>>>>>>> Stashed changes

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGRH.Web.ViewModels
{
    // ViewModel for creating a new client from the web application.
    public class ClientCreateViewModel
    {
        [Required(ErrorMessage = "The first name is required.")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The last name is required.")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The password is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
<<<<<<< Updated upstream
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string PasswordHash { get; set; } = string.Empty;

        public int RoleId { get; set; }
=======
        [DisplayName("Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
>>>>>>> Stashed changes

        // Note: The API does not use RoleId directly, but it can be collected from the view.
        // We will not send this to the API in this implementation.
        [DisplayName("Role")]
        public int RoleId { get; set; }

<<<<<<< Updated upstream
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; } = string.Empty;

        
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        // -----------------------------------------------------------
=======
        [DisplayName("Is Active")]
        public bool IsActive { get; set; } = true;
>>>>>>> Stashed changes
    }
}
