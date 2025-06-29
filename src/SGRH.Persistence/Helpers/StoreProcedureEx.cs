using Microsoft.Extensions.Logging;
using Npgsql;
using SGRH.Application.Common.Logging;
using SGRH.Domain.Base;
using System.Data;

namespace SGRH.Persistence.Helpers
{
    public static class StoreProcedureEx
    {
        public static async Task<OperationResult<string>> ExecuteAsync<T>(
            string connectionString,
            string procedureName,
            Dictionary<string, object> parameters, // Nombre del procedimiento + Dto con un valor
            IAppLogger<T> logger)
        {
            // Inicializar result con un fallo por defecto. Si una excepción ocurre antes de cualquier asignación
            // en el try, este será el valor inicial. El catch lo sobrescribirá si hay una excepción.
            var result = OperationResult<string>.Failure("Operación no completada. Mensaje por defecto.");

            try
            {
                // Crear la conexion y el comando en BD pasándole la conexion.
                using var connection = new NpgsqlConnection(connectionString);
                using var command = new NpgsqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Bucle. Agrega nombre del parametro (key) + valor real (value) en base al diccionario (acepta nulls).
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                }

                // Crear parametro de salida y agregar al comando.
                var pResult = new NpgsqlParameter("presult", NpgsqlTypes.NpgsqlDbType.Text)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(pResult);

                // Abrir conexion y esperar
                await connection.OpenAsync();
                var affectedRows = await command.ExecuteNonQueryAsync();

                // Crear variable y verificar si pResult y su Value no son nulos.
                string message;
                if (pResult?.Value != null && pResult.Value != DBNull.Value)
                {
                    message = pResult.Value.ToString();
                }
                else
                {
                    message = "No message provided by stored procedure.";
                }

            
                if (!string.IsNullOrWhiteSpace(message) &&
                    (message.ToLower().Contains("exitosamente") || message.ToLower().Contains("correctamente")))
                {
                    result = OperationResult<string>.Success(message);
                }
                // condición para `affectedRows` para SPs
                // else if (affectedRows > 0)
                // {
                //     result = OperationResult<string>.Success(message);
                // }
                else
                {
                   
                    result = OperationResult<string>.Failure(message);
                }

                logger.Info("Stored procedure {Procedure} executed. Message: {Message}. Affected rows: {AffectedRows}", procedureName, message, affectedRows);
            }
            catch (Exception ex)
            {
                var error = $"Exception in {procedureName}: {ex.Message}";
                logger.ErrorEx(ex, error);
                result = OperationResult<string>.Failure(error);
            }

            return result;
        }
    }
}