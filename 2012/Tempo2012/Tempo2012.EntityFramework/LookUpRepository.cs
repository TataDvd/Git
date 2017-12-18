using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.EntityFramework
{
    public class LookUpRepository
    {
        LookUpRepository()
        {
            Lookups = new Dictionary<int, LookupModel>();
            Afields = new Dictionary<int, List<SaldoAnaliticModel>>();
            ContoFields = new Dictionary<int, List<SaldoAnaliticModel>>();
        }
        private static LookUpRepository _Instance;
        private Dictionary<int, LookupModel> Lookups;
        private Dictionary<int, List<SaldoAnaliticModel>> Afields;
        public Dictionary<int, List<SaldoAnaliticModel>> ContoFields { get; set; }

        public static LookUpRepository Instance
        {
            get { return _Instance ?? (_Instance = new LookUpRepository()); }
        }
        public LookupModel LookUp(int key)
        {
            if (Lookups.ContainsKey(key))
            {
                return Lookups[key];
            }
            return null;
        }
        public List<SaldoAnaliticModel> AnaliticalFields(int key)
        {
            if (Afields.ContainsKey(key))
            {
                return Afields[key];
            }
            return null;
        }
        public void Add(int key, LookupModel lookupModel)
        {
            Lookups.Add(key, lookupModel);
        }
        public void Add(int key, List<SaldoAnaliticModel> analiticalFieldses)
        {
            Afields.Add(key, analiticalFieldses);
        }

        public void RemoveAf(int key)
        {
            Afields.Remove(key);
        }

    }
}
