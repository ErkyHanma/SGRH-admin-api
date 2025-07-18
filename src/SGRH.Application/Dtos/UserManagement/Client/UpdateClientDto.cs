using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Dtos.UserManagement.Client
{
    namespace SGRH.Application.Dtos.UserManagement.Client
    {
        public class UpdateClientDto : DtoBase
        {
            public string Name { get; set; } // Client's name for update.
            public string Email { get; set; } // Client's email for update.
                                              // Add other properties required for updating an existing client.
        }
    }
}
