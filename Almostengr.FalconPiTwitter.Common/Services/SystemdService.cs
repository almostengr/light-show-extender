using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Common.Services
{
    public class SystemdService : ISystemdService
    {
        private const string FPT_SERVICE_FILE = "/lib/systemd/system/falconpitwitter.service";
        private const string SYSD_MESSAGE = 
            "This application has not been configured as a system service. See https://thealmostengineer.com/projects/falcon-pi-twitter/#installation-steps for more info.";
        private readonly ILogger<SystemdService> _logger;

        public SystemdService(ILogger<SystemdService> logger)
        {
            _logger =logger;
        }

        public void CheckForSystemdServiceFile()
        {
            if (File.Exists(FPT_SERVICE_FILE) == false)
            {
                _logger.LogWarning(SYSD_MESSAGE);
            }
        }

    }
}