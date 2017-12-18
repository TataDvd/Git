using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.UI.WPF.ViewModels.ContoManagment
{
    public class SinkEventArgs:EventArgs
    {
        public int Id { get; set;}
        public int Direction { get; set;}
        public SinkEventArgs(int id)
        {
            Id = id;
        }
        public SinkEventArgs(int id,int direction)
        {
            Id = id;
            Direction = direction;
        }
    }
}
