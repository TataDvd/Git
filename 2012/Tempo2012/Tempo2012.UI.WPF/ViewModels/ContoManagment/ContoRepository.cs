using System.Collections.Generic;
using Tempo2012.UI.WPF.Models;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class ContoRepository
    {
        private ContoRepository()
        {
            SaldoItems = new Dictionary<string, List<SaldoItem>>();
        }

        private static ContoRepository _Instance;
        public static ContoRepository Instance
        {
            get { return _Instance ?? (_Instance = new ContoRepository()); }
        }
        private Dictionary<string, List<SaldoItem>> SaldoItems;
        public List<SaldoItem> ContoItems(string key)
        {
            if (SaldoItems.ContainsKey(key))
            {
                return SaldoItems[key];
            }
            return null;
        }
        public void Add(string key, List<SaldoItem> lookupModel)
        {
            SaldoItems.Add(key, lookupModel);
        }
        public void Remove(string key)
        {
            SaldoItems.Remove(key);
        }
    }
}