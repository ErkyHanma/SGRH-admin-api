using Npgsql;
using SGRH.Common.Common;
using SGRH.Domain.Base;
using System.Data;

namespace SGRH.Persistence.Helpers
{
    public static class StoreProcedureEx
    {
        public static async Task<OperationResult<string>> ExecuteAsync<T>(
            string connectionString,
            string procedureName,
            Dictionary<string, object> parameters,
            IAppLogger<T> logger)
        {
            var result = OperationResult<string>.Failure("Operación no completada. Mensaje por defecto.");

            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                using var command = new NpgsqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                }

                var pResult = new NpgsqlParameter("presult", NpgsqlTypes.NpgsqlDbType.Text)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(pResult);

                await connection.OpenAsync();
                var affectedRows = await command.ExecuteNonQueryAsync();

                var message = pResult?.Value != null && pResult.Value != DBNull.Value
                    ? pResult.Value.ToString()
                    : "No message provided by stored procedure.";

                var isSuccess =
                    !string.IsNullOrWhiteSpace(message) &&
                    (message.ToLower().Contains("exitosamente") ||
                     message.ToLower().Contains("correctamente") ||
                     message.ToLower().Contains("success") ||
                     affectedRows > 0);

                result = isSuccess
                    ? OperationResult<string>.Success(message)
                    : OperationResult<string>.Failure(message);

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
