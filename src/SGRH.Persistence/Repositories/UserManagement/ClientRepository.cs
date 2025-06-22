using Core.Application.Interfaces.Repositories.UserManagement;
using Dapper;// Coloqué Dapper para ejecutar procedimientos almacenados (SPs) de forma eficiente y mapear los resultados directamente a objetos C# como 'Client'.
using Npgsql;
using SGRH.Domain.Entities.UserManagement;
using System.Threading.Tasks;

namespace SGRH.Persistence.Repositories.UserManagement
{
    public class ClientRepository : IClientRepository
    {
        private readonly string _connectionString;

        public ClientRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<string> CreateClientAsync(Client client)
        {
            var sql = @"
                INSERT INTO Clients (UserId, FirstName, LastName, Email, PasswordHash, RoleId, Phone, Address)
                VALUES (@UserId, @FirstName, @LastName, @Email, @PasswordHash, @RoleId, @Phone, @Address);
            ";

            using var connection = new NpgsqlConnection(_connectionString);
            var result = await connection.ExecuteAsync(sql, client);

            return result > 0 ? "Client created successfully" : "Failed to create client";
        }

        public async Task<Client?> GetClientByIdAsync(int clientId)
        {
            var sql = "SELECT * FROM Clients WHERE UserId = @UserId";

            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Client>(sql, new { UserId = clientId });
        }

        public async Task<string> UpdateClientAsync(Client client)
        {
            var sql = @"
                UPDATE Clients
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    Email = @Email,
                    PasswordHash = @PasswordHash,
                    RoleId = @RoleId,
                    Phone = @Phone,
                    Address = @Address
                WHERE UserId = @UserId;
            ";

            using var connection = new NpgsqlConnection(_connectionString);
            var result = await connection.ExecuteAsync(sql, client);

            return result > 0 ? "Client updated successfully" : "Failed to update client";
        }

        public async Task<string> DisableClientAsync(int clientId, int updatedBy)
        {
            var sql = @"
                UPDATE Clients
                SET IsActive = false,
                    UpdatedBy = @UpdatedBy,
                    UpdatedAt = NOW()
                WHERE UserId = @UserId;
            ";

            using var connection = new NpgsqlConnection(_connectionString);
            var result = await connection.ExecuteAsync(sql, new { UserId = clientId, UpdatedBy = updatedBy });

            return result > 0 ? "Client disabled successfully" : "Failed to disable client";
        }
    }
}
