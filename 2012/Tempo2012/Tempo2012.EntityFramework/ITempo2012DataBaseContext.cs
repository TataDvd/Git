
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.EntityFramework
{
    interface ITempo2012DataBaseContext
    {
        bool CreateTable(LookupModel model);
        bool DeleteAA(long p);
        bool DeleteAccount(int id);
        bool DeleteAt(long p);
        bool DeleteLookUp(LookUpMetaData lookUpMetaData);
        void DeleteMovement(int AccID, int selectedgroup);
        bool DeleteRow(System.Collections.Generic.List<string> list, LookupModel lookupModel);
        System.Collections.Generic.IEnumerable<AccountsModel> GetAllAccounts(int firmaid);
        System.Collections.Generic.IEnumerable<AnaliticalAccount> GetAllAnaliticalAccount();
        System.Collections.Generic.IEnumerable<AnaliticalAccountType> GetAllAnaliticalAccountType();
        System.Collections.Generic.IEnumerable<AnaliticalFields> GetAllAnaliticalFields();
        System.Collections.Generic.IEnumerable<CartotecaCredit> GetAllCartotecaCredit();
        System.Collections.Generic.IEnumerable<CartotecaDebit> GetAllCartotecaDebit();
        System.Collections.Generic.IEnumerable<City> GetAllCity();
        System.Collections.Generic.IEnumerable<MapAnanaliticAccToAnaliticField> GetAllConnectorAnaliticField();
        System.Collections.Generic.IEnumerable<MapAnanaliticAccToAnaliticField> GetAllConnectorTypeField();
        System.Collections.Generic.IEnumerable<Conto> GetAllConto(int firmaid);
        System.Collections.Generic.IEnumerable<Country> GetAllCountry();
        System.Collections.Generic.IEnumerable<LookUpSpecific> GetAllDocTypes();
        System.Collections.Generic.IEnumerable<FirmModel> GetAllFirma();
        System.Collections.Generic.IEnumerable<LookUpMetaData> GetAllLookups(string where);
        System.Collections.Generic.IEnumerable<TableField> GetAllLookupsFields();
        System.Collections.Generic.List<System.Collections.Generic.List<string>> GetAllMovements(int accid, out string sumalvk, out string sumalvd);
        System.Collections.Generic.IEnumerable<LookUpSpecific> GetAllNationalAccounts();
        System.Collections.Generic.IEnumerable<LookUpMetaData> GetAllSysLookups();
        System.Collections.Generic.IEnumerable<AnaliticalFields> GetAnaliticalFields();
        System.Collections.Generic.IEnumerable<SaldoAnaliticModel> GetCurrentMovements(int accid, int groupid);
        FirmModel GetFirmById(int id);
        FirmModel GetLastFirm();
        FirmModel GetLastFirst();
        LookupModel GetLookup(int numer);
        System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<string>> GetLookup(string name);
        SaldosModel GetSaldo(int currentAccId, int lookupid, string rowid);
        LookupModel GetSysLookup(int p);
        System.Collections.Generic.IEnumerable<LookUpMetaData> GetSystemLookups();
        User GetUser(string username, string password);
        System.Collections.Generic.IEnumerable<SaldoAnaliticModel> LoadAllAnaliticfields(int AccID);
        void LoadMapToLookUps(System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> selectedAnaliticalFields, int acc, long analitic);
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> LoadMapToLookUps(int analiticId, long lookupid);
        bool Save<T>(T entry, bool isNew);
        void Save<T>(T list);
        bool SaveAA(AnaliticalAccount CurrentAnaliticalAccount, System.Collections.Generic.IEnumerable<AnaliticalFields> CurrentFieldSelected);
        bool SaveAT(AnaliticalAccountType CurrentAllTypeAccount, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> FieldsSelected);
        void SaveConto(Conto CurrentConto);
        void SaveMovement(SaldoAnaliticModel saldoAnaliticModel);
        bool SaveRow(System.Collections.Generic.IEnumerable<string> row, LookupModel lookup);
        void SaveSaldos(System.Collections.ObjectModel.ObservableCollection<SaldosModel> Fields, int accID, int lookUpId);
        void UpdateAA(AnaliticalAccount CurrentAnaliticalAccount, System.Collections.Generic.IEnumerable<AnaliticalFields> CurrentFieldSelected);
        bool UpdateAccount(AccountsModel transport, bool p, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> SelectedAnaliticalFields,out string errormessage);
        bool UpdateAT(AnaliticalAccountType CurrentAllTypeAccount, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> FieldsSelected);
        bool UpdateLookup(LookUpMetaData lookUpMetaData);
        void UpdateMovement(SaldoAnaliticModel saldoAnaliticModel);
        bool UpdateRow(System.Collections.Generic.List<string> list, LookupModel lookupModel);
    }
}
