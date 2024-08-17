namespace Almostengr.Extensions;

public class QueryResponse : IQueryResponse
{
    public readonly string _message;
    public readonly bool _succeeded;
    public readonly object? _data;

    public QueryResponse(bool succeeded, object? data = null, string message = "")
    {
        _succeeded = succeeded;
        _data = data;
        _message = message;
    }
}
