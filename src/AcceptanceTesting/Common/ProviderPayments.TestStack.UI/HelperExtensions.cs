using System;
using System.IO;
using System.Threading.Tasks;

namespace ProviderPayments.TestStack.UI
{
    public static class HelperExtensions
    {
        public static async Task<byte[]> ReadAllBytesAsync(this Stream stream)
        {
            var buffer = new byte[stream.Length];
            var bufferPosition = 0;
            var readBuffer = new byte[1024];
            int count;

            while ((count = await stream.ReadAsync(readBuffer, 0, readBuffer.Length)) > 0)
            {
                Array.Copy(readBuffer, 0, buffer, bufferPosition, count);
                bufferPosition += count;
            }

            return buffer;
        }
    }
}