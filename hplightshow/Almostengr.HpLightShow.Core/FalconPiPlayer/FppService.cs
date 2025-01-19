namespace Almostengr.HpLightShow.Core.FalconPiPlayer;

public sealed class FppService
{
    public async Task<HandlerResult> MonitorAsync()
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