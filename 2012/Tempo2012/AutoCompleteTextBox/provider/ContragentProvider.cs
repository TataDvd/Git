using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dragonz.actb.provider
{
    public class ContragentProvider : IAutoCompleteDataProvider
    {
        private List<string> Numbers;
        private List<string> Bulstats;
        private List<string> Names;
        public ContragentProvider(IEnumerable<string> list)
        {
             Numbers=new List<string>();
             Bulstats=new List<string>();
             Names=new List<string>();
            foreach (string item in list)
            {
                var s = item.Split('|');
                Numbers.Add(s[0]);
                Bulstats.Add(s[1]);
                Names.Add(s[2]);
            }
        }
        public IEnumerable<string> GetItems(string textPattern)
        {
            if (textPattern.Contains("#"))
            {
                for (int i = 0; i < Bulstats.Count; i++)
                {
                    if (Bulstats[i].StartsWith(textPattern))
                        yield return string.Format("{0} {1} {2}", Numbers[i], Bulstats[i].Replace("#", ""), Names[i]);
                }
            }
            else
            {
                int civra;
                if (int.TryParse(textPattern,out civra))
                {
                    for (int i = 0; i < Numbers.Count; i++)
                    {
                        if (Numbers[i].StartsWith(textPattern))
                            yield return string.Format("{0} {1} {2}", Numbers[i], Bulstats[i].Replace("#",""), Names[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < Names.Count; i++)
                    {
                        if (Names[i].Contains(textPattern))
                            yield return string.Format("{0} {1} {2}", Numbers[i], Bulstats[i].Replace("#", ""), Names[i]);
                    }
                }
            }

            
        }
    }
}
