using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class Users
    {
        public virtual int Id { get; set; }
        public virtual string Username{get; set; }
        public virtual string PassWord { get; set;}
        public virtual string Name { get; set;}
        public virtual string Rights { get; set;}
   }
}
