namespace Almostengr.FalconPiTwitter.DataTransferObjects
{
    public class FalconFppdMultiSyncSystemsDto : BaseDto
    {
        public List<RemoteSystems> RemoteSystems { get; init; }
    }

    public class RemoteSystems
    {
        public string Address { get; init; }
        public string FppModeString { get; init; }
        public string Hostname { get; init; }
    }

}