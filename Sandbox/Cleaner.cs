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
            // async read
            using (StreamReader sr = new StreamReader(readPath, Encoding.Default))
            {
                string line, date = "", stat = "";

                Regex dateRegex = new Regex(@"\d+");
                Regex rawDateRegexApr = new Regex(@"в России на \d* апр");
                Regex rawDateRegexMay = new Regex(@"в России на \d* ма");
                Regex statRegex = new Regex(@"[А-Я].*[/] *\d* *[/] *\d*");

                while ((line = sr.ReadLine()) != null) // reading one line
                {
                    if (line == "") continue;

                    Match matchApr = rawDateRegexApr.Match(line);
                    Match matchMay = rawDateRegexMay.Match(line);
                    if (matchApr.Success)
                    {
                        var dayMatch = dateRegex.Match(matchApr.Value);
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

                    if (matchMay.Success)
                    {
                        var dayMatch = dateRegex.Match(matchMay.Value);
                        if (dayMatch.Success)
                        {
                            var day = dayMatch.Value;
                            if (day.Length == 1)
                            {
                                day = "0" + day;
                            }
                            date = $"{day}.05.2020";
                            continue;
                        }
                    }

                    var matchStat = statRegex.Match(line);
                    if (matchStat.Success)
                    {
                        stat = matchStat.Value;
                        stat = splitStat(stat);
                        text += date + " " + stat + "\n";
                    }

                }
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
