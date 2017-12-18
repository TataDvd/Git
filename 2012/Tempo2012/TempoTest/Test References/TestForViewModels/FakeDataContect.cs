using System.Collections.Generic;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;

namespace TempoTest.Test_References.TestForViewModels
{
    public class FakeDataContect : TempoDataBaseContext
    {
        public override IEnumerable<string> GetLookupByName(string tablename, string fieldname)
        {
            return new List<string>{"Edno","Dve"};
        }

        public override IEnumerable<AccountsModel> GetAllAccounts(int firmaid)
        {
            return new List<AccountsModel>{new AccountsModel{AnaliticalNum = 1,FirmaId = firmaid,LevelAccount = 1,Id=1}};
        }

        public override IEnumerable<DdsItemModel> GetAllDnevItems(int typednev)
        {
            return new List<DdsItemModel>{new DdsItemModel{Code = "1",Dds = 20,DdsPercent = 20,DdsSuma = 453}};
        }
    }
}