using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    class AccTree
    {
        readonly List<AccTree> _children = new List<AccTree>();
        public IList<AccTree> Children
        {
            get { return _children; }
        }

        public AccountsModel CurrentAcc;
    }
    
}
