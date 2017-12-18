using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    [Serializable]
    public class City
    {
        public virtual int Id { get; set;}
        public virtual string Name { get; set;}
        public virtual string Zip { get; set;}
        public virtual int CountryId { get; set;} 
    }
}
