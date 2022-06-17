using Almostengr.FalconPiTwitter.Common.DataTransferObjects;

namespace Almostengr.FalconPiTwitter.Common.Services
{
    public class FptSettingsService : IFptSettingsService
    {
        public FptSettingsDto GetFptSettings()
        {
            throw new NotImplementedException();

            // if file exists, the read and return file contents
        }

        public void UpsertSettings(FptSettingsDto fptSettingsDto)
        {
            throw new NotImplementedException();

            // if file does not exist, then insert contents from request
            // if file does exist, then update contents
        }
    }
}