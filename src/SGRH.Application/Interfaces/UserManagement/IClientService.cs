using SGRH.Application.Common.Base;
using SGRH.Application.Dtos.UserManagement.Client;
using SGRH.Application.Dtos.UserManagement.Client.SGRH.Application.Dtos.UserManagement.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Interfaces.UserManagement
{
    public interface IClientService : IBaseService<ClientDto, CreateClientDto, UpdateClientDto, RemoveClientDto>
    {
        // Add specific methods for Client if any (e.g., GetClientsByEmail).
    }
}
