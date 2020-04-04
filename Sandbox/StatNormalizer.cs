using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sandbox
{
    class StatNormalizer
    {
        class Stat
        {
            public DateTime Date;
            public int Cases;
            public int Deaths;
            public int Recovered;
        }
        class Region
        {
            public string Name;
            public List<Stat> Stats;
        }

        public static void Normalize(string readPath, string writePath)
        {
            List<Region> regions = new List<Region>();

            string text = "";
            string line, regName;

            StreamReader sr = new StreamReader(readPath, System.Text.Encoding.Default);
            while ((line = sr.ReadLine()) != null) // reading one line
            {
                if (line == "") continue;
                string[] stat = line.Split(' ');
                Regex regionRegex = new Regex(@"[А-Я].*");
                regName = regionRegex.Match(line).Value;
                bool regionExist = false;

                foreach (Region region in regions)
                {
                    if (region.Name == regName)
                    {
                        regionExist = true;
                        break;
                    }
                }

                if (regionExist)
                {
                    regions.First(i => i.Name == regName)
                        .Stats
                        .Add(new Stat
                        {
                            Date = DateTime.ParseExact(stat[0], "dd.MM.yyyy", CultureInfo.InvariantCulture),
                            Cases = int.Parse(stat[1]),
                            Deaths = int.Parse(stat[2]),
                            Recovered = int.Parse(stat[3])
                        });
                }
                else
                {
                    regions.Add(new Region
                    {
                        Name = regName,
                        Stats = new List<Stat>
                        {
                            new Stat
                            {
                                Date = DateTime.ParseExact(stat[0], "dd.MM.yyyy", CultureInfo.InvariantCulture),
                                Cases = int.Parse(stat[1]),
                                Deaths = int.Parse(stat[2]),
                                Recovered = int.Parse(stat[3])
                            }
                        }
                    });
                }
            }

            foreach (Region region in regions)
            {
                for (int i = region.Stats.Count - 1; i > 0; i--)
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        region.Stats[j].Cases -= region.Stats[i].Cases;
                        region.Stats[j].Deaths -= region.Stats[i].Deaths;
                        region.Stats[j].Recovered -= region.Stats[i].Recovered;
                    }
                }
            }

            foreach (Region region in regions)
            {
                text += region.Name + "\n";
                foreach (Stat stat in region.Stats)
                {
                    text += $"{stat.Date} {stat.Cases} {stat.Deaths} {stat.Recovered}\n";
                }
                text += "\n\n";
            }

            File.WriteAllText(writePath, text);

            //// write to output
            //try
            //{
            //    using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            //    {
            //        sw.WriteLine(text);
            //    }
            //    Console.WriteLine("Normalizer");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }
    }
}
