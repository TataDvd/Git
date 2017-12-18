using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework
{
    public interface IField
    {
        int Id { get; set;}
        string Name { get; set;}
        string Description { get; set;}
    }
}
