using System.Text;
using RhtServices.Common.Utilities.DomainService;
using RhtServices.FalconPiPlayer.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.FppMonitor;

public sealed class FppMonitorHandler : IHandler<HandlerResult>
{
    private readonly IFppHttpClient _fppHttpClient;

    public FppMonitorHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task<HandlerResult> ExecuteAsync()
    {
        FppStatusResult fppStatus = await _fppHttpClient.GetFppdStatusAsync();
        if (fppStatus == null)
        {
            return new HandlerResult(false);
        }

        if (fppStatus.Status == (int)FppStatusTypes.Idle)
        {
            return new HandlerResult(true);
        }

        StringBuilder errors = new();

        if (fppStatus.Warnings.Count > 0)
        {
            errors.Append("Warnings were found.");
        }

        if (errors.Length > 0)
        {
            throw new Exception(errors.ToString());
        }

        return new HandlerResult(true);
    }
}
