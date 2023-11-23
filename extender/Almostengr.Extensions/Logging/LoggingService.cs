using Microsoft.Extensions.Logging;

namespace Almostengr.Extensions.Logging;

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

    public void Error(string message, params object[] args)
    {
        _logger.LogError(message, args);
    }

    public void Warning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void Information(string message, params object[] args)
    {
        #if !RELEASE
            _logger.LogInformation(message, args);
        #endif
    }

    public void Debug(string message, params object[] args)
    {
        #if !RELEASE
        _logger.LogDebug(message, args);
        #endif
    }
}