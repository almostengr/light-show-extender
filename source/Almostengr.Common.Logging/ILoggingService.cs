namespace Almostengr.Common.Logging;

public interface ILoggingService<T>
{
    void Debug(string message, params object[] args);
    void Error(Exception exception, string message, params object[] args);
    void Information(string message, params object[] args);
    void Warning(string message, params object[] args);
}