using System;
using System.IO;
using System.Text;
using Ionic.Zlib;

namespace Decompress
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && File.Exists(args[0]))
            {
                string path = args[0];
                using (StreamReader sr = new StreamReader(path))
                {
                    // Decompress the old packet..
                    Stream dataStream = sr.BaseStream;
                    MemoryStream decompressed = new MemoryStream();

                    using (ZlibStream zlibStream = new ZlibStream(dataStream, CompressionMode.Decompress))
                    {
                        zlibStream.CopyTo(decompressed);
                    }

                    byte[] decompressedData = decompressed.ToArray();

                    FileStream output = new FileStream("output.bin", FileMode.Create);
                    output.Write(decompressedData, 0, decompressedData.Length);
                    output.Close();
                }
            }
        }
    }
}
