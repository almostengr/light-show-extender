namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.Domain;

public enum FppStatusOption
{
    Idle = 0,
    Playing = 1,
    StoppingGracefully = 2,
    StoppingGracefullyAfterLoop = 3,
    Paused = 5,
}
