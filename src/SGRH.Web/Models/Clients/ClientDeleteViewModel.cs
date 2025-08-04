// Archivo: SGRH.Web/Models/Clients/ClientDeleteViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace SGRH.Web.Models.Clients
{
    public class ClientDeleteViewModel
    {
        [Required]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}
