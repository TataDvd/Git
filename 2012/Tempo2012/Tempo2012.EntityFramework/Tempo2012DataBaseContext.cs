using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;
using Tempo2012.EntityFramework.FakeData;
using Tempo2012.EntityFramework.Interface;

namespace Tempo2012.EntityFramework
{
    public class Tempo2012DataBaseContext 
    {
        public Tempo2012DataBaseContext()
        {
        }
        public IEnumerable<MapAnanaliticAccToAnaliticField> GetAllConnectorAnaliticField()
        {
            return RealDataContext.GetAllConnectorAnaliticField();
        }
        public IEnumerable<AnaliticalAccountType> GetAllAnaliticalAccountType()
        {
            return RealDataContext.GetAllAnaliticalAccountType();
        }
        public IEnumerable<AnaliticalFields> GetAllAnaliticalFields()
        {
            return RealDataContext.GetAllAnaliticalFields();
        }
        public IEnumerable<AnaliticalAccount> GetAllAnaliticalAccount()
        {
            return RealDataContext.GetAllAnaliticalAccount();
        }
        public IEnumerable<AccountsModel> GetAllAccounts(int firmaid)
        {
            return RealDataContext.GetAllAccounts(firmaid);
        }
        public IEnumerable<TableField> GetAllLookupsFields()
        {
            return RealDataContext.GetAllLookupsFields();
        }
        public IEnumerable<FirmModel> GetAllFirma()
        {
            //return FakeDataContext.GetAllFirma();
            return RealDataContext.GetAllFirma();
        }
        public IEnumerable<Country> GetAllCountry()
        {
            //return FakeDataContext.GetAllCountry();
            return RealDataContext.GetAllCountry();
        }
        public IEnumerable<City> GetAllCity()
        {
            //return FakeDataContext.GetAllSity();
            return RealDataContext.GetAllCityFromXML();
        }
        public bool Save<T>(T entry,bool isNew)
        {
            if (entry is FirmModel)
            {
                return RealDataContext.UpdateFirma(entry as FirmModel, isNew);
            }
            return true;
        }
        public void Save<T>(T list)
        {
            FakeDataContext.Save<T>(list);//fix
        }
        public FirmModel GetFirmById(int id)
        {
            return RealDataContext.GetAllFirma().FirstOrDefault(e => e.Id == id);
        }
        public FirmModel GetLastFirm()
        {
            return RealDataContext.GetAllFirma().Last();
        }
        public FirmModel GetLastFirst()
        {
            return RealDataContext.GetAllFirma().First();
        }
        public Users GetUser(string username, string password)
        {
            return FakeDataContext.GetAllUsers().FirstOrDefault(e => e.Username == username && e.PassWord == password);//to do
        }
        public IEnumerable<Conto> GetAllConto(int firmaid,long page=-1)
        {
            return RealDataContext.GetAllConto(firmaid,page);
        }
        public IEnumerable<CartotecaDebit> GetAllCartotecaDebit()
        {
            return RealDataContext.GetAllCartotecaDebit();
        }
        public IEnumerable<CartotecaCredit> GetAllCartotecaCredit()
        {
            return RealDataContext.GetAllCartotecaCredit();
        }
        public IEnumerable<LookUpSpecific> GetAllDocTypes()
        {
            return RealDataContext.GetKindDocuments();
        }
        public IEnumerable<LookUpSpecific> GetAllNationalAccounts()
        {
            return RealDataContext.GetNationalAccounts();//to do
        }
        public bool CreateTable(LookupModel model)
        {
            return RealDataContext.CreateTable(model);
        }
        public IEnumerable<LookUpMetaData> GetAllLookups(string where)
        {
            return RealDataContext.GetAllLookups(where);
        }
        public IEnumerable<LookUpMetaData> GetSystemLookups()
        {
            return RealDataContext.GetSystemLookups();
        }
        public IEnumerable<IEnumerable<string>> GetLookup(string name,int firmaId)
        {
            return RealDataContext.GetLookup(name,firmaId);
        }
        public LookupModel GetLookup(int numer)
        {
            return RealDataContext.GetLookup(numer);
        }
        public bool SaveRow(IEnumerable<string> row, LookupModel lookup,int firmaId)
        {
            return RealDataContext.SaveRow(row,lookup,firmaId);

        }
        public bool SaveRow(IEnumerable<string> row, LookupModel lookup)
        {
            return RealDataContext.SaveRow(row, lookup);

        }
        public bool UpdateRow(List<string> list, LookupModel lookupModel)
        {
            return RealDataContext.UpdateRow(list, lookupModel);
        }
        public bool DeleteRow(List<string> list, LookupModel lookupModel)
        {
            return RealDataContext.DeleteRow(list, lookupModel);
        }
        
        public bool DeleteAccount(int id)
        {
            return RealDataContext.DeleteAccount(id);
        }
        public IEnumerable<MapAnanaliticAccToAnaliticField> GetAllConnectorTypeField()
        {
            return RealDataContext.GetAllConnectorTypeField();
        }
        public bool DeleteLookUp(LookUpMetaData lookUpMetaData)
        {
            return RealDataContext.DeleteLookUp(lookUpMetaData);
        }
        public bool UpdateLookup(LookupModel lookUpMetaData)
        {
            return RealDataContext.UpdateLookup(lookUpMetaData);
        }
        public IEnumerable<LookUpMetaData> GetAllSysLookups()
        {
            return RealDataContext.GetSystemLookups();
        }
        public LookupModel GetSysLookup(int p)
        {
            return RealDataContext.GetSysLookup(p);
        }
        public bool SaveAA(AnaliticalAccount CurrentAnaliticalAccount,IEnumerable<AnaliticalFields> CurrentFieldSelected)
        {
           return RealDataContext.SaveAa(CurrentAnaliticalAccount,CurrentFieldSelected);
        }
        public void UpdateAA(AnaliticalAccount CurrentAnaliticalAccount,IEnumerable<AnaliticalFields> CurrentFieldSelected)
        {
            RealDataContext.UpdateAa(CurrentAnaliticalAccount, CurrentFieldSelected);
        }
        public bool DeleteAA(long p)
        {
            return RealDataContext.DeleteAa(p);
        }
        public IEnumerable<AnaliticalFields> GetAnaliticalFields()
        {
            return RealDataContext.GetAnaliticalFields();
        }
        public bool SaveAT(AnaliticalAccountType CurrentAllTypeAccount, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> FieldsSelected)
        {
            return RealDataContext.SaveAt(CurrentAllTypeAccount,FieldsSelected);
        }
        public bool UpdateAT(AnaliticalAccountType CurrentAllTypeAccount, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> FieldsSelected)
        {
            return RealDataContext.UpdateAt(CurrentAllTypeAccount, FieldsSelected);
        }
        public bool DeleteAt(long p)
        {
            return RealDataContext.DeleteAt(p);
        }
        public bool UpdateAccount(AccountsModel transport, bool p, System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> SelectedAnaliticalFields,out string errormessage)
        {
            return RealDataContext.UpdateAccount(transport,p,SelectedAnaliticalFields,out errormessage);
        }
        public void LoadMapToLookUps(System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> selectedAnaliticalFields,int acc,long analitic)
        {
            RealDataContext.LoadMapToLookUps(selectedAnaliticalFields,acc,analitic);
        }
        public Dictionary<string,List<string>> LoadMapToLookUps(int analiticId, long lookupid)
        {
            return RealDataContext.LoadMapToLookUps(analiticId, lookupid);
        }

        public void SaveSaldos(System.Collections.ObjectModel.ObservableCollection<SaldosModel> Fields, int accID, int lookUpId)
        {
            RealDataContext.SaveSaldos(Fields,accID,lookUpId);
        }

        public SaldosModel GetSaldo(int currentAccId, int lookupid, string rowid)
        {
            return RealDataContext.GetSaldo(currentAccId,lookupid,rowid);
        }





        public void SaveMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            RealDataContext.SaveMovement(saldoAnaliticModel);
        }

        public List<List<string>> GetAllMovements(int accid,int accnum,int firmid,out string sumalvk,out string sumalvd,out string subsumalvk,out string subsumalvd)
        {
            return RealDataContext.GetAllMovements(accid,accnum,firmid,out sumalvk,out sumalvd,out subsumalvk,out subsumalvd);
        }
        public void GetAllMovementsSalosVK(int accid, out string sumalvd, out string sumalvk, out string sumavd, out string sumavk, out string sumakd, out string sumakk)
        {
            RealDataContext.GetAllMovementsSalosVK(accid, out sumalvd, out sumalvk, out sumavd, out sumavk, out sumakd,
                                                   out sumakk);
        }
        public IEnumerable<SaldoAnaliticModel> GetCurrentMovements(int accid,int groupid)
        {
            return RealDataContext.GetCurrentMovements(accid, groupid);
        }

        public void UpdateMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            RealDataContext.UpdateMovement(saldoAnaliticModel);
        }

        public void DeleteMovement(int AccID, int selectedgroup)
        {
            RealDataContext.DeleteMovement(AccID, selectedgroup);
        }
        public IEnumerable<SaldoAnaliticModel> LoadAllAnaliticfields(int AccID)
        {
            return RealDataContext.LoadAllAnaliticfields(AccID);
        }

        public void SaveConto(Conto CurrentConto)
        {
            RealDataContext.SaveConto(CurrentConto);
        }
        public void SaveContoMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            RealDataContext.SaveContoMovement(saldoAnaliticModel);
        }


        public IEnumerable<SaldoAnaliticModel> LoadContoDetails(int contoid, int typeconto)
        {
            return RealDataContext.LoadContoDetails(contoid, typeconto);
        }

        public void UpdateConto(Conto CurrentConto)
        {
             RealDataContext.UpdateConto(CurrentConto);
        }

        public void UpdateContoMovement(SaldoAnaliticModel sa)
        {
            RealDataContext.UpdateContoMovement(sa);
        }

        public bool DeleteConto(int contoid)
        {
            return RealDataContext.DeleteConto(contoid);
        }

        public DdsDnevnikModel LoadDenevnicItem(int contoid,int typeop)
        {
            return RealDataContext.LoadDenevnicItem(contoid,typeop);
        }

        public void SaveDdsDnevnicModel(DdsDnevnikModel ddsDnevnikModel)
        {
            RealDataContext.SaveDdsDnevnicModel(ddsDnevnikModel);
        }

        public bool DeleteDdsDnevnicModel(int dnevitemid)
        {
            return RealDataContext.DeleteDdsDnevnicModel(dnevitemid);
        }

        public bool DeleteFirma(int firmid)
        {
           return RealDataContext.DeleteFirma(firmid);
        }

        public IEnumerable<DdsItemModel> GetAllDnevItems(int typednev)
        {
            return RealDataContext.LoadDnevItems(typednev);
        }
        public IEnumerable<string> GetLookupByName(string tablename, string fieldname)
        {
            return RealDataContext.GetLookupByName(tablename, fieldname);
        }


        public bool CheckMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            return RealDataContext.CheckMovement(saldoAnaliticModel);
        }

        public bool CheckLookup(int lookupId, string val)
        {
            return RealDataContext.CheckLookup(lookupId,val);
        }

        public void GetAllMovementsSaldos(int accid,int accnum , int firmid, out decimal sumalvksub, out decimal sumalvdsub)
        {
            RealDataContext.GetAllMovementsSaldos(accid, accnum, firmid, out sumalvksub, out sumalvdsub);
        }



        public void GetAccMovent(AccountsModel _acc, System.Collections.ObjectModel.ObservableCollection<string> OborotDt, System.Collections.ObjectModel.ObservableCollection<string> OborotKt)
        {
            RealDataContext.GetAccMovement(_acc, OborotDt, OborotKt);
        }

        public List<List<string>> GetDnevItem(int KindActivity,int month,int year)
        {
            return RealDataContext.GetDnevItem(KindActivity,month,year);
        }

        public void ExecuteQuery(string getQuery, DataTable items)
        {
            RealDataContext.ExecuteQuery(getQuery, items);
        }

        public Purchases GetPurchases(int month,int year)
        {
            return RealDataContext.GetPurchases(month,year);
        }

        public Sells GetSales(int month,int year)
        {
            return RealDataContext.GetSales(month,year);
        }



        public IEnumerable<Conto> GetAllConto(int firmaId, int startingIndex, int numberOfRecords)
        {
            return RealDataContext.GetAllConto(firmaId, startingIndex, numberOfRecords);
        }

        public IEnumerable<Conto> GetAllConto(int p, ISearchAcc pSearcAcc)
        {
            return RealDataContext.GetAllConto(p, pSearcAcc);
        }

        public List<List<string>> GetOborotnaVed(DateTime toDate,DateTime fromDate)
        {
            return RealDataContext.GetOborotnaVed(toDate,fromDate);
        }

        public void CopyAccFromYtoY(int firmaId, int fromYear, int toYear)
        {
            RealDataContext.CopyAccFromYtoY(firmaId, fromYear, toYear);
        }

        public IEnumerable<InvoiseControl> GetFullInvoiseContoDebit(int accId)
        {
            return RealDataContext.GetFullInvoiseContoDebit(accId);
        }
        public IEnumerable<InvoiseControl> GetFullInvoiseContoCredit(int accId)
        {
            return RealDataContext.GetFullInvoiseContoCredit(accId);
        }
        public AccSaldo GetSaldoAcc(int accId)
        {
            return RealDataContext.GetSaldoAcc(accId);
        }

        public IEnumerable<IEnumerable<string>> GetSysLookup(string p)
        {
            return RealDataContext.GetSysLookup(p);
        }

        public bool CheckLookup(List<string> list, LookupModel lookup)
        {
            return RealDataContext.CheckLookup(list,lookup);
        }

        public string GetSettings(string key)
        {
            return RealDataContext.GetSettings(key);
        }

        public void SaveNuberFormats()
        {
            RealDataContext.SaveNuberFormats();
        }

        public void SaveKurs(DateTime Data, string Code, decimal Kurs)
        {
            RealDataContext.SaveKurs(Data,Code,Kurs);
        }

        public IEnumerable<ValutaEntity> GetCurRates(string codevaluta,DateTime fromDate,DateTime toDate)
        {
            return RealDataContext.GetCurRates(codevaluta,fromDate,toDate);
        }

        public void DeleteKurs(List<ValutaEntity> itemsfordelete)
        {
            RealDataContext.DeleteKurs(itemsfordelete);
        }

        public decimal? LoadKursForaDay(DateTime Data, string vidvaluta)
        {
            return RealDataContext.LoadKursForaDay(Data, vidvaluta);
        }

        public List<List<string>> GetAllMovementsDetails(int p, int p_2, int p_3, out string sumalvk, out string sumalvd, out string sumalvdsub, out string sumalvksub)
        {
            return RealDataContext.GetAllMovementsDetail(p, p_2, p_3, out sumalvk,out sumalvd,out sumalvdsub,out sumalvksub);
        }

        public List<SaldoFactura> GetAllAnaliticSaldos(int accid, int firmid)
        {
            return RealDataContext.GetAllAnaliticSaldos(accid, firmid);
        }

        public bool DeleteAllConto(int firmaId)
        {
            return RealDataContext.DeleteAllConto(firmaId);
        }

        public List<List<string>> GetVies(int month, int year,Dictionary<string,string> declar )
        {
            return RealDataContext.GetVies(month, year,declar);
        }

        public int GetAllContoCount(int id, int year)
        {
            return RealDataContext.GetAllContoCount(id, year);
        }

        public IEnumerable<AccItemSaldo> GetInfoFactura(int acc, int type, string fn, string constr)
        {
            return RealDataContext.GetInfoFactura(acc,type, fn, constr);
        }
    }
}
