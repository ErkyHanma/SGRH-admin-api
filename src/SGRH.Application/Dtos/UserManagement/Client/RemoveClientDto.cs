using System.ComponentModel.DataAnnotations;

namespace SGRH.Application.Dtos.UserManagement.Client
{
    // DTO para la eliminación de un cliente. 
    // Aunque el ID se pasa por la URL, este DTO puede ser útil
    // para la capa de servicio si se requiere un objeto DTO para la eliminación.
    public class RemoveClientDto
    {
        [Required]
        public int UserId { get; set; }
    }
}
