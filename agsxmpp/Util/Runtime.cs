using System;

namespace agsXMPP.Util
{
    internal static class Runtime
    {
        public static bool IsMono()
        {
            var t = Type.GetType("Mono.Runtime");
            if (t != null)
                return true;

            return false;
        }

        public static bool IsUnix()
        {
            var p = (int) Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128))
                return true;

            return false;
        }
    }
}