using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class ContoComparer : IEqualityComparer<Conto>
    {
        public bool Equals(Conto x, Conto y)
        {
            return x.Id==y.Id;
        }

        public int GetHashCode(Conto product)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(product, null)) return 0;
            //Calculate the hash code for the product.
            return 1;
        }
    }
}
