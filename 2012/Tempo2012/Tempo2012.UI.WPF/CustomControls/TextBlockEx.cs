using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Tempo2012.EntityFramework;

namespace Tempo2012.UI.WPF.CustomControls
{
    public class TextBlockEx:TextBlock
    {
        public TextBlockEx():base()
        {
            if (Entrence.FontSize > 0)
            {
                FontSize = Entrence.FontSize;
            }
            FontWeight = FontWeights.Bold;
        }
    }
}
