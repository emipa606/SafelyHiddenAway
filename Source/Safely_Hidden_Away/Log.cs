using System.Diagnostics;

namespace Safely_Hidden_Away
{
    internal static class Log
    {
        [Conditional("DEBUG")]
        public static void Message(string x)
        {
            Verse.Log.Message(x);
        }
    }
}