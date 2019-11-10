using System;
using System.Linq;

namespace Kuznechik_Console
{
    public static class TransformOperations
    {
        public static byte[] GetBytes(this string text)
        {
            return Enumerable.Range(0, text.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(text.Substring(x, 2), 16))
                .ToArray();
        }

        public static string GetString(this byte[] bytesArray)
        {
            return BitConverter.ToString(bytesArray).Replace("-", "");
        }
    }
}
