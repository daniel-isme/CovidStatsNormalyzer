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
            _ = Cleaner.ClearRawText(
                readPath: @"C:\Users\danii\source\repos\Sandbox\Sandbox\AllDataFiles\TextData\raw_text_data.txt",
                writePath: @"C:\Users\danii\source\repos\Sandbox\Sandbox\AllDataFiles\TextData\cleared_raw.txt");

            StatNormalizer.Normalize(
                readPath: @"C:\Users\danii\source\repos\Sandbox\Sandbox\AllDataFiles\TextData\cleared_raw.txt",
                writePath: @"C:\Users\danii\source\repos\Sandbox\Sandbox\AllDataFiles\TextData\normalized.txt");
        }
    }
}
