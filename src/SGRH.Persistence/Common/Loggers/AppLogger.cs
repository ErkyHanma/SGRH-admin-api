using Microsoft.Extensions.Logging;

namespace SGRH.Application.Common.Logging
{
    public class AppLogger<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public AppLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        // Registra error con excepcion, incluyendo su [Type] o tipo de clase
        public void ErrorEx(Exception ex, string message, params object[] args)
        {
            _logger.LogError(ex, "[{Type}] " + message, typeof(T).Name, args);
        }

        // Registra error sin excepcion, incluyendo su [Type] o tipo de clase
        public void ErrorNoEx(string message, params object[] args)
        {
            _logger.LogError("[{Type}] " + message, typeof(T).Name, args);
        }

        // Registra mensaje de informe, incluyendo su [Type] o tipo de clase
        public void Info(string message, params object[] args)
        {
            var newArgs = new object[] { typeof(T).Name }
         .Concat(args ?? Array.Empty<object>())
         .ToArray();

            _logger.LogInformation("[{Type}] " + message, newArgs);
        }


    }

}