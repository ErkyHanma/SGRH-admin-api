// This model represents the client data received from the API.
// It is the base model that all other ViewModels and the Controller rely on.
namespace SGRH.Web.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        // Propiedades agregadas para corregir los errores.
        public string? UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
