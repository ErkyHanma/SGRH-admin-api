// src/SGRH.Web/Models/Clients/ClientEditViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace SGRH.Web.Models.Clients
{
    public class ClientEditViewModel
    {
        [Required] // User ID is necessary to identify the client to edit
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

        [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters.")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; } = string.Empty;

        // Do not include PasswordHash here unless you have a specific field for changing password on edit.
        // If the API requires it for update, add it, but be careful with security handling.
    }
}