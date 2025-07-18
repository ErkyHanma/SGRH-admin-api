using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.UserManagement.Client
{    public class CreateClientDto
    {
        public string Name { get; set; } // Client's name for creation.
        public string Email { get; set; } // Client's email for creation.
        // Add other properties required for creating a new client.
    }
}
