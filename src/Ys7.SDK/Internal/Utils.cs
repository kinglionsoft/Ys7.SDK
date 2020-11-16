using System;
using System.IO;
using System.Threading.Tasks;

namespace Ys7.SDK.Internal
{
    internal static class Utils
    {
        public static string GetBase64OfFile(string file)
        {
            return Convert.ToBase64String(File.ReadAllBytes(file));
        }

        public static string GetBase64OfStream(Stream file)
        {
            var buffer = new byte[file.Length];
            file.Read(buffer, 0, buffer.Length);
            return Convert.ToBase64String(buffer);
        }

        public static async Task<string> GetBase64OfStreamAsync(Stream file)
        {
            var buffer = new byte[file.Length];
            await file.ReadAsync(buffer, 0, buffer.Length);
            return Convert.ToBase64String(buffer);
        }

        public static string EnsureImageFormat(string file)
        {
            if (!File.Exists(file)) throw new FileNotFoundException(file);

            var ext = Path.GetExtension(file);

            if (!ext.Equals(".JPEG", StringComparison.InvariantCultureIgnoreCase)
                && !ext.Equals(".JPG", StringComparison.InvariantCultureIgnoreCase)
                && !ext.Equals(".PNG", StringComparison.InvariantCultureIgnoreCase)
                && !ext.Equals(".BMP", StringComparison.InvariantCultureIgnoreCase)
                && !ext.Equals(".TIFF", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                throw new ArgumentOutOfRangeException(nameof(file), ext, "only JPEG, JPG, PNG, BMP, TIFF allowed");
            }

            return ext;
        }
    }
}