using System.IO.Compression;
using System.Text;

namespace SFA.DAS.Payments.Automation.WebUI
{
    public static class ZipExtensions
    {
        public static void AddEntry(this ZipArchive zip, string contents, string name)
        {
            zip.AddEntry(Encoding.UTF8.GetBytes(contents), name);
        }
        public static void AddEntry(this ZipArchive zip, byte[] data, string name)
        {
            var zipEntry = zip.CreateEntry(name);
            using (var stream = zipEntry.Open())
            {
                stream.Write(data, 0, data.Length);
                stream.Flush();
                stream.Close();
            }
        }
    }
}