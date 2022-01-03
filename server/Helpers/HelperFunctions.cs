using System;

namespace server.Helpers
{
    public class HelperFunctions
    {
        public static string TruncateLongString(string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str)) return str;

            int minLen = Math.Min(str.Length, maxLength);
            string subStr = str.Substring(0, minLen);
            
            if(minLen == str.Length) return subStr;
            return subStr+"...";
        }
    }
}