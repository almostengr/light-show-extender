namespace Almostengr.FalconPiPlayerClient.Domain;

public enum FppStatusOption
{
    Idle = 0,
    Playing = 1,
    StoppingGracefully = 2,
    StoppingGracefullyAfterLoop = 3,
    Paused = 5,
}