using Almostengr.FalconPiTwitter.Common.DataTransferObjects;

namespace Almostengr.FalconPiTwitter.Common.Services
{
    public interface IFptSettingsService
    {
        FptSettingsDto GetFptSettings();
        void UpsertSettings(FptSettingsDto fptSettingsDto);
    }
}