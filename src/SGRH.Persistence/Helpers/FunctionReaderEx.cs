using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Persistence.Helpers
{
    public static class FunctionReaderEx
    {
        public static async Task<List<T>> CallFunctionAsync<T>(
            string connectionString,
            string functionName,
            Func<NpgsqlDataReader, T> functionMapper, //Para mapear parametros
            Dictionary<string, object>? parameters = null) // Puede ser null para buscar por id
        { 
            var result = new List<T>();

            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand(functionName, connection)
                {
                    CommandType = CommandType.Text
                };

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
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
                throw new Exception($"Error ejecutando la función SQL: {ex.Message}", ex);
            }

        }

    }
}
