using Almostengr.FalconPiTwitter.Common.Constants;

namespace Almostengr.FalconPiTwitter.Common.Extensions
{
    public static class Extensions
    {
        public static bool IsPlaylistIdleOfflineOrTesting(this string value)
        {
            return string.IsNullOrEmpty(value) ||
                value.Contains(PlaylistIgnoreName.Testing) ||
                value.Contains(PlaylistIgnoreName.Offline) ||
                value.Contains(PlaylistIgnoreName.Idle);
        }

        public static bool ContainsOfflineTestOrNull(this string value)
        {
            return value.Contains(PlaylistIgnoreName.Testing) ||
                value.Contains(PlaylistIgnoreName.Offline) ||
                string.IsNullOrEmpty(value);
        }

        public static string SongNameFromFileName(this string value)
        {
            return value.Replace(".mp3", "").Replace(".m4a", "").Replace(".ogg", "")
              .Replace(".mp4", "").Replace("_", " ").Replace("-", " ");
        }

    }
}