using Almostengr.FalconPiTwitter.Common.Constants;

namespace Almostengr.FalconPiTwitter.Common.Extensions
{
    public static class Extensions
    {
        public static bool ContainsIdleOfflineOrTesting(this string value)
        {
            value = value.ToLower();
            return string.IsNullOrEmpty(value) ||
                value.Contains(PlaylistIgnoreName.Testing) ||
                value.Contains(PlaylistIgnoreName.Offline) ||
                value.Contains(PlaylistIgnoreName.Idle);
        }

        public static bool ContainsOfflineTestOrNull(this string value)
        {
            value = value.ToLower();
            return value.Contains(PlaylistIgnoreName.Testing) ||
                value.Contains(PlaylistIgnoreName.Offline) ||
                string.IsNullOrEmpty(value);
        }

        public static string GetSongNameFromFileName(this string value)
        {
            value = value.ToLower();
            return value.Replace(".mp3", "").Replace(".m4a", "").Replace(".ogg", "")
              .Replace(".mp4", "").Replace("_", " ").Replace("-", " ");
        }

        public static bool IsNull<T>(this T value)
        {
            if (value == null)
            {
                return true;
            }

            return false;
        }

        public static bool IsRemoteInstance(this string value)
        {
            if (value == FppMode.Remote)
            {
                return true;
            }

            return false;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string ToDoubleString(this double value)
        {
            return value.ToString("0.00");
        }

    }
}