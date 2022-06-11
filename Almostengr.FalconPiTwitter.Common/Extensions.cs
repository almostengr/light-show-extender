using Almostengr.FalconPiTwitter.Common.Constants;

namespace Almostengr.FalconPiTwitter.Common.Extensions
{
    public static class Extensions
    {
        public static bool IsPlaylistIdleOfflineOrTesting(this string status)
        {
            return string.IsNullOrEmpty(status) ||
                status.Contains(PlaylistIgnoreName.Testing) ||
                status.Contains(PlaylistIgnoreName.Offline) ||
                status.Contains(PlaylistIgnoreName.Idle);
        }

        public static bool ContainsOfflineTestOrNull(this string value)
        {
            return value.Contains(PlaylistIgnoreName.Testing) || 
                value.Contains(PlaylistIgnoreName.Offline) || 
                string.IsNullOrEmpty(value);
        }
        
    }
}