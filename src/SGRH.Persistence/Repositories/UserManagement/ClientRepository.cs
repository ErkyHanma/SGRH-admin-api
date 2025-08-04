<<<<<<< Updated upstream
﻿using Core.Application.Interfaces.Repositories.UserManagement;
using Dapper;
using Npgsql;
using SGRH.Domain.Entities.UserManagement;
using System.Collections.Generic; // Added for IEnumerable if not already present
=======
﻿using Core.SGRH.Application.Interfaces.Repositories.UserManagement;
using Dapper;
using Npgsql;
using SGRH.Domain.Entities.UserManagement;
using System.Collections.Generic;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
                    ""first_name"", ""last_name"", ""email"", ""password_hash"", ""role_id"", ""phone"", ""address"",
                    ""created_at"", ""created_by"", ""updated_at"", ""updated_by"", ""is_deleted"", ""is_active"", ""deleted_by"", ""deleted_at""
=======
                    first_name, last_name, email, password_hash, role_id, phone, address,
                    created_at, created_by, updated_at, updated_by, is_deleted, is_active, deleted_by, deleted_at
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        public async Task<Client?> GetClientByIdAsync(int clientId)
=======
        public async Task<Client?> GetClientByIdAsync(int id)
>>>>>>> Stashed changes
        {
            var sql = @"
                SELECT 
                    user_id AS UserId, 
                    first_name AS FirstName, 
                    last_name AS LastName,
<<<<<<< Updated upstream
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
=======
                    email AS Email,
                    password_hash AS PasswordHash,
                    role_id AS RoleId,
                    phone AS Phone,
                    address AS Address,
                    created_at AS CreatedAt,
                    created_by AS CreatedBy,
                    updated_at AS UpdatedAt,
                    updated_by AS UpdatedBy,
                    deleted_by AS DeletedBy,
                    deleted_at AS DeletedAt,
                    is_deleted AS IsDeleted,
                    is_active AS IsActive
                FROM userManagement.users 
                WHERE user_id = @id AND is_deleted = false;
>>>>>>> Stashed changes
            ";

            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Client>(sql, new { id });
        }

        public async Task<string> UpdateClientAsync(Client client)
        {
            var sql = @"
                UPDATE userManagement.users
<<<<<<< Updated upstream
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
=======
                SET first_name = @FirstName,
                    last_name = @LastName,
                    email = @Email,
                    password_hash = @PasswordHash,
                    role_id = @RoleId,
                    phone = @Phone,
                    address = @Address,
                    updated_at = @UpdatedAt,
                    updated_by = @UpdatedBy,
                    is_active = @IsActive,
                    is_deleted = @IsDeleted
                WHERE user_id = @UserId;
>>>>>>> Stashed changes
            ";

            using var connection = new NpgsqlConnection(_connectionString);
            var result = await connection.ExecuteAsync(sql, client);

            return result > 0 ? "Client updated successfully" : "Failed to update client";
        }

        public async Task<string> DeleteClientAsync(int id)
        {
            var sql = @"
                UPDATE userManagement.users
<<<<<<< Updated upstream
                SET 
                    ""is_active"" = FALSE,
                    ""updated_by"" = @UpdatedBy,
                    ""updated_at"" = NOW(),
                    ""deleted_by"" = @UpdatedBy,
                    ""deleted_at"" = NOW()
                WHERE ""user_id"" = @UserId;
=======
                SET is_deleted = TRUE,
                    deleted_at = NOW()
                WHERE user_id = @id;
>>>>>>> Stashed changes
            ";

            using var connection = new NpgsqlConnection(_connectionString);
            var result = await connection.ExecuteAsync(sql, new { id });

            return result > 0 ? "Client deleted successfully" : "Failed to delete client";
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
<<<<<<< Updated upstream
=======
            // CORRECCIÓN: Agregamos la condición 'is_deleted = false' para no mostrar clientes eliminados.
>>>>>>> Stashed changes
            var sql = @"
                SELECT 
                    user_id AS UserId, 
                    first_name AS FirstName, 
                    last_name AS LastName,
<<<<<<< Updated upstream
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
=======
                    email AS Email,
                    password_hash AS PasswordHash,
                    role_id AS RoleId,
                    phone AS Phone,
                    address AS Address,
                    created_at AS CreatedAt,
                    created_by AS CreatedBy,
                    updated_at AS UpdatedAt,
                    updated_by AS UpdatedBy,
                    deleted_by AS DeletedBy,
                    deleted_at AS DeletedAt,
                    is_deleted AS IsDeleted,
                    is_active AS IsActive
                FROM userManagement.users 
                WHERE is_active = TRUE AND is_deleted = false;
>>>>>>> Stashed changes
            ";

            using var connection = new NpgsqlConnection(_connectionString);
            var clients = await connection.QueryAsync<Client>(sql);

            return clients;
        }
    }
}