namespace Almostengr.FalconPiTwitter.Common.DataTransferObjects
{
    public class FalconFppdMultiSyncSystemsDto : BaseDto
    {
        public List<RemoteSystems> Systems { get; init; }
    }

    public class RemoteSystems
    {
        public string Address { get; init; }
        public string FppModeString { get; init; }
        public string Hostname { get; init; }
    }

}