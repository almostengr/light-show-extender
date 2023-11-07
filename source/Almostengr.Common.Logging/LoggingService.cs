using Microsoft.Extensions.Logging;

namespace Almostengr.Common.Logging;

public sealed class LoggingService<T> : ILoggingService<T> where T : class
{
    private readonly ILogger<T> _logger;

    public LoggingService(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void Error(Exception? exception, string message, params object[] args)
    {
        _logger.LogError(exception, message, args);
    }

    public void Warning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void Information(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void Debug(string message, params object[] args)
    {
        _logger.LogDebug(message, args);
    }
}