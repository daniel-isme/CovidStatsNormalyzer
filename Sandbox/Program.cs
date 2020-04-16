using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sandbox
{
    class Program
    {
        static void Main()
        {
            var folder = @"C:\Users\danii\source\repos\Sandbox\Sandbox\AllDataFiles\TextData\";
            Cleaner.ClearRawText(
                readPath: folder + @"raw_text_data.txt",
                writePath: folder + @"cleared_raw.txt");

            StatNormalizer.Normalize(
                readPath: folder + @"cleared_raw.txt",
                writePath: folder + @"normalized.txt");
        }
    }
}
