using System;

namespace Almostengr.FalconPiTwitter.Services
{
    public interface IBaseService
    {
        string CalculateTimeBetween(DateTime startDate, DateTime endDate);
        double GetRandomWaitTime();
    }
}