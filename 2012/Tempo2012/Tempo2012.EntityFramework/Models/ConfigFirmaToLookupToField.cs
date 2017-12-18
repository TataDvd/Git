using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class ConfigFirmaToLookupToField
    {
        private Dictionary<int, Dictionary<int, string>> mDictionary=new Dictionary<int, Dictionary<int, string>>();

        public void Add(int fn, int ln, string fieldn)
        {
            if (mDictionary.ContainsKey(fn))
            {
                var ld = mDictionary[fn];
                if (ld.ContainsKey(ln))
                {
                    ld[ln] = fieldn;
                }
                else
                {
                    ld.Add(ln,fieldn);
                }
            }
            else
            {
                mDictionary.Add(fn,new Dictionary<int, string>{{ln,fieldn}});
            }
        }
        public string GetField(int fn, int ln)
        {
            if (mDictionary.ContainsKey(fn))
            {
                var ld = mDictionary[fn];
                if (ld.ContainsKey(ln))
                {
                    return ld[ln];
                }
            }
            return string.Empty;
        }

        public void LoadFromDb()
        {
            var context = new TempoDataBaseContext();
            mDictionary = context.LoadConfig();
        }
    }
}
