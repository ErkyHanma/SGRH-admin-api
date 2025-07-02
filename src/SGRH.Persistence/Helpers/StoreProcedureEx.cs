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
            var result = new OperationResult<string>();

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
                if (pResult?.Value != null)
                {
                    message = pResult.Value.ToString();
                }
                else
                {
                    message = "No message";
                }

                // Verificar resultado
                if (!string.IsNullOrWhiteSpace(message) && message.ToLower().Contains("success") || affectedRows > 0)
                {
                    result = OperationResult<string>.Success(message);
                }
                else if (!string.IsNullOrWhiteSpace(message) && message.ToLower().Contains("success")) {
                   
                    result = OperationResult<string>.Success(message);
                }
                else
                {

                    result = OperationResult<string>.Failure(message);
                }

                logger.Info("Stored procedure {Procedure} executed. Message: {Message}", procedureName, message);
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




