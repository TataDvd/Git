using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Tempo2012.EntityFramework.Interface;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.EntityFramework
{
    public interface IDataBaseContext
    {
        IEnumerable<MapAnanaliticAccToAnaliticField> GetAllConnectorAnaliticField();
        IEnumerable<AnaliticalAccountType> GetAllAnaliticalAccountType();
        IEnumerable<AnaliticalFields> GetAllAnaliticalFields();
        IEnumerable<AnaliticalAccount> GetAllAnaliticalAccount();
        IEnumerable<AccountsModel> GetAllAccounts(int firmaid);
        IEnumerable<TableField> GetAllLookupsFields();
        IEnumerable<FirmModel> GetAllFirma();
        IEnumerable<Country> GetAllCountry();
        IEnumerable<City> GetAllCity();
        bool Save<T>(T entry, bool isNew);
        void Save<T>(T list);
        FirmModel GetFirmById(int id);
        FirmModel GetLastFirm();
        FirmModel GetLastFirst();
        User GetUser(string username, string password);
        IEnumerable<Conto> GetAllConto(int firmaid, long page = -1);
        IEnumerable<CartotecaDebit> GetAllCartotecaDebit();
        IEnumerable<CartotecaCredit> GetAllCartotecaCredit();
        IEnumerable<LookUpSpecific> GetAllDocTypes();
        IEnumerable<LookUpSpecific> GetAllNationalAccounts();
        bool CreateTable(LookupModel model);
        IEnumerable<LookUpMetaData> GetAllLookups(string where);
        IEnumerable<LookUpMetaData> GetSystemLookups();
        IEnumerable<IEnumerable<string>> GetLookup(string name, int firmaId);
        LookupModel GetLookup(int numer);
        bool SaveRow(IEnumerable<string> row, LookupModel lookup, int firmaId);
        bool SaveRow(IEnumerable<string> row, LookupModel lookup);
        bool UpdateRow(List<string> list, LookupModel lookupModel);
        bool DeleteRow(List<string> list, LookupModel lookupModel);
        bool DeleteAccount(int id);
        IEnumerable<MapAnanaliticAccToAnaliticField> GetAllConnectorTypeField();
        List<List<string>> CheckSellsPurchases(DateTime fromDate, DateTime toDate, int kindDDS);
        List<List<string>> GetOborotnaVedDetail(DateTime fromDate, DateTime toDate,bool hideAllZero, int id = -1);
        bool DeleteLookUp(LookUpMetaData lookUpMetaData);
        bool UpdateLookup(LookupModel lookUpMetaData);
        IEnumerable<LookUpMetaData> GetAllSysLookups();
        IEnumerable<IEnumerable<string>> GetDetailsContoToAccUni(int id, int typeAccount,int kol,int val,string filter,DateTime enddate);
        void GetDetailsContoToAccUniOld(int id);
        IEnumerable<IEnumerable<string>> GetDetailsContoToAccMat(int id, int typeAccount, string filter);
        IEnumerable<IEnumerable<string>> GetDetailsContoToAccVal(int id, int typeAccount, string filter);
        LookupModel GetSysLookup(int p);
        bool SaveAA(AnaliticalAccount CurrentAnaliticalAccount, IEnumerable<AnaliticalFields> CurrentFieldSelected);
        void UpdateAA(AnaliticalAccount CurrentAnaliticalAccount, IEnumerable<AnaliticalFields> CurrentFieldSelected);
        bool DeleteAA(long p);
        IEnumerable<AnaliticalFields> GetAnaliticalFields();
        IEnumerable<QuantityModel> GetAllContoQuantity(int firmid,int accid, DateTime fromDate, DateTime toDate, int v, string codeMaterial);
        bool SaveAT(AnaliticalAccountType CurrentAllTypeAccount, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> FieldsSelected);
        IEnumerable<Conto> GetAllContoWithDdsAndNot(int id, CSearchAcc mask, string clnum,string num);
        bool UpdateAT(AnaliticalAccountType CurrentAllTypeAccount, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> FieldsSelected);
        OboronaVed GetOborotnaVedSaldo(DateTime fromDate, int id, int firmaId);
        bool DeleteAt(long p);
        bool UpdateAccount(AccountsModel transport, bool p, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> SelectedAnaliticalFields, out string errormessage);
        IEnumerable<Conto> GetContosByContragent(int id, DateTime fromDate, DateTime toDate, string code, string nom);
        IEnumerable<Conto> GetAllContoGrupedByContragent(int id, DateTime fromDate, DateTime toDate,string nom,int accid);
        void LoadMapToLookUps(System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> selectedAnaliticalFields, int acc, long analitic);
        Dictionary<string, List<string>> LoadMapToLookUps(int analiticId, long lookupid);
        void SaveSaldos(System.Collections.ObjectModel.ObservableCollection<SaldosModel> Fields, int accID, int lookUpId);
        SaldosModel GetSaldo(int currentAccId, int lookupid, string rowid);
        void SaveMovement(SaldoAnaliticModel saldoAnaliticModel);
        List<List<string>> GetAllMovements(int accid, int accnum, int firmid, out string sumalvk, out string sumalvd, out string subsumalvk, out string subsumalvd);
        void GetAllMovementsSalosVK(int accid, out string sumalvd, out string sumalvk, out string sumavd, out string sumavk, out string sumakd, out string sumakk);
        IEnumerable<SaldoAnaliticModel> GetCurrentMovements(int accid, int groupid);
        void UpdateMovement(SaldoAnaliticModel saldoAnaliticModel);
        void DeleteMovement(int AccID, int selectedgroup);
        IEnumerable<SaldoAnaliticModel> LoadAllAnaliticfields(int AccID);
        bool SaveConto(Conto CurrentConto, List<SaldoAnaliticModel> debit, List<SaldoAnaliticModel> credit);
        void SaveContoMovement(SaldoAnaliticModel saldoAnaliticModel);
        IEnumerable<SaldoAnaliticModel> LoadContoDetails(int contoid, int typeconto);
        void UpdateConto(Conto CurrentConto);
        void UpdateContoMovement(SaldoAnaliticModel sa);
        bool DeleteConto(int contoid);
        DdsDnevnikModel LoadDenevnicItem(int contoid, int typeop);
        void SaveDdsDnevnicModel(DdsDnevnikModel ddsDnevnikModel, bool isedit);
        bool DeleteDdsDnevnicModel(int dnevitemid);
        bool DeleteFirma(int firmid);
        IEnumerable<DdsItemModel> GetAllDnevItems(int typednev);
        IEnumerable<string> GetLookupByName(string tablename, string fieldname);
        bool CheckMovement(SaldoAnaliticModel saldoAnaliticModel);
        bool CheckLookup(int lookupId, string val);
        void GetAllMovementsSaldos(int accid, int accnum, int firmid, out decimal sumalvksub, out decimal sumalvdsub);
        void GetAccMovent(AccountsModel _acc, System.Collections.ObjectModel.ObservableCollection<string> OborotDt, System.Collections.ObjectModel.ObservableCollection<string> OborotKt);
        List<List<string>> GetDnevItem(int KindActivity, int month, int year);
        void ExecuteQuery(string getQuery, DataTable items);
        Purchases GetPurchases(int month, int year);
        Sells GetSales(int month, int year);
        IEnumerable<Conto> GetAllConto(int firmaId, int startingIndex, int numberOfRecords);
        IEnumerable<Conto> GetAllConto(int firmaId, ISearchAcc pSearcAcc);
        IEnumerable<Conto> GetAllContoOrfiltered(int firmaId, ISearchAcc pSearcAcc);
        IEnumerable<Conto> GetAllConto(int firmaId, ISearchAcc pSearcAcc, int startingIndex, int numberOfRecords);
        IEnumerable<Conto> GetNextConto(int firmaId, ISearchAcc pSearcAcc);
        IEnumerable<Conto> GetPrevConto(int firmaId, ISearchAcc pSearcAcc);
        List<List<string>> GetOborotnaVed(DateTime toDate, DateTime fromDate,bool hideAllZero);
        Dictionary<string, string> GetOborotnaVedTemplate(DateTime toDate, DateTime fromDate);
        void CopyAccFromYtoY(int firmaId, int fromYear, int toYear, bool et1, bool et2, bool et3,bool et4,bool et5, BackgroundWorker bw);
        IEnumerable<InvoiseControl> GetFullInvoiseContoDebit(int accId,bool withoutsuma=false,string filter=null);
        IEnumerable<InvoiseControl> GetFullInvoiseContoCredit(int accId,bool withoutsuma=false,string filter=null);
        AccSaldo GetSaldoAcc(int accId);
        IEnumerable<IEnumerable<string>> GetSysLookup(string p);
        bool CheckLookup(List<string> list, LookupModel lookup);
        string GetSettings(string key);
        void SaveNuberFormats();
        void SaveKurs(DateTime Data, string Code, decimal Kurs);
        IEnumerable<ValutaEntity> GetCurRates(string codevaluta, DateTime fromDate, DateTime toDate);
        void DeleteKurs(List<ValutaEntity> itemsfordelete);
        decimal? LoadKursForaDay(DateTime Data, string vidvaluta);
        List<List<string>> GetAllMovementsDetails(int accid, int accnum, int firmid, out string sumalvk, out string sumalvd, out string sumalvdsub, out string sumalvksub);
        List<SaldosStModel> GetAllMovementsDetailControl(int accid);
        List<SaldoAnaliticModel> GetAllMovementsDetailraz(int accid);
        List<SaldoFactura> GetAllAnaliticSaldos(int accid, int firmid,string kindValuta=null);
        bool DeleteAllConto(int firmaId);
        List<ViesRow> GetVies(int month, int year, Dictionary<string, string> declar);
        List<ViesRowG> GetViesG(int month, int year, Dictionary<string, string> declar);
        int GetAllContoCount(int id, int year, int mount);
        IEnumerable<AccItemSaldo> GetInfoFactura(int acc, int type, string fn, string constr);
        List<List<string>> GetDnevItem(int kindActivity, DateTime fromDate, DateTime toDate);
        List<Conto> GetAccMovent(int accId);
        IEnumerable<User> GetAllUsers();

        void SaveUser(User CurrentUser);

        IEnumerable<Dictionary<string, object>> GetLookupDictionary(string name, int firmaId, string filter = "",
                                                                    string range = "", string orderby = "");

        int GetFilteredLookupCount(string name, int firmaId, string filter = "");

        IEnumerable<Dictionary<string, object>> GetSysLookupDictionary(string tablename, string sqlFilter, string range, string orderby);

        int GetFilteredSysLookupCount(string tablename, string sqlFilter);
        void DeleteContos(int firmaId, DateTime fromDate, DateTime toDate);

        IEnumerable<IEnumerable<string>> GetLookup(string name, int firmaId, string filter = "", string range = "", string fields = "*");


        void UpdateRowSys(List<string> list, LookupModel lookup);

        IEnumerable<AccItemSaldo> GetInfoFactura(int p1, int p2, string contr);

        IEnumerable<System.Collections.ObjectModel.ObservableCollection<string>> GetAllSearches();

        void SaveMap(int p1, int p2, string Field);

        void DeleteMap(int fid, int lid);
        IEnumerable<IEnumerable<string>> GetDetailsContoToAcc(int id, int typ, string filter);

        string SelectMax(string tableName, string fieldName);
        string FbBatchExecution(string sql);

        IEnumerable<Conto> GetAllContoWithDds(int p, CSearchAcc cSearchAcc, int tipdnev);
        

        List<List<string>> GetDebit(DateTime fromDate, DateTime toDate, int accId, int firmId);
        List<List<string>> GetCredit(DateTime fromDate, DateTime toDate, int accId, int firmId);

        IEnumerable<Conto> GetAllContoWithoutDds(int p, CSearchAcc cSearchAcc, int tipdnev);

        void Reorder(DateTime date);

        IEnumerable<ValutaControl> GetAllContoValuta(int FirmaId, int ValId, DateTime fromDate, DateTime toDate, string vidval, int mode = 1,string codeclient="");

        IEnumerable<IEnumerable<string>> GetDetailsContoToAccEx(int p1, int p2, string filter);
        int GetAllContoCount(int id, CSearchAcc cSearchAcc);

        List<List<string>> GetUnusableClients(bool delitem = false);
        List<List<string>> GetUnusableDost(bool v = false);
        List<List<string>> GetOborotnaFullDetailed(DateTime FromDate, DateTime ToDate,bool HideAllZero);
    }
}