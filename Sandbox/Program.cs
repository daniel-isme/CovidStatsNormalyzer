using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sandbox
{
    class Program
    {

        public static void Main()
        {
            var folder = @"C:\Users\danii\source\repos\Sandbox\Sandbox\AllDataFiles\TextData\";

            string badRegions = @"bad_regions.txt";
            string rawTextData = @"raw_text_data.txt";
            string clearedRaw = @"cleared_raw.txt";
            string normalized = @"normalized.txt";

            Cleaner.CorrectRegNames(
                badRegionsPath: folder + badRegions,
                dataPath: folder + rawTextData);

            Cleaner.ClearRawText(
                readPath: folder + rawTextData,
                writePath: folder + clearedRaw);

            StatNormalizer.Normalize(
                readPath: folder + clearedRaw,
                writePath: folder + normalized);

        }
    }
}
