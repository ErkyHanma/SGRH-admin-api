using Core.Application.Interfaces.Repositories.UserManagement;
using Dapper;
using Npgsql;
using SGRH.Domain.Entities.UserManagement;
using System.Collections.Generic; // Added for IEnumerable if not already present
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
                INSERT INTO userManagement.users (
                    ""first_name"", ""last_name"", ""email"", ""password_hash"", ""role_id"", ""phone"", ""address"",
                    ""created_at"", ""created_by"", ""updated_at"", ""updated_by"", ""is_deleted"", ""is_active"", ""deleted_by"", ""deleted_at""
                )
                VALUES (
                    @FirstName, @LastName, @Email, @PasswordHash, @RoleId, @Phone, @Address,
                    @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy, @IsDeleted, @IsActive, @DeletedBy, @DeletedAt
                );
            ";

            using var connection = new NpgsqlConnection(_connectionString);
            var result = await connection.ExecuteAsync(sql, client);

            return result > 0 ? "Client created successfully" : "Failed to create client";
        }

        public async Task<Client?> GetClientByIdAsync(int clientId)
        {
            var sql = @"
                SELECT 
                    user_id AS UserId, 
                    first_name AS FirstName, 
                    last_name AS LastName,
                    email AS Email, 
                    password_hash AS PasswordHash, 
                    role_id AS RoleId,
                    phone AS Phone, 
                    address AS Address, 
                    created_at AS CreatedAt,
                    created_by AS CreatedBy, 
                    updated_at AS UpdatedAt, 
                    updated_by AS UpdatedBy,
                    is_deleted AS IsDeleted, 
                    is_active AS IsActive, 
                    deleted_by AS DeletedBy,
                    deleted_at AS DeletedAt
                FROM userManagement.users 
                WHERE ""user_id"" = @UserId;
            ";

            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Client>(sql, new { UserId = clientId });
        }

        public async Task<string> UpdateClientAsync(Client client)
        {
            var sql = @"
                UPDATE userManagement.users
                SET 
                    ""first_name"" = @FirstName,
                    ""last_name"" = @LastName,
                    ""email"" = @Email,
                    ""password_hash"" = @PasswordHash,
                    ""role_id"" = @RoleId,
                    ""phone"" = @Phone,
                    ""address"" = @Address,
                    ""updated_at"" = @UpdatedAt,
                    ""updated_by"" = @UpdatedBy
                WHERE ""user_id"" = @UserId;
            ";

            using var connection = new NpgsqlConnection(_connectionString);
            var result = await connection.ExecuteAsync(sql, client);

            return result > 0 ? "Client updated successfully" : "Failed to update client";
        }

        public async Task<string> DisableClientAsync(int clientId, int updatedBy)
        {
            var sql = @"
                UPDATE userManagement.users
                SET 
                    ""is_active"" = FALSE,
                    ""updated_by"" = @UpdatedBy,
                    ""updated_at"" = NOW(),
                    ""deleted_by"" = @UpdatedBy,
                    ""deleted_at"" = NOW()
                WHERE ""user_id"" = @UserId;
            ";

            using var connection = new NpgsqlConnection(_connectionString);
            var result = await connection.ExecuteAsync(sql, new { UserId = clientId, UpdatedBy = updatedBy });

            return result > 0 ? "Client disabled successfully" : "Failed to disable client";
        }

        //Added per Andersson to get all users type clients if is needit
        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            var sql = @"
                SELECT 
                    user_id AS UserId, 
                    first_name AS FirstName, 
                    last_name AS LastName,
                    email AS Email, 
                    password_hash AS PasswordHash, 
                    role_id AS RoleId,
                    phone AS Phone, 
                    address AS Address, 
                    created_at AS CreatedAt,
                    created_by AS CreatedBy, 
                    updated_at AS UpdatedAt, 
                    updated_by AS UpdatedBy,
                    is_deleted AS IsDeleted, 
                    is_active AS IsActive, 
                    deleted_by AS DeletedBy,
                    deleted_at AS DeletedAt
                FROM userManagement.users 
                WHERE ""is_active"" = TRUE;
            ";

            using var connection = new NpgsqlConnection(_connectionString);
            var clients = await connection.QueryAsync<Client>(sql);

            return clients;
        }
    }
}