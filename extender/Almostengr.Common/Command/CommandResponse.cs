namespace Almostengr.Common.Command;

public class CommandResponse : ICommandResponse
{
    public readonly string _message;
    public readonly bool _succeeded;
    public readonly object? _data;

    public CommandResponse(bool succeeded, object? data = null, string message = "")
    {
        _succeeded = succeeded;
        _data = data;
        _message = message;
    }
}
