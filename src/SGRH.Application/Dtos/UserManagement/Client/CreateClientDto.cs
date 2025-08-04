using System.ComponentModel.DataAnnotations;

namespace SGRH.Application.Dtos.UserManagement.Client
{
    public class CreateClientDto
    {
        [Required(ErrorMessage = "El nombre es un campo obligatorio.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es un campo obligatorio.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "El correo electrónico es un campo obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es un campo obligatorio.")]
        public string PasswordHash { get; set; } = null!;

        public string? Phone { get; set; }
        public string? Address { get; set; }

        [Required(ErrorMessage = "El rol es un campo obligatorio.")]
        public int RoleId { get; set; }
    }
}
