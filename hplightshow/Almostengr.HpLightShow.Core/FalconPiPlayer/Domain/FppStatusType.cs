namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Domain;

public enum FppStatusType
{
    Idle = 0,
    Playing = 1,
    StoppingGracefully = 2,
    StoppingGracefullyAfterLoop = 3,
    Paused = 5,
}