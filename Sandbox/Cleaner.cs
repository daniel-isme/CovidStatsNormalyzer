using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sandbox
{
    class Cleaner
    {
        public static async Task ClearRawText(string readPath, string writePath)
        {
            string text = "";

            // read from raw_text_data.txt and do regex
            try
            {
                // async read
                using (StreamReader sr = new StreamReader(readPath, System.Text.Encoding.Default))
                {
                    string line, date = "", stat = "";

                    Regex dateRegex = new Regex(@"\d{2}[.]\d{2}[.]\d{4}");
                    Regex statRegex = new Regex(@"[|].*[|]([|]\d*[|]){3}[|]\d*");

                    while ((line = sr.ReadLine()) != null) // reading one line
                    {
                        if (line == "") continue;

                        Match match = dateRegex.Match(line);
                        if (match.Success)
                        {
                            date = match.Value;
                            continue;
                        }

                        match = statRegex.Match(line);
                        if (match.Success)
                        {
                            stat = match.Value;
                            stat = splitStat(stat);
                            text += date + " " + stat + "\n";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            File.WriteAllText(writePath, text);

        }



        private static string splitStat(string rawStat)
        {
            string clearStat = "", region = "";
            string[] statNums = new string[4];

            int i = 0;
            while (i < rawStat.Length)
            {
                if (rawStat[i] == '[')
                {
                    i += 2;
                    while (rawStat[i] != ']')
                    {
                        region += rawStat[i];
                        i++;
                    }
                    i += 4;
                    int current = 0;
                    while (current < 4)
                    {
                        while (i < rawStat.Length && rawStat[i] != '|')
                        {
                            statNums[current] += rawStat[i];
                            i++;
                        }
                        if (statNums[current] == null)
                        {
                            statNums[current] = "0";
                        }
                        current++;
                        i += 2;
                    }
                }

                i++;
            }

            foreach (string statNum in statNums)
            {
                clearStat += statNum + " ";
            }
            clearStat += region;

            return clearStat;
        }
    }
}
