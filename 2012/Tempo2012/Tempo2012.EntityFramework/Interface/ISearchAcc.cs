using System;
using System.Collections.Generic;

namespace Tempo2012.EntityFramework.Interface
{
    public interface ISearchAcc
    {
        DateTime ToDate { get; set; }
        DateTime FromDate { get; set; }
        string NumDoc { get; set; }
        string Reason { get; set;}
        string Note { get; set; }
        string CDetails { get; set;}
        string DDetails { get; set;}
        string Ob { get; set;}
        string Folder { get; set;}
        byte TypeDate { get; set;}
        int Month { get; set; }
        IList<INameValuePair> CreditItems { get; set;}
        IList<INameValuePair> DebitItems { get; set; }
        IAccNum CreditAcc { get; set;}
        IAccNum DebitAcc { get; set;}
        string Pr1 { get; set;}
        string Pr2 { get; set; }

        string PorNom { get; set; }
        string Id { get; set; }
        string UserId { get; set;}

        string DebitMask { get; set; }
        string CreditMask { get; set; }
    }
}
