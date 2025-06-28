using Npgsql;
using System.Data;

namespace SGRH.Persistence.Helpers
{
    public static class FunctionReaderEx
    {
        public static async Task<List<T>> CallFunctionAsync<T>(
            string connectionString,
            string functionName,
            Func<NpgsqlDataReader, T> functionMapper, //Para mapear parametros
            Dictionary<string, object>? parameters = null) // Puede ser null, solo para buscar por id
        {
            var result = new List<T>();

            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand(functionName, connection)
                {
                    CommandType = CommandType.Text // Definimos como .Text a falta de una alternativa para funciones
                };

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        var npgsqlParam = CreateNpgsqlParameter(parameter.Key, parameter.Value); //Llamamos a la funcion
                        command.Parameters.Add(npgsqlParam);
                    }
                }

                // Ejecuta la consulta y obtiene un reader para procesar resultados
                using var reader = await command.ExecuteReaderAsync();

                // Itera cada fila, transforma en tipo T y agrega a la lista resultado
                while (await reader.ReadAsync())
                {
                    result.Add(functionMapper(reader));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing SQL function: {ex.Message}", ex);
            }
        }
        // Esta funcion nos permite asignar un tipo explicito como parametro de entrada para la funcion BD 
        private static NpgsqlParameter CreateNpgsqlParameter(string name, object? value)
        {
            var parameter = new NpgsqlParameter(name, value ?? DBNull.Value); // Crear parametro de consulta (maneja null)

            if (value is DateTime)
            {
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date;
            }
            else if (value is int)
            {
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer;
            }
            else if (value is decimal)
            {
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Numeric;
            }
            else if (value is bool)
            {
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean;
            }
            else if (value is string)
            {
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar;
            }

            return parameter;
        }

    }
}
