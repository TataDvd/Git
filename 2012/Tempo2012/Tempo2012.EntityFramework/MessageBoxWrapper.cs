using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YesNoApp;

namespace Tempo2012.EntityFramework
{
    public enum MessageBoxWrapperButton
    {
        YesNo, Yes, No,
        OKCancel
    }
    public enum MessageBoxWrapperResult {Yes, No}
    public class MessageBoxWrapper
    {
        public static void Show(string message)
        {
            YesNoWindow ya = new YesNoWindow(message,"Предупреждение",true);
            ya.ShowDialog();
        }

        public static void Show(string message, string warning)
        {
           YesNoWindow ya = new YesNoWindow(message,warning,true);
            ya.ShowDialog();
        }


        public static MessageBoxWrapperResult Show(string message, string warning, MessageBoxWrapperButton messageBoxWrapperButton)
        {
            YesNoWindow yn = new YesNoWindow(message, warning);
            yn.ShowDialog();
            if (yn.DialogResult.HasValue && yn.DialogResult.Value)
            {
                return MessageBoxWrapperResult.Yes;
            }
            return MessageBoxWrapperResult.No;
        }

       
    }

    public class PassDialog
    {
        public static bool Show()
        {
            PasswordDialog pd = new PasswordDialog();
            pd.ShowDialog();
            if (pd.DialogResult.HasValue && pd.DialogResult.Value)
            {
                return true;
            }
            return false;
        }
    }
}
