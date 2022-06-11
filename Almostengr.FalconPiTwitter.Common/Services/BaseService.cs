namespace Almostengr.FalconPiTwitter.Common.Services
{
    public abstract class BaseService : IBaseService
    {
        public double GetRandomWaitTime()
        {
            Random random = new();
            double waitHours = 0;

            while (waitHours < 0.75)
            {
                waitHours = 7 * random.NextDouble();
            }

            return waitHours;
        }
    }
}