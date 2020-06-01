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
                Regex rawDateRegexJun = new Regex(@"в России на \d* июн");
                Regex statRegex = new Regex(@"[А-Я].*[/] *\d* *[/] *\d*");

                while ((line = sr.ReadLine()) != null) // reading one line
                {
                    if (line == "") continue;

                    Match matchApr = rawDateRegexApr.Match(line);
                    Match matchMay = rawDateRegexMay.Match(line);
                    Match matchJun = rawDateRegexJun.Match(line);
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

                    if (matchJun.Success)
                    {
                        var dayMatch = dateRegex.Match(matchJun.Value);
                        if (dayMatch.Success)
                        {
                            var day = dayMatch.Value;
                            if (day.Length == 1)
                            {
                                day = "0" + day;
                            }
                            date = $"{day}.06.2020";
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

        public static void CorrectRegNames(string badRegionsPath, string dataPath)
        {
            using (StreamReader sr1 = new StreamReader(badRegionsPath, Encoding.Default))
            {
                string line;

                Regex badRegionRegex = new Regex(@"[А-Я].*[*]_[-]_[*]");
                Regex goodRegionRegex = new Regex(@"[*]_[-]_[*][А-Я].*");
                Console.OutputEncoding = Encoding.UTF8;

                while ((line = sr1.ReadLine()) != null) // reading one line
                {
                    if (line == "") continue;
                    string badRegName = badRegionRegex.Match(line).Value;
                    if (badRegName.Length < 1) continue;
                    badRegName = badRegName.Substring(0, badRegName.Length - 5);
                    string goodRegName = goodRegionRegex.Match(line).Value;
                    goodRegName = goodRegName.Substring(5, goodRegName.Length - 5);

                    string text = File.ReadAllText(dataPath);
                    text = text.Replace(badRegName, goodRegName);
                    File.WriteAllText(dataPath, text);
                }
            }
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
