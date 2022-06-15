namespace Almostengr.FalconPiTwitter.Common.Services
{
    public abstract class BaseService : IBaseService
    {
        public virtual double GetRandomWaitTime()
        {
            Random random = new();
            double waitHours = 0;

            while (waitHours < 3)
            {
                waitHours = 7 * random.NextDouble();
            }

            return waitHours;
        }
    }
}