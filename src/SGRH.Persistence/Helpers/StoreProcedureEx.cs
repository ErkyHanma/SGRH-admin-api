using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using SGRH.Domain.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGRH.Persistence.Helpers
{
    public static class StoreProcedureEx
    {
        public static async Task<OperationResult<string>> ExecuteAsync(
            string connectionString,
            string procedureName,
            Dictionary<string, object> parameters, // Nombre del procedimiento + Dto con un valor
            ILogger logger)
        {
            var result = new OperationResult<string>();

            try
            {
                // Setear la conexion y el comando en bd pasandole la conexion.
                using var conn = new NpgsqlConnection(connectionString);
                using var command = new NpgsqlCommand(procedureName, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                // Bucle. Agrega nombre del parametro + valor real en base al diccionario, (Acepta nulls).
                foreach (var kv in parameters)
                {
                    command.Parameters.AddWithValue(kv.Key, kv.Value ?? DBNull.Value);
                }

                // Crear parametro de salida y agregar al comando 
                var pResult = new NpgsqlParameter("presult", NpgsqlDbType.Varchar)
                {
                    Size = 1000,
                    Direction = System.Data.ParameterDirection.Output
                };
                command.Parameters.Add(pResult);

                // Abrir conexion y esperar
                await conn.OpenAsync();
                var affected = await command.ExecuteNonQueryAsync();
                var message = pResult.Value?.ToString() ?? "No message";

                // Verificar resultado (careful)
                if (affected > 0)
                {
                    result = OperationResult<string>.Success(message);
                }
                else
                {
                    result = OperationResult<string>.Failure(message);
                }

                logger.LogInformation("[{Procedure}] Result: {Message}", procedureName, message);
            }
            catch (Exception ex)
            {
                var error = $"Exception in {procedureName}: {ex.Message}";
                logger.LogError(ex, error);
                result = OperationResult<string>.Failure(error);
            }

            return result;
        }
    }

}
