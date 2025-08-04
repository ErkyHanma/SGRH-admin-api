// Archivo: SGRH.Web/Models/Clients/ClientIndexViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace SGRH.Web.Models.Clients
{
    public class ClientIndexViewModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
    }
}
