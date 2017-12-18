using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.EntityFramework.DataContext
{
    public static class DataContext
    {
        public static IEnumerable<FirmModel> GetAllFirma()
        {
            List<FirmModel> list=new List<FirmModel>();

            return list;
        }

    }
}
