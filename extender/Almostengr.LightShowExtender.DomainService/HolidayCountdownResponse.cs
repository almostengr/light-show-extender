using Almostengr.Common.Command;

namespace Almostengr.LightShowExtender.Worker.DomainService;

public sealed class HolidayCountdownResponse : CommandResponse, ICommandResponse
{
    public HolidayCountdownResponse(bool succeeded, object? data = null, string message = "") : base(succeeded, data, message)
    {
    }
}