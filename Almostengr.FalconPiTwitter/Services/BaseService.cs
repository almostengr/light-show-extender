using System;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Services
{
    public abstract class BaseService : IBaseService
    {
        private readonly ILogger<BaseService> _logger;
        internal int AlarmCount = 0;
        internal Random Random = new Random();

        public BaseService(ILogger<BaseService> logger)
        {
            _logger = logger;
        }

        public string CalculateTimeBetween(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeDiff = endDate - startDate;
            _logger.LogDebug(timeDiff.ToString());

            string output = string.Empty;
            output += (timeDiff.Days > 0 ? (timeDiff.Days + (timeDiff.Days == 1 ? " day " : " days ")) : string.Empty);
            output += (timeDiff.Hours > 0 ? (timeDiff.Hours + (timeDiff.Hours == 1 ? " hour " : " hours ")) : string.Empty);
            output += (timeDiff.Minutes > 0 ? (timeDiff.Minutes + (timeDiff.Minutes == 1 ? " minute " : " minutes ")) : string.Empty);

            return output;
        }

        public void ResetAlarmCount()
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            
            if (AlarmCount > 0 && currentTime.Minutes >= 55)
            {
                _logger.LogWarning($"Alarm count reset. Previous count: {AlarmCount}");
                AlarmCount = 0;
            }
        }

    }
}