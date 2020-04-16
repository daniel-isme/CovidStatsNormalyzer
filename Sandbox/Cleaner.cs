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
        public static void ClearRawText(string readPath, string writePath)
        {
            string text = "";

            // read from raw_text_data.txt and do regex
            try
            {
                // async read
                using (StreamReader sr = new StreamReader(readPath, Encoding.Default))
                {
                    string line, date = "", stat = "";

                    Regex dateRegex = new Regex(@"\d+");
                    Regex rawDateRegex = new Regex(@"в России на \d* апр");
                    Regex statRegex = new Regex(@"[А-Я].*[/] *\d* *[/] *\d*");

                    while ((line = sr.ReadLine()) != null) // reading one line
                    {
                        if (line == "") continue;

                        Match match = rawDateRegex.Match(line);
                        if (match.Success)
                        {
                            var dayMatch = dateRegex.Match(match.Value);
                            if (dayMatch.Success)
                            {
                                var day = dayMatch.Value;
                                if (day.Length == 1)
                                {
                                    day = "0" + day;
                                }
                                date = $"{day}.04.2020";
                                continue;
                            }
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
            string clearStat = "";
            var statNums = new List<string>();
            Regex dateRegex = new Regex(@"\d+");
            Regex regionRegex = new Regex(@"[А-Я].* – ");

            foreach(Match match in dateRegex.Matches(rawStat))
            {
                statNums.Add(match.Value);
            }

            var region = regionRegex.Match(rawStat).Value;
            region = region.Remove(region.Length - 3);

            foreach (string statNum in statNums)
            {
                clearStat += statNum + " ";
            }
            clearStat += region;

            return clearStat;
        }
    }
}
