using System.Collections.Generic;

namespace Almostengr.FalconPiTwitter.DataTransferObjects
{
    public class FalconFppdMultiSyncSystemsDto : BaseDto
    {
        public List<RemoteSystems> RemoteSystems { get; set; }
    }

    public class RemoteSystems
    {
        public string Address { get; set; }
        public string FppModeString { get; set; }
        public string Hostname { get; set; }
    }

}