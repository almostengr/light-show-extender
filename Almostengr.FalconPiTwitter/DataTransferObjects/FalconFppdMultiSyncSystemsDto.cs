using System.Collections.Generic;
using Almostengr.FalconPiTwitter.Common.Constants;

namespace Almostengr.FalconPiTwitter.DataTransferObjects
{
    public class FalconFppdMultiSyncSystemsDto : BaseDto
    {
        public List<RemoteSystems> RemoteSystems { get; set; }
    }

    public class RemoteSystems
    {
        public string Address { get; set; }
        public string Version { get; set; }
        public string FppModeString { get; set; }
        public string Hostname { get; set; }
    }
}