using Microsoft.Extensions.Logging;
using Npgsql;
using SGRH.Domain.Base;
using System.Data;

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
                // Crear la conexión y el comando en BD pasándole la conexión.
                using var connection = new NpgsqlConnection(connectionString);
                using var command = new NpgsqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Bucle. Agrega nombre del parámetro + valor real en base al diccionario (acepta nulls).
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                }

                // Crear parámetro de salida y agregar al comando.
                var pResult = new NpgsqlParameter("presult", NpgsqlTypes.NpgsqlDbType.Text)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(pResult);

                // Abrir conexión y esperar
                await connection.OpenAsync();
                var affectedRows = await command.ExecuteNonQueryAsync();

                var message = pResult.Value?.ToString() ?? "No message";

                // Verificar resultado
                if (affectedRows > 0)
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


