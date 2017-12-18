using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;
using Tempo2012.EntityFramework.FakeData;
using Tempo2012.EntityFramework.Interface;
using System.ComponentModel;

namespace Tempo2012.EntityFramework
{
    [Serializable]
    public class TempoDataBaseContext : IDataBaseContext
    {
        public TempoDataBaseContext()
        {
        }
        public virtual  IEnumerable<MapAnanaliticAccToAnaliticField> GetAllConnectorAnaliticField()
        {
            return RealDataContext.GetAllConnectorAnaliticField();
        }
        public virtual  IEnumerable<AnaliticalAccountType> GetAllAnaliticalAccountType()
        {
            return RealDataContext.GetAllAnaliticalAccountType();
        }
        public virtual  IEnumerable<AnaliticalFields> GetAllAnaliticalFields()
        {
            return RealDataContext.GetAllAnaliticalFields();
        }
        public virtual  IEnumerable<AnaliticalAccount> GetAllAnaliticalAccount()
        {
            return RealDataContext.GetAllAnaliticalAccount();
        }
        public virtual  IEnumerable<AccountsModel> GetAllAccounts(int firmaid)
        {
            return RealDataContext.GetAllAccounts(firmaid);
        }
        public virtual IEnumerable<AccountsModel> GetAllAccounts(int firmaid,int year=-1)
        {
            return RealDataContext.GetAllAccounts(firmaid,year);
        }
        public virtual  IEnumerable<TableField> GetAllLookupsFields()
        {
            return RealDataContext.GetAllLookupsFields();
        }
        public virtual  IEnumerable<FirmModel> GetAllFirma()
        {
            //return FakeDataContext.GetAllFirma();
            return RealDataContext.GetAllFirma();
        }
        public virtual  IEnumerable<Country> GetAllCountry()
        {
            //return FakeDataContext.GetAllCountry();
            return RealDataContext.GetAllCountry();
        }
        public virtual  IEnumerable<City> GetAllCity()
        {
            //return FakeDataContext.GetAllSity();
            return RealDataContext.GetAllCityFromXML();
        }
        public virtual  bool Save<T>(T entry,bool isNew)
        {
            if (entry is FirmModel)
            {
                return RealDataContext.UpdateFirma(entry as FirmModel, isNew);
            }
            return true;
        }
        public virtual  void Save<T>(T list)
        {
            FakeDataContext.Save<T>(list);//fix
        }
        public virtual  FirmModel GetFirmById(int id)
        {
            return RealDataContext.GetAllFirma().FirstOrDefault(e => e.Id == id);
        }
        public virtual  FirmModel GetLastFirm()
        {
            return RealDataContext.GetAllFirma().Last();
        }
        public virtual  FirmModel GetLastFirst()
        {
            return RealDataContext.GetAllFirma().First();
        }
        public virtual  User GetUser(string username, string password)
        {
            return RealDataContext.GetAllUsers().FirstOrDefault(e => e.UserName == username);//to do
        }
        public virtual  IEnumerable<Conto> GetAllConto(int firmaid,long page=-1)
        {
            return RealDataContext.GetAllConto(firmaid,page);
        }
        public virtual  IEnumerable<CartotecaDebit> GetAllCartotecaDebit()
        {
            return RealDataContext.GetAllCartotecaDebit();
        }
        public virtual  IEnumerable<CartotecaCredit> GetAllCartotecaCredit()
        {
            return RealDataContext.GetAllCartotecaCredit();
        }
        public virtual  IEnumerable<LookUpSpecific> GetAllDocTypes()
        {
            return RealDataContext.GetKindDocuments();
        }
        public virtual  IEnumerable<LookUpSpecific> GetAllNationalAccounts()
        {
            return RealDataContext.GetNationalAccounts();//to do
        }
        public virtual  bool CreateTable(LookupModel model)
        {
            return RealDataContext.CreateTable(model);
        }
        public virtual  IEnumerable<LookUpMetaData> GetAllLookups(string where)
        {
            return RealDataContext.GetAllLookups(where);
        }
        public virtual  IEnumerable<LookUpMetaData> GetSystemLookups()
        {
            return RealDataContext.GetSystemLookups();
        }
        public virtual  IEnumerable<IEnumerable<string>> GetLookup(string name,int firmaId)
        {
            return RealDataContext.GetLookup(name,firmaId);
        }
        public virtual  LookupModel GetLookup(int numer)
        {
            return RealDataContext.GetLookup(numer);
        }
        public virtual  bool SaveRow(IEnumerable<string> row, LookupModel lookup,int firmaId)
        {
            return RealDataContext.SaveRow(row,lookup,firmaId);

        }
        public virtual  bool SaveRow(IEnumerable<string> row, LookupModel lookup)
        {
            return RealDataContext.SaveRow(row, lookup);

        }
        public virtual  bool UpdateRow(List<string> list, LookupModel lookupModel)
        {
            return RealDataContext.UpdateRow(list, lookupModel);
        }
        public virtual  bool DeleteRow(List<string> list, LookupModel lookupModel)
        {
            return RealDataContext.DeleteRow(list, lookupModel);
        }
        
        public virtual  bool DeleteAccount(int id)
        {
            return RealDataContext.DeleteAccount(id);
        }
        public virtual  IEnumerable<MapAnanaliticAccToAnaliticField> GetAllConnectorTypeField()
        {
            return RealDataContext.GetAllConnectorTypeField();
        }
        public virtual  bool DeleteLookUp(LookUpMetaData lookUpMetaData)
        {
            return RealDataContext.DeleteLookUp(lookUpMetaData);
        }
        public virtual  bool UpdateLookup(LookupModel lookUpMetaData)
        {
            return RealDataContext.UpdateLookup(lookUpMetaData);
        }
        public virtual  IEnumerable<LookUpMetaData> GetAllSysLookups()
        {
            return RealDataContext.GetSystemLookups();
        }
        public virtual  LookupModel GetSysLookup(int p)
        {
            return RealDataContext.GetSysLookup(p);
        }
        public virtual  bool SaveAA(AnaliticalAccount CurrentAnaliticalAccount,IEnumerable<AnaliticalFields> CurrentFieldSelected)
        {
           return RealDataContext.SaveAa(CurrentAnaliticalAccount,CurrentFieldSelected);
        }
        public virtual  void UpdateAA(AnaliticalAccount CurrentAnaliticalAccount,IEnumerable<AnaliticalFields> CurrentFieldSelected)
        {
            RealDataContext.UpdateAa(CurrentAnaliticalAccount, CurrentFieldSelected);
        }
        public virtual  bool DeleteAA(long p)
        {
            return RealDataContext.DeleteAa(p);
        }
        public virtual  IEnumerable<AnaliticalFields> GetAnaliticalFields()
        {
            return RealDataContext.GetAnaliticalFields();
        }
        public virtual  bool SaveAT(AnaliticalAccountType CurrentAllTypeAccount, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> FieldsSelected)
        {
            return RealDataContext.SaveAt(CurrentAllTypeAccount,FieldsSelected);
        }
        public virtual  bool UpdateAT(AnaliticalAccountType CurrentAllTypeAccount, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> FieldsSelected)
        {
            return RealDataContext.UpdateAt(CurrentAllTypeAccount, FieldsSelected);
        }
        public virtual  bool DeleteAt(long p)
        {
            return RealDataContext.DeleteAt(p);
        }
        public virtual  bool UpdateAccount(AccountsModel transport, bool p, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> SelectedAnaliticalFields,out string errormessage)
        {
            return RealDataContext.UpdateAccount(transport,p,SelectedAnaliticalFields,out errormessage);
        }
        public virtual  void LoadMapToLookUps(System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> selectedAnaliticalFields,int acc,long analitic)
        {
            RealDataContext.LoadMapToLookUps(selectedAnaliticalFields,acc,analitic);
        }
        public virtual  Dictionary<string,List<string>> LoadMapToLookUps(int analiticId, long lookupid)
        {
            return RealDataContext.LoadMapToLookUps(analiticId, lookupid);
        }

        public virtual  void SaveSaldos(System.Collections.ObjectModel.ObservableCollection<SaldosModel> Fields, int accID, int lookUpId)
        {
            RealDataContext.SaveSaldos(Fields,accID,lookUpId);
        }

        public virtual  SaldosModel GetSaldo(int currentAccId, int lookupid, string rowid)
        {
            return RealDataContext.GetSaldo(currentAccId,lookupid,rowid);
        }





        public virtual  void SaveMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            RealDataContext.SaveMovement(saldoAnaliticModel);
        }

        public virtual  List<List<string>> GetAllMovements(int accid,int accnum,int firmid,out string sumalvk,out string sumalvd,out string subsumalvk,out string subsumalvd)
        {
            return RealDataContext.GetAllMovements(accid,accnum,firmid,out sumalvk,out sumalvd,out subsumalvk,out subsumalvd);
        }
        public virtual  void GetAllMovementsSalosVK(int accid, out string sumalvd, out string sumalvk, out string sumavd, out string sumavk, out string sumakd, out string sumakk)
        {
            RealDataContext.GetAllMovementsSalosVK(accid, out sumalvd, out sumalvk, out sumavd, out sumavk, out sumakd,
                                                   out sumakk);
        }
        public virtual  IEnumerable<SaldoAnaliticModel> GetCurrentMovements(int accid,int groupid)
        {
            return RealDataContext.GetCurrentMovements(accid, groupid);
        }

        public virtual  void UpdateMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            RealDataContext.UpdateMovement(saldoAnaliticModel);
        }

        public virtual  void DeleteMovement(int AccID, int selectedgroup)
        {
            RealDataContext.DeleteMovement(AccID, selectedgroup);
        }
        public virtual  IEnumerable<SaldoAnaliticModel> LoadAllAnaliticfields(int AccID)
        {
            return RealDataContext.LoadAllAnaliticfields(AccID);
        }

        public virtual bool SaveConto(Conto CurrentConto, List<SaldoAnaliticModel> debit, List<SaldoAnaliticModel> credit)
        {
            return RealDataContext.SaveConto(CurrentConto,debit,credit);
        }

        public virtual void SaveContoWithOutTransaction(Conto currentConto)
        {
            RealDataContext.SaveContoWithOutTransaction(currentConto);
        }
        public virtual  void SaveContoMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            RealDataContext.SaveContoMovement(saldoAnaliticModel);
        }


        public virtual  IEnumerable<SaldoAnaliticModel> LoadContoDetails(int contoid, int typeconto)
        {
            return RealDataContext.LoadContoDetails(contoid, typeconto);
        }

        public virtual  void UpdateConto(Conto CurrentConto)
        {
             RealDataContext.UpdateConto(CurrentConto);
        }

        public virtual  void UpdateContoMovement(SaldoAnaliticModel sa)
        {
            RealDataContext.UpdateContoMovement(sa);
        }

        public virtual  bool DeleteConto(int contoid)
        {
            return RealDataContext.DeleteConto(contoid);
        }

        public virtual  DdsDnevnikModel LoadDenevnicItem(int contoid,int typeop)
        {
            return RealDataContext.LoadDenevnicItem(contoid,typeop);
        }

        public virtual  void SaveDdsDnevnicModel(DdsDnevnikModel ddsDnevnikModel,bool isedit)
        {
            RealDataContext.SaveDdsDnevnicModel(ddsDnevnikModel,isedit);
        }

        public virtual  bool DeleteDdsDnevnicModel(int dnevitemid)
        {
            return RealDataContext.DeleteDdsDnevnicModel(dnevitemid);
        }

        public virtual  bool DeleteFirma(int firmid)
        {
           return RealDataContext.DeleteFirma(firmid);
        }

        public virtual  IEnumerable<DdsItemModel> GetAllDnevItems(int typednev)
        {
            return RealDataContext.LoadDnevItems(typednev);
        }
        public virtual  IEnumerable<string> GetLookupByName(string tablename, string fieldname)
        {
            return RealDataContext.GetLookupByName(tablename, fieldname);
        }


        public virtual  bool CheckMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            return RealDataContext.CheckMovement(saldoAnaliticModel);
        }

        public virtual  bool CheckLookup(int lookupId, string val)
        {
            return RealDataContext.CheckLookup(lookupId,val);
        }

        public virtual  void GetAllMovementsSaldos(int accid,int accnum , int firmid, out decimal sumalvksub, out decimal sumalvdsub)
        {
            RealDataContext.GetAllMovementsSaldos(accid, accnum, firmid, out sumalvksub, out sumalvdsub);
        }



        public virtual  void GetAccMovent(AccountsModel _acc, System.Collections.ObjectModel.ObservableCollection<string> OborotDt, System.Collections.ObjectModel.ObservableCollection<string> OborotKt)
        {
            RealDataContext.GetAccMovement(_acc, OborotDt, OborotKt);
        }

        public virtual  List<List<string>> GetDnevItem(int KindActivity,int month,int year)
        {
            return RealDataContext.GetDnevItem(KindActivity,month,year);
        }

        public virtual  void ExecuteQuery(string getQuery, DataTable items)
        {
            RealDataContext.ExecuteQuery(getQuery, items);
        }

        public virtual  Purchases GetPurchases(int month,int year)
        {
            return RealDataContext.GetPurchases(month,year);
        }

        public virtual  Sells GetSales(int month,int year)
        {
            return RealDataContext.GetSales(month,year);
        }



        public virtual  IEnumerable<Conto> GetAllConto(int firmaId, int startingIndex, int numberOfRecords)
        {
            return RealDataContext.GetAllConto(firmaId, startingIndex, numberOfRecords);
        }

        public virtual  IEnumerable<Conto> GetAllConto(int p, ISearchAcc pSearcAcc)
        {
            return RealDataContext.GetAllConto(p, pSearcAcc);
        }

        public virtual  List<List<string>> GetOborotnaVed(DateTime toDate,DateTime fromDate)
        {
            return RealDataContext.GetOborotnaVed(toDate,fromDate);
        }

        public virtual  void CopyAccFromYtoY(int firmaId, int fromYear, int toYear, bool et1, bool et2, bool et3, BackgroundWorker bw)
        {
            RealDataContext.CopyAccFromYtoY(firmaId, fromYear, toYear,et1,et2,et3,bw);
        }

        public virtual  IEnumerable<InvoiseControl> GetFullInvoiseContoDebit(int accId,bool withoutsuma=false)
        {
            return RealDataContext.GetFullInvoiseContoDebit(accId,withoutsuma);
        }
        public virtual  IEnumerable<InvoiseControl> GetFullInvoiseContoCredit(int accId,bool withoutsuma=false)
        {
            return RealDataContext.GetFullInvoiseContoCredit(accId,withoutsuma);
        }
        public virtual  AccSaldo GetSaldoAcc(int accId)
        {
            return RealDataContext.GetSaldoAcc(accId);
        }

        public virtual  IEnumerable<IEnumerable<string>> GetSysLookup(string p)
        {
            return RealDataContext.GetSysLookup(p);
        }

        public virtual  bool CheckLookup(List<string> list, LookupModel lookup)
        {
            return RealDataContext.CheckLookup(list,lookup);
        }

        public virtual  string GetSettings(string key)
        {
            return RealDataContext.GetSettings(key);
        }

        public virtual  void SaveNuberFormats()
        {
            RealDataContext.SaveNuberFormats();
        }

        public virtual  void SaveKurs(DateTime Data, string Code, decimal Kurs)
        {
            RealDataContext.SaveKurs(Data,Code,Kurs);
        }

        public virtual  IEnumerable<ValutaEntity> GetCurRates(string codevaluta,DateTime fromDate,DateTime toDate)
        {
            return RealDataContext.GetCurRates(codevaluta,fromDate,toDate);
        }

        public virtual  void DeleteKurs(List<ValutaEntity> itemsfordelete)
        {
            RealDataContext.DeleteKurs(itemsfordelete);
        }

        public virtual  decimal? LoadKursForaDay(DateTime Data, string vidvaluta)
        {
            return RealDataContext.LoadKursForaDay(Data, vidvaluta);
        }

        public virtual  List<List<string>> GetAllMovementsDetails(int p, int p_2, int p_3, out string sumalvk, out string sumalvd, out string sumalvdsub, out string sumalvksub)
        {
            return RealDataContext.GetAllMovementsDetail(p, p_2, p_3, out sumalvk,out sumalvd,out sumalvdsub,out sumalvksub);
        }

        public virtual  List<SaldoFactura> GetAllAnaliticSaldos(int accid, int firmid)
        {
            return RealDataContext.GetAllAnaliticSaldos(accid, firmid);
        }

        public virtual  bool DeleteAllConto(int firmaId)
        {
            return RealDataContext.DeleteAllConto(firmaId);
        }

        public virtual List<ViesRow> GetVies(int month, int year, Dictionary<string, string> declar)
        {
            return RealDataContext.GetVies(month, year,declar);
        }

        public virtual  int GetAllContoCount(int id, int year,int mount)
        {
            return RealDataContext.GetAllContoCount(id, year,mount);
        }

        public virtual  IEnumerable<AccItemSaldo> GetInfoFactura(int acc, int type, string fn, string constr)
        {
            return RealDataContext.GetInfoFactura(acc,type, fn, constr);
        }


        public virtual List<List<string>> GetDnevItem(int kindActivity, DateTime fromDate, DateTime toDate)
        {
            return RealDataContext.GetDnevItem(kindActivity, fromDate, toDate);
        }


        public virtual List<Conto> GetAccMovent(int accId)
        {
            return RealDataContext.GetAccMovent(accId);
        }


        public virtual IEnumerable<User> GetAllUsers()
        {
            return RealDataContext.GetAllUsers();
        }


        public virtual void SaveUser(User CurrentUser)
        {
            RealDataContext.SaveUser(CurrentUser);
        }


        public virtual IEnumerable<Dictionary<string, object>> GetLookupDictionary(string name, int firmaId, string filter = "", string range = "",string orderby="")
        {
            return RealDataContext.GetLookupDictionary(name, firmaId, filter, range, orderby);
        }


        public virtual int GetFilteredLookupCount(string name, int firmaId, string filter = "")
        {
            return RealDataContext.GetFilteredLookupCount(name, firmaId, filter);
        }




        public IEnumerable<Dictionary<string, object>> GetSysLookupDictionary(string tablename, string sqlFilter, string range,string orderby)
        {
            return RealDataContext.GetSysLookupDictionary(tablename, sqlFilter, range, orderby);
        }

        public int GetFilteredSysLookupCount(string tablename, string sqlFilter)
        {
            return RealDataContext.GetFilteredSysLookupCount(tablename, sqlFilter);
        }


        public void DeleteContos(int firmaId, DateTime fromDate, DateTime toDate)
        {
            RealDataContext.DeleteContos(firmaId, fromDate, toDate);
        }


        public IEnumerable<IEnumerable<string>> GetLookup(string name, int firmaId,string filter = "", string range = "", string fields = "*")
        {
            return RealDataContext.GetLookup(name, firmaId, filter, range, fields);
        }


        public void UpdateRowSys(List<string> list, LookupModel lookup)
        {
            RealDataContext.UpdateRowSys(list, lookup);
        }


        public IEnumerable<AccItemSaldo> GetInfoFactura(int acc, int type, string contr)
        {
            return RealDataContext.GetInfoFactura(acc, type, contr);
        }

        public Dictionary<int, Dictionary<int, string>> LoadConfig()
        {
            return RealDataContext.LoadConfig();
        }


        public IEnumerable<System.Collections.ObjectModel.ObservableCollection<string>> GetAllSearches()
        {
            return RealDataContext.GetAllSearches();
        }


        public void SaveMap(int fid, int lid, string field)
        {
            RealDataContext.SaveMap(fid, lid, field);
        }


        public void DeleteMap(int fid, int lid)
        {
             RealDataContext.DeleteMap(fid,lid);
        }


        public IEnumerable<IEnumerable<string>> GetDetailsContoToAcc(int id, int typ, string filter)
        {
            return RealDataContext.GetDetailsContoToAcc(id,typ,filter);
        }


        public string SelectMax(string tableName, string fieldName)
        {
            return RealDataContext.SelectMax(tableName, fieldName);
        }


        public string FbBatchExecution(string sql)
        {
            return RealDataContext.FbBatchExecution(sql);
        }


        public IEnumerable<Conto> GetAllContoWithDds(int p, CSearchAcc cSearchAcc,int tipdnev)
        {
            return RealDataContext.GetAllContoWithDds(p, cSearchAcc,tipdnev);
        }


        public List<List<string>> GetDebit(DateTime fromDate, DateTime toDate, int accId, int firmId)
        {
            return RealDataContext.GetDebit(fromDate, toDate, accId, firmId);
        }


        public List<List<string>> GetCredit(DateTime fromDate, DateTime toDate, int accId, int firmId)
        {
            return RealDataContext.GetCredit(fromDate, toDate, accId, firmId);
        }


        public IEnumerable<Conto> GetAllContoWithoutDds(int p, CSearchAcc cSearchAcc, int tipdnev)
        {
            return RealDataContext.GetAllContoWithoutDds(p, cSearchAcc, tipdnev);
        }


        public void Reorder()
        {
            RealDataContext.Reorder();
        }


        public IEnumerable<ValutaControl> GetAllContoValuta(int FirmaId, int ValId, DateTime fromDate, DateTime toDate, string vidval,int mode=1,string codeclient="")
        {
            return RealDataContext.GetAllContoValuta(FirmaId, ValId, fromDate,toDate,vidval,mode,codeclient);
        }


        public IEnumerable<IEnumerable<string>> GetDetailsContoToAccEx(int p1, int p2, string filter)
        {
            return RealDataContext.GetDetailsContoToAccEx(p1, p2, filter);
        }

        public IEnumerable<Conto> GetAllContoWithDdsAndNot(int id, CSearchAcc mask,string cl,string num)
        {
            return RealDataContext.GetAllContoWithDdsAndNot(id,mask,cl,num);
        }

        public IEnumerable<Conto> GetAllConto(int firmaId, ISearchAcc pSearcAccint, int startingIndex, int numberOfRecords)
        {
           return RealDataContext.GetAllConto(firmaId, pSearcAccint, startingIndex, numberOfRecords);
        }

        public int GetAllContoCount(int id, CSearchAcc cSearchAcc)
        {
            return RealDataContext.GetAllContoCount(id,cSearchAcc);
        }

        public IEnumerable<Conto> GetNextConto(int firmaId, ISearchAcc pSearcAcc)
        {
            return RealDataContext.GetNextConto(firmaId, pSearcAcc);
        }

        public IEnumerable<Conto> GetPrevConto(int firmaId, ISearchAcc pSearcAcc)
        {
            return RealDataContext.GetPrevConto(firmaId, pSearcAcc);
        }


        public List<List<string>> GetUnusableClients(bool delitem=false)
        {
            return RealDataContext.GetUnusableClients(delitem);
        }


        public Dictionary<string, string> GetOborotnaVedTemplate(DateTime toDate, DateTime fromDate)
        {
            return RealDataContext.GetOborotnaVedTemplate(toDate, fromDate);
        }

        public List<List<string>> GetOborotnaVedDetail(DateTime fromDate, DateTime toDate,int id=-1)
        {
            return RealDataContext.GetOborotnaVedDetailed(fromDate,toDate,id);
        }

        public List<List<string>> GetUnusableDost(bool v=false)
        {
            return RealDataContext.GetUnusableDost(v);
        }

        public List<List<string>> GetOborotnaFullDetailed(DateTime FromDate, DateTime ToDate)
        {
            return RealDataContext.GetOborotnaFullDetailed(FromDate, ToDate);
        }

        public OboronaVed GetOborotnaVedSaldo(DateTime fromDate, int id, int firmaId)
        {
            return RealDataContext.GetOborotnaVedSaldo(fromDate, id);
        }

        public IEnumerable<QuantityModel> GetAllContoQuantity(int firmid,int accid,DateTime fromDate, DateTime toDate, int v, string codeMaterial)
        {
             return RealDataContext.GetAllContoQuantity(firmid,accid,fromDate,toDate,v, codeMaterial);
        }

        public IEnumerable<IEnumerable<string>> GetDetailsContoToAccUni(int id, int typeAccount, string filter)
        {
            return RealDataContext.GetDetailsContoToAccUni(id,typeAccount,filter);
        }

        public IEnumerable<IEnumerable<string>> GetDetailsContoToAccMat(int id, int typeAccount, string filter)
        {
            return RealDataContext.GetDetailsContoToAccMat(id, typeAccount, filter);
        }

        public IEnumerable<Conto> GetAllContoOrfiltered(int firmaId, ISearchAcc pSearcAcc)
        {
            return RealDataContext.GetAllContoOrfiltered(firmaId,pSearcAcc);
        }

        public IEnumerable<Conto> GetContosByContragent(int id, DateTime fromDate, DateTime toDate, string code, string nom)
        {
            return RealDataContext.GetContosByContragent(id, fromDate, toDate, code,nom);
        }
    }
}
