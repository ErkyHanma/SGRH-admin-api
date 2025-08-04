// File: C:\Users\ander\Source\Repos\SGRH-admin-api\src\SGRH.Web\ViewModels\ClientCreateViewModel.cs

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
        [DisplayName("Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // Note: The API does not use RoleId directly, but it can be collected from the view.
        // We will not send this to the API in this implementation.
        [DisplayName("Role")]
        public int RoleId { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; } = true;
    }
}
