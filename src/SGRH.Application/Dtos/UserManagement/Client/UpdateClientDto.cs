using System.ComponentModel.DataAnnotations;

namespace SGRH.Application.Dtos.UserManagement.Client
{
    public class UpdateClientDto
    {
        [Required(ErrorMessage = "El ID de usuario es obligatorio.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "El nombre es un campo obligatorio.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es un campo obligatorio.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "El correo electrónico es un campo obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Email { get; set; } = null!;

        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        [Required(ErrorMessage = "El rol es un campo obligatorio.")]
        public int RoleId { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
