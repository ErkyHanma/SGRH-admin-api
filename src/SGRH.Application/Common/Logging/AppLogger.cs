using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace SGRH.Application.Common.Logging
{
    public class AppLogger<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public AppLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void ErrorEx(Exception ex, string message, params object[] args)
        {
            _logger.LogError(ex, "[{Type}] {Message}", typeof(T).Name, FormatMessage(message, args));
        }

        public void ErrorNoEx(string message, params object[] args)
        {
            _logger.LogError("[{Type}] {Message}", typeof(T).Name, FormatMessage(message, args));
        }

        public void Info(string message, params object[] args)
        {
            var newArgs = new object[] { typeof(T).Name }.Concat(args ?? Array.Empty<object>()).ToArray();
            _logger.LogInformation("[{Type}] {Message}", newArgs.Prepend(message).ToArray());
        }

        private string FormatMessage(string message, object[] args)
        {
            if (args == null || args.Length == 0)
                return message;

            try
            {
                return string.Format(message, args.Select(SafeSerialize).ToArray());
            }
            catch
            {
                return message + " [Argument formatting failed]";
            }
        }

        private string SafeSerialize(object obj)
        {
            try
            {
                return obj == null ? "null" : JsonSerializer.Serialize(obj);
            }
            catch
            {
                return obj?.ToString() ?? "null";
            }
        }
    }
}