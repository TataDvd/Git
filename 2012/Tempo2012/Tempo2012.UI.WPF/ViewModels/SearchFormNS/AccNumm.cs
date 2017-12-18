using System;
using Tempo2012.EntityFramework.Interface;

namespace Tempo2012.UI.WPF.ViewModels.SearchFormNS
{
    [Serializable]
    public class AccNum:IAccNum
    {
        public int Num { get; set;}
        public int SubNum { get; set;}
        public override string ToString()
        {
            if (SubNum > 0) return String.Format("{0}/{1}", Num, SubNum);
            return String.Format("{0}", Num);
        }
    }
}