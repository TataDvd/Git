using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsInput;
using dragonz.actb.core;
using dragonz.actb.provider;
using Tempo2012.EntityFramework;

namespace Tempo2012.UI.WPF.CustomControls
{
    public class TextBoxEx:TextBox
    {
        protected string OldText;
        protected bool Modify;
        protected bool Creation = false;

        public TextBoxEx()
        {
            if (Entrence.FontSize > 0)
            {
                FontSize = Entrence.FontSize;
            }
            
        }

       

        protected override void OnGotFocus(System.Windows.RoutedEventArgs e)
        {
            OldText = Text;
            base.OnGotFocus(e);
            SelectAll();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (Creation) Modify = true;
            Creation = true;
            base.OnTextChanged(e);
        }

        protected override void  OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key==Key.Escape)
            {
                if (Modify)
                {
                    Text = OldText;
                    e.Handled = true;
                    Modify = false;
                }
                else
                {
                    e.Handled = true;
                    InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
                    InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
                }
            }
            base.OnKeyDown(e);
        }

        

      
    }
}
