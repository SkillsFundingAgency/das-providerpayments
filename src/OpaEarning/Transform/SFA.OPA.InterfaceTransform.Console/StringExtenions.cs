using System;

namespace SFA.OPA.InterfaceTransform.Console
{
    public static class StringExtenions
    {
        public static string ReplaceCaseInsensitive(this string originalString, string oldValue, string newValue)
        {
            int startIndex = 0;
            while (true)
            {
                startIndex = originalString.IndexOf(oldValue, startIndex, StringComparison.InvariantCultureIgnoreCase);
                if (startIndex == -1)
                    break;

                originalString = originalString.Substring(0, startIndex) + newValue + originalString.Substring(startIndex + oldValue.Length);

                startIndex += newValue.Length;
            }

            return originalString;
        }
      
    }
}
