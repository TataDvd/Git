using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;

namespace Tempo2012.EntityFramework
{
    public class LoadConfigManager
    {
        private Dictionary<string,string> _paramsDictionary=new Dictionary<string, string>();

        public LoadConfigManager(string filename)
        {
            if (File.Exists(filename))
            {
                string[] files = File.ReadAllLines(filename);
                foreach (string s in files)
                {
                    if (s.StartsWith("//") || s.StartsWith("{") || s.StartsWith(" ")) continue;
                    var par = s.Split('=');
                    if (par.Length > 1)
                    {
                        if (_paramsDictionary.ContainsKey(par[0]))
                        {
                            _paramsDictionary[par[0]] = par[1];
                        }
                        else
                        {
                            _paramsDictionary.Add(par[0], par[1]);
                        }


                    }
                }
            }
        }

        public LoadConfigManager()
        {
            var conf = ConfigTempoSinglenton.GetInstance();
            foreach (var s in conf.FirmSettings)
            {


                if (_paramsDictionary.ContainsKey(string.Format("{0}-{1}-{2}", s.HoldingId, s.FirmaId, s.Key)))
                {
                    _paramsDictionary[string.Format("{0}-{1}-{2}", s.HoldingId, s.FirmaId, s.Key)] = s.Value;
                }
                else
                {
                    _paramsDictionary.Add(string.Format("{0}-{1}-{2}", s.HoldingId, s.FirmaId, s.Key), s.Value);
                }
            }

        }


        public string GetPrameter(string key)
        {
            var conf = ConfigTempoSinglenton.GetInstance();
            string ret=null;
            if (_paramsDictionary.ContainsKey(string.Format("{0}-{1}-{2}", conf.ActiveHolding, conf.ActiveFirma, key)))
            {
                ret = _paramsDictionary[string.Format("{0}-{1}-{2}", conf.ActiveHolding, conf.ActiveFirma, key)];
            }
            return ret;
        }
    }
}
