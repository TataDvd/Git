using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework
{
    [Serializable]
    public class HoldingModel
    {
        public int Nom { get; set;}
        public string Name { get; set; }
        public string IpServer { get; set;}
        public string Template { get; set;}
        public string ConectionString { get; set; }
    }
}
