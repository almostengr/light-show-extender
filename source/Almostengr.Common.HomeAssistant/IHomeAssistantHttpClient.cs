namespace Almostengr.Common.HomeAssistant;

public interface IHomeAssistantHttpClient
{
    Task<HaSwitchResponseDto> TurnOffSwitchAsync(HaSwitchRequestDto requestDto);
    Task<HaSwitchResponseDto> TurnOnSwitchAsync(HaSwitchRequestDto requestDto);
}