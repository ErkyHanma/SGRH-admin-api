using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.UserManagement.Client
{
    public class ClientDto : DtoBase
    {
        public string Name { get; set; } // Client's name.
        public string Email { get; set; } // Client's email address.
        // Add other basic properties for a client here.
    }
}
