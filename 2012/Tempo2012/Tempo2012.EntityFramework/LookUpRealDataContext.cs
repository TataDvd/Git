using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;
using System.Configuration;
using System.Data;
using System.IO;
using Tempo2012.EntityFramework.Interface;

namespace Tempo2012.EntityFramework
{
    public static partial class RealDataContext
    {
        public static bool CreateTable(LookupModel lookupmodel)
        {
            bool result = true;

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open(); 
                dbman.BeginTransaction();
                dbman.CreateParameters(3);
                dbman.AddOutputParameters(2, "@Id", lookupmodel.LookUpMetaData.Id);
                dbman.AddParameters(0, "@Name", lookupmodel.LookUpMetaData.Name);
                dbman.AddParameters(1, "@Description", lookupmodel.LookUpMetaData.Description);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, Commands.InsertLookup);
                lookupmodel.LookUpMetaData.Id = (int)dbman. Parameters[2].Value;
                int sortorder = 0;
                foreach (var field in lookupmodel.Fields)
                {
                    string commands =
                        string.Format(
                            "INSERT INTO \"lookupsdetails\" (\"IdLookUp\", \"IdLookField\",\"SORTORDER\",ISUNIQUE,ISREQUARED,TN) VALUES ({0},{1},{2},{3},{4},'{5}')",
                            lookupmodel.LookUpMetaData.Id, field.Id, sortorder,field.IsUnique?1:0,field.IsRequared?1:0,field.Tn??"");
                    dbman.ExecuteNonQuery(CommandType.Text, commands);
                    sortorder++;
                }
                string test = new TableCreator("nom_" + lookupmodel.LookUpMetaData.Id, lookupmodel.Fields).ToString();
                dbman.ExecuteNonQuery(CommandType.Text, test);
                StringBuilder triger = new StringBuilder();
                string tablename = "nom_" + lookupmodel.LookUpMetaData.Id;
                string trigername = "trigernom_" + lookupmodel.LookUpMetaData.Id;
                string generatorname = "generatornom_" + lookupmodel.LookUpMetaData.Id;
                triger.AppendFormat("CREATE GENERATOR {0}", generatorname);
                dbman.ExecuteNonQuery(CommandType.Text, triger.ToString());
                triger.Clear();
                triger.AppendFormat(
                    "CREATE TRIGGER \"{1}\" FOR \"{2}\" ACTIVE BEFORE INSERT AS BEGIN  new.\"Id\" = gen_id({0}, 1);END",
                    generatorname, trigername, tablename);
                //triger.Append("END $$ SET TERM ; $$ SET TERM $$ ;");
                dbman.ExecuteNonQuery(CommandType.Text, triger.ToString());
                //string maptablename = string.Format("CREATE TABLE \"{0}tofirm\"(\"FirmId\" Integer NOT NULL,\"NomID\" Integer NOT NULL)", tablename);
                //dbman. ExecuteNonQuery(CommandType.Text, maptablename);
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "CreateTable");
                dbman. RollBackTransaction();
                result = false;
            }

            finally
            {
                dbman. Dispose();
            }
            return result;
        }
        public static IEnumerable<City> GetAllCityFromXML()
        {
            List<City> list = new List<City>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select * from \"cities\"");
                while (dbman.DataReader.Read())
                {
                    list.Add(new City
                    {
                        Id = int.Parse(dbman. DataReader["Id"].ToString()),
                        Name = dbman.DataReader["Name"].ToString(),
                        CountryId = int.Parse(dbman. DataReader["CountryId"].ToString()),
                        Zip = dbman. DataReader["Zip"].ToString()
                    });

                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllCityFromXML");
            }

            finally
            {
                dbman. Dispose();
            }
            return list;
        }
        public static IEnumerable<Country> GetAllCountry()
        {
            List<Country> allcountry = new List<Country>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select * from \"countries\"");
                while (dbman. DataReader.Read())
                {
                    allcountry.Add(new Country
                    {
                        Id = int.Parse(dbman. DataReader["Id"].ToString()),
                        Name = dbman. DataReader["Name"].ToString(),

                    });

                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllCountry");
            }

            finally
            {
                dbman. Dispose();
            }

            return allcountry;
        }
        public static IEnumerable<TableField> GetAllLookupsFields()
        {
            var result = new List<TableField>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman. ExecuteReader(CommandType.Text, "select * from \"lookupsfield\"");
                while (dbman. DataReader.Read())
                {
                    int test;
                    result.Add(new TableField
                    {
                        Id = (int)dbman. DataReader["Id"],
                        Name = dbman. DataReader["Name"].ToString(),
                        NameEng = dbman. DataReader["NameEng"].ToString(),
                        DbField = dbman. DataReader["DBField"].ToString(),
                        IsRequared = int.Parse(dbman. DataReader["IsNull"].ToString()) == 1 ? true : false,
                        Length = int.Parse(dbman. DataReader["Length"].ToString()),
                        RFIELDKEY = dbman. DataReader["RFIELDKEY"].ToString(),
                        RFIELDNAME =  dbman. DataReader["RFIELDNAME"].ToString(),
                        RCODELOOKUP = int.TryParse(dbman. DataReader["RCODELOOKUP"].ToString(),out test)? test : 0,
                        GROUP = int.TryParse(dbman. DataReader["GROUP"].ToString(),out test)? test : 0
                    });
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllLookupsFields");
            }

            finally
            {
                dbman. Dispose();
            }
            return result;
        }
        public static IEnumerable<LookUpMetaData> GetAllLookups(string where)
        {
            List<LookUpMetaData> list = new List<LookUpMetaData>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select * from \"lookups\""+where);
                while (dbman. DataReader.Read())
                {
                    list.Add(new LookUpMetaData
                    {
                        Id = (int)dbman. DataReader["Id"],
                        Name = dbman. DataReader["Name"].ToString(),
                        Description = dbman. DataReader["Description"].ToString(),
                        Tablename = dbman. DataReader["Tablename"].ToString(),
                        NameEng = dbman. DataReader["NAMEENG"].ToString(),
                    });
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllLookups(string where)");
            }

            finally
            {
                dbman. Dispose();
            }
            return list;
        }

        public static IEnumerable<IEnumerable<string>> GetLookup(string name, int firmaId, string filter = "",
                                                                 string range = "", string fields = "*")
        {
            List<List<string>> result = new List<List<string>>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string s = string.Format("select {0} {1} from \"{2}\" where FIRMAID={3} {4}", range, fields, name,
                    firmaId, filter);
                dbman.ExecuteReader(CommandType.Text,s);
                while (dbman. DataReader.Read())
                {
                    List<string> row = new List<string>();
                    for (var i = 0; i < dbman. DataReader.FieldCount; i++)
                    {
                        row.Add(dbman. DataReader[i].ToString());
                    }
                    result.Add(row);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message,"GetLookup(string name, int firmaId, string filter = \"\",string range = \"\", string fields = \"*\")");
            }

            finally
            {
                dbman. Dispose();
            }
            return result;
        }

        public static IEnumerable<Dictionary<string,object>> GetLookupDictionary(string name, int firmaId, string filter = "", string range = "",string orderby="")
        {
            List<Dictionary<string,object>> result = new List<Dictionary<string,object>>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string s = string.Format("select {0} * from \"{1}\" where FIRMAID={2} {3} {4}", range, name, firmaId,
                    filter, orderby);
                dbman.ExecuteReader(CommandType.Text,s);
                while (dbman. DataReader.Read())
                {
                    Dictionary<string,object> row =new Dictionary<string, object>();
                    for (var i = 0; i < dbman. DataReader.FieldCount; i++)
                    {
                        row.Add(dbman. DataReader.GetName(i), dbman. DataReader[i]);
                    }
                    result.Add(row);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetLookupDictionary");
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }
        public static IEnumerable<Dictionary<string, object>> GetSysLookupDictionary(string name, string filter = "", string range = "",string orderby="")
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
               string filt = !string.IsNullOrWhiteSpace(filter) ? string.Format(" where {0}",filter.StartsWith("AND")?filter.Remove(0,3):filter): "";
                dbman. ExecuteReader(CommandType.Text, string.Format("select {0} * from \"{1}\"{2}", range, name, filt));
                while (dbman.DataReader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    for (var i = 0; i < dbman. DataReader.FieldCount; i++)
                    {
                        row.Add(dbman.DataReader.GetName(i), dbman.DataReader[i]);
                    }
                    result.Add(row);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message,"static IEnumerable<Dictionary<string, object>> GetSysLookupDictionary(string name, string filter = \"\", string range = \"\",string orderby=\"\")");
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }
        internal static int GetFilteredSysLookupCount(string name, string filter)
        {
            int rez = 0;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string filt = !string.IsNullOrWhiteSpace(filter) ? string.Format(" where {0}", filter) : "";
                rez = (int)dbman. ExecuteScalar(CommandType.Text, string.Format("select count(*) from \"{0}\"{1}", name,filt));

            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "static int GetFilteredSysLookupCount(string name, string filter)");
            }
            return rez;
        }
        internal static int GetFilteredLookupCount(string name, int firmaId, string filter)
        {
            int rez = 0;
            try
            {
                var dbman = new DBManager(DataProvider.Firebird);
                dbman.ConnectionString = Entrence.ConnectionString;
                dbman.Open();
                rez =(int)dbman.ExecuteScalar(CommandType.Text, string.Format("select count(*) from \"{0}\" where FIRMAID={1} {2}", name ,firmaId, filter));
                
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "static int GetFilteredLookupCount(string name, int firmaId, string filter)");
            }
            return rez;
        }
        public static IEnumerable<IEnumerable<string>> GetSysLookup(string name)
        {
            List<List<string>> result = new List<List<string>>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, string.Format("select * from \"" + name + "\""));
                while (dbman.DataReader.Read())
                {
                    List<string> row = new List<string>();
                    for (var i = 0; i < dbman. DataReader.FieldCount; i++)
                    {
                        row.Add(dbman. DataReader[i].ToString());
                    }
                    result.Add(row);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<IEnumerable<string>> GetSysLookup(string name)");
            }

            finally
            {
                dbman. Dispose();
            }
            return result;
        }
        public static IEnumerable<string> GetLookupByName(string tablename, string fieldname)
        {
            List<string> result = new List<string>();
            LookUpMetaData lookUpMetaData = GetAllLookups(string.Format(" where NAMEENG='{0}'", tablename)).FirstOrDefault();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            if (lookUpMetaData != null)
            {
                LookupModel lookupModel = new LookupModel(GetLookUpFields(lookUpMetaData.Id).ToList(), lookUpMetaData);
                try
                {
                    dbman.Open();
                    dbman.ExecuteReader(CommandType.Text, string.Format("select \"{0}\" from \"{1}\" where FIRMAID={2}", fieldname, lookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
                    while (dbman. DataReader.Read())
                    {
                        result.Add(dbman.DataReader[fieldname].ToString());
                    }
                }

                catch (Exception ex)
                {
                    Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<string> GetLookupByName(string tablename, string fieldname)");
                }

                finally
                {
                    dbman.Dispose();
                }
            }
            return result;
        }

        public static bool UpdateRow(IEnumerable<string> row, LookupModel lookup)
        {
            List<List<string>> result = new List<List<string>>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman. CreateParameters(lookup.Fields.Count);
                if (lookup.LookUpMetaData.Tablename == "DDSDNEVSELLSFIELDS" || lookup.LookUpMetaData.Tablename == "DDSDNEVFIELDS")
                {
                    lookup.Fields[0].DbField = "ID";
                    lookup.Fields[0].NameEng = "ID";
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("UPDATE \"{0}\" SET ", lookup.LookUpMetaData.Tablename);
                foreach (var field in lookup.Fields.Take(lookup.Fields.Count-1))
                {
                    if (field.NameEng != "ID" || field.NameEng !="Id")
                    {
                        sb.AppendFormat("\"{0}\"=@{1},", field.NameEng, field.NameEng.Replace(' ', '_'));
                    }
                }
                sb.Remove(sb.Length - 1, 1);
                if (lookup.LookUpMetaData.Tablename == "DDSDNEVSELLSFIELDS" || lookup.LookUpMetaData.Tablename == "DDSDNEVFIELDS")
                {
                    sb.AppendFormat(" where \"ID\"={0}", row.ElementAt(0));
                }
                else
                {
                    sb.AppendFormat(" where \"Id\"={0}", row.ElementAt(0));
                }

                var i = 0;
                foreach (var field in lookup.Fields.Take(lookup.Fields.Count - 1))
                {

                    if (field.DbField.ToLower() == "integer")
                    {
                        dbman. AddParameters(i, "@" + field.NameEng.Replace(' ', '_'), int.Parse(row.ElementAt(i)));
                    }
                    else
                    {
                        if (field.DbField.ToUpper().Contains("DECIMAL"))
                        {
                            dbman. AddParameters(i, "@" + field.NameEng.Replace(' ', '_'),
                                                    decimal.Parse(row.ElementAt(i)));
                        }
                        else
                        {
                            if (field.DbField.ToUpper() == ("CHAR(38)"))
                            {
                                if (field.Name != "Id" || field.Name != "ID")
                                {
                                    dbman. AddParameters(i, "@" + field.NameEng.Replace(' ', '_'),
                                                            Guid.NewGuid().ToString());
                                }
                            }
                            else
                            {
                                if (field.DbField.ToUpper() == ("DATE"))
                                {
                                    dbman. AddParameters(i, "@" + field.NameEng.Replace(' ', '_'),
                                                    DateTime.Parse(row.ElementAt(i)));
                                }
                                else
                                {
                                    dbman. AddParameters(i, "@" + field.NameEng.Replace(' ', '_'), row.ElementAt(i));
                                }
                            }
                        }

                    }
                    i++;
                }

                dbman. ExecuteNonQuery(CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static bool UpdateRow(IEnumerable<string> row, LookupModel lookup)");
            }

            finally
            {
                dbman. Dispose();
            }
            return true;
        }
        public static bool SaveRow(IEnumerable<string> roww, LookupModel lookup)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                List<string> row = new List<string>(roww);
                if (roww != null)
                {
                    if (lookup.Fields[1].DbField == "integer")
                    {
                        string cmd = string.Format("Select * from \"{0}\" where \"{1}\"={2}}",
                                                   lookup.LookUpMetaData.Tablename, lookup.Fields[1].NameEng, row[1]);

                        var reader = dbman. ExecuteReader(CommandType.Text, cmd);
                        if (reader.Read())
                        {
                            return false;
                        }
                    }
                }
                dbman. CreateParameters(lookup.Fields.Count - 1);
                row.Insert(0,"1");
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("INSERT INTO \"{0}\" (", lookup.LookUpMetaData.Tablename);
                foreach (var field in lookup.Fields)
                {
                    if (field.NameEng != "Id") sb.AppendFormat("\"{0}\",", field.NameEng);
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendFormat(") Values (", lookup.LookUpMetaData.Tablename);
                var i = 0;
                var k = 0;
                foreach (var field in lookup.Fields)
                {

                    if (field != null)
                    {
                        if (field.NameEng == "Id")
                        {
                            i++;
                            continue;
                        }
                        sb.AppendFormat("@{0},", field.NameEng.Replace(' ', '_'));
                        if (field.DbField.ToLower() == "integer")
                        {
                            dbman. AddParameters(k, "@" + field.NameEng.Replace(' ', '_'),
                                                    int.Parse(row.ElementAt(i)));
                        }
                        else
                        {
                            if (field.DbField.ToLower().Contains("date"))
                            {
                                dbman. AddParameters(k, "@" + field.NameEng.Replace(' ', '_'),
                                                        DateTime.Parse(row.ElementAt(i)));
                            }
                            else
                            {
                                if (field.DbField.ToLower().Contains("decimal"))
                                {
                                    dbman. AddParameters(k, "@" + field.NameEng.Replace(' ', '_'),
                                                            decimal.Parse(row.ElementAt(i)));
                                }
                                else
                                {
                                    if (field.DbField == ("CHAR(38)"))
                                    {
                                        dbman. AddParameters(k, "@" + field.NameEng.Replace(' ', '_'),
                                                                Guid.NewGuid().ToString());
                                    }
                                    else
                                    {
                                        dbman. AddParameters(k, "@" + field.NameEng.Replace(' ', '_'),
                                                                row.ElementAt(i));
                                    }
                                }
                            }
                        }
                        k++;
                        i++;
                    }

                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(")");
                dbman.ExecuteNonQuery(CommandType.Text, sb.ToString());
                dbman.CommitTransaction();
            }
            catch (Exception ex)
            {
                dbman.RollBackTransaction();
                Logger.Instance().WriteLogError(ex.Message, "public static bool SaveRow(IEnumerable<string> roww, LookupModel lookup)");
            }

            finally
            {
                dbman.Dispose();
            }
            return true;
        }
        public static bool SaveRow(IEnumerable<string> roww, LookupModel lookup,int firmaId)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                List<string> row=new List<string>(roww);
                //lookup.Fields.Add(new TableField { DbField = "integer", GROUP = 4, Id = 4, Length = 4, IsRequared = false, NameEng = "FIRMAID", Name = "Фирма Номер" });
                if (roww != null)
                {
                    if (lookup.Fields[1].DbField == "integer")
                    {
                        string cmd = string.Format("Select * from \"{0}\" where \"{1}\"={2} and FIRMAID={3}",
                                                   lookup.LookUpMetaData.Tablename, lookup.Fields[1].NameEng, row[1],
                                                   firmaId);

                        var reader = dbman. ExecuteReader(CommandType.Text, cmd);
                        if (reader.Read())
                        {
                           
                            return false;
                        }
                    }
                }
                dbman. CreateParameters(lookup.Fields.Count - 1);

                if (row.Count == lookup.Fields.Count)
                {
                    row[row.Count - 1] = firmaId.ToString();
                }
                else
                {
                    row.Add(firmaId.ToString());
                }
                StringBuilder sb = new StringBuilder();
                //                INSERT INTO "nom_1" ("Id", "Name", "Address", "Telefon")
                // VALUES (
                //*Id, 
                //Name, 
                //Address, 
                //Telefon
                //)
                sb.AppendFormat("INSERT INTO \"{0}\" (", lookup.LookUpMetaData.Tablename);
                foreach (var field in lookup.Fields)
                {
                    if (field.NameEng != "Id") sb.AppendFormat("\"{0}\",", field.NameEng);
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(") Values (");
                var i = 0;
                var k = 0;
                foreach (var field in lookup.Fields)
                {

                    if (field != null)
                    {
                        if (field.NameEng == "Id")
                        {
                            i++;
                            continue;
                        }
                        sb.AppendFormat("@{0},", field.NameEng.Replace(' ', '_'));
                        if (field.DbField.ToLower() == "integer")
                        {
                            dbman. AddParameters(k, "@" + field.NameEng.Replace(' ', '_'),
                                                    int.Parse(row.ElementAt(i)));
                        }
                        else
                        {
                            if (field.DbField.ToLower().Contains("date"))
                            {
                                dbman. AddParameters(k, "@" + field.NameEng.Replace(' ', '_'),
                                                        DateTime.Parse(row.ElementAt(i)));
                            }
                            else
                            {
                                if (field.DbField.ToLower().Contains("decimal"))
                                {
                                    dbman. AddParameters(k, "@" + field.NameEng.Replace(' ', '_'),
                                                            decimal.Parse(row.ElementAt(i)));
                                }
                                else
                                {
                                    if (field.DbField == ("CHAR(38)"))
                                    {
                                        dbman. AddParameters(k, "@" + field.NameEng.Replace(' ', '_'),
                                                                Guid.NewGuid().ToString());
                                    }
                                    else
                                    {
                                        dbman. AddParameters(k, "@" + field.NameEng.Replace(' ', '_'),
                                                                row.ElementAt(i));
                                    }
                                }
                            }
                        }
                        k++;
                        i++;
                    }

                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(")");
                dbman. ExecuteNonQuery(CommandType.Text, sb.ToString());
                dbman. CommitTransaction();
            }
            catch (Exception ex)
            {
                dbman. RollBackTransaction();
                Logger.Instance().WriteLogError(ex.Message, "public static bool SaveRow(IEnumerable<string> roww, LookupModel lookup,int firmaId)");
            }

            finally
            {
                dbman. Dispose();
            }
            return true;
        }

        internal static IEnumerable<Conto> GetAllContoGrupedByContragent(int firmaId, DateTime from, DateTime to, string nom,int accid)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            List<Conto> allConto = new List<Conto>();
            try
            {
                dbman.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("select c.*,cm.LOOKUPVAL,cm.\"VALUE\" from \"conto\" c");
                sb.Append(" inner join \"accounts\" a on a.\"Id\"= c.\"CreditAccount\"");
                sb.Append(" inner join \"accounts\" b on b.\"Id\"= c.\"DebitAccount\"");
                sb.Append(" left outer join CONTOMOVEMENT cm on cm.CONTOID = c.\"Id\"");
                //sb.Append(" left outer join DDSDNEV d on d.NOM=c.\"Id\"");
                sb.AppendFormat(" where \"FirmId\"={0}", firmaId);
                sb.AppendFormat(" AND \"Date\">='{0}.{1}.{2}' and \"Date\"<='{3}.{4}.{5}'",
                   from.Day,
                   from.Month,
                   from.Year,
                   to.Day,
                   to.Month,
                   to.Year);
                sb.AppendFormat(" AND cm.ACCFIELDKEY=28 AND (c.\"CreditAccount\"={0} OR  c.\"DebitAccount\"={0})", accid);
                sb.Append(" AND (cm.ACCFIELDKEY=28 or cm.\"VALUE\" is null)");
                string s = sb.ToString();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    var c = new Conto();
                    c.Id = int.Parse(dbman.DataReader["Id"].ToString());
                    c.CartotecaCredit = int.Parse(dbman.DataReader["CartotecaCredit"].ToString());
                    c.CartotekaDebit = int.Parse(dbman.DataReader["CartotekaDebit"].ToString());
                    c.CreditAccount = int.Parse(dbman.DataReader["CreditAccount"].ToString());
                    c.Data = DateTime.Parse(dbman.DataReader["Date"].ToString());
                    c.DataInvoise = DateTime.Parse(dbman.DataReader["DataInvoise"].ToString());
                    c.DebitAccount = int.Parse(dbman.DataReader["DebitAccount"].ToString());
                    c.DocumentId = int.Parse(dbman.DataReader["DocumentId"].ToString());
                    c.FirmId = int.Parse(dbman.DataReader["FirmId"].ToString());
                    c.Reason = dbman.DataReader["Reason"].ToString();
                    c.Note = dbman.DataReader["Note"].ToString();
                    c.NumberObject = int.Parse(dbman.DataReader["NumberObject"].ToString());
                    c.Oborot = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                    c.DocNum = dbman.DataReader["DOCNUM"].ToString();
                    c.OborotValutaD = decimal.Parse(dbman.DataReader["OBOROTVALUTA"].ToString());
                    c.OborotKolD = decimal.Parse(dbman.DataReader["OBOROTKOL"].ToString());
                    c.OborotValutaK = decimal.Parse(dbman.DataReader["OBOROTVALUTAK"].ToString());
                    c.OborotKolK = decimal.Parse(dbman.DataReader["OBOROTKOLK"].ToString());
                    c.Folder = dbman.DataReader["FOLDER"].ToString();
                    c.Nd = int.Parse(dbman.DataReader["PORNOM"].ToString());
                    c.IsDdsPurchases = int.Parse(dbman.DataReader["ISDDSPURCHASES"].ToString());
                    c.IsDdsSales = int.Parse(dbman.DataReader["ISDDSSALES"].ToString());
                    c.IsDdsPurchasesIncluded = int.Parse(dbman.DataReader["ISDDSPURCHASESINCLUDED"].ToString());
                    c.IsDdsSalesIncluded = int.Parse(dbman.DataReader["ISDDSSALESINCLUDED"].ToString());
                    c.VopPurchases = dbman.DataReader["VOPPURCHASES"].ToString();
                    c.VopSales = dbman.DataReader["VOPSALES"].ToString();
                    c.IsSales = int.Parse(dbman.DataReader["ISSALES"].ToString());
                    c.IsPurchases = int.Parse(dbman.DataReader["ISPURCHASES"].ToString());
                    c.DDetails = dbman.DataReader["DDETAILS"].ToString();
                    c.CDetails = dbman.DataReader["CDETAILS"].ToString();
                    c.UserId = int.Parse(dbman.DataReader["USERID"].ToString());
                    c.Pr1 = dbman.DataReader["PR1"].ToString();
                    c.Pr2 = dbman.DataReader["PR2"].ToString();
                    c.KD = dbman.DataReader["KD"].ToString();
                    c.DDetails= dbman.DataReader["VALUE"].ToString();
                    c.CDetails= dbman.DataReader["LOOKUPVAL"].ToString();
                    allConto.Add(c);
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static IEnumerable<Conto> GetContosByContragent(int firmaId, DateTime from, DateTime to, int code,string nom)");
            }

            finally
            {
                dbman.Dispose();
            }

            return allConto;
        }

        internal static List<List<string>> CheckSellsPurchases(DateTime fromDate, DateTime toDate, int kindDDS)
        {
            List<List<string>> list = new List<List<string>>();
            string command = string.Format("SELECT a.DOCN,a.NAMEKONTR,COUNT(a.DOCN) as CC FROM DDSDNEV a where a.KINDACTIVITY = {0} and a.DATADOC>='{1}.{2}.{3}' and a.DATADOC<='{4}.{5}.{6}' GROUP BY a.DOCN,a.NAMEKONTR having COUNT(a.DOCN) > 1"
                ,kindDDS
                ,fromDate.Day,fromDate.Month,fromDate.Year
                ,toDate.Day, toDate.Month, toDate.Year);
            if (kindDDS==2)
                command = string.Format("SELECT a.DOCN,COUNT(a.DOCN) as CC FROM DDSDNEV a where a.KINDACTIVITY = {0} and a.DATADOC>='{1}.{2}.{3}' and a.DATADOC<='{4}.{5}.{6}' GROUP BY a.DOCN having COUNT(a.DOCN) > 1"
                , kindDDS
                , fromDate.Day, fromDate.Month, fromDate.Year
                , toDate.Day, toDate.Month, toDate.Year);
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, command);
                while (dbman.DataReader.Read())
                {
                    var row = new List<string>();
                    row.Add(dbman.DataReader["DOCN"].ToString());
                    if (kindDDS == 1)
                    {
                        row.Add(dbman.DataReader["NAMEKONTR"].ToString());
                    }
                    else
                    {
                        row.Add("");
                    }
                    row.Add(dbman.DataReader["CC"].ToString());
                    list.Add(row);
                    
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<LookUpMetaData> GetSystemLookups()");
            }

            finally
            {
                dbman.Dispose();
            }
            return list;
        
    }

        public static IEnumerable<TableField> GetLookUpFields(int id)
        {
            string command =
                string.Format(
                    "SELECT f.\"Id\", f.\"Name\", f.\"DBField\", f.\"IsNull\", f.NAMEENG,f.\"Length\",f.RFIELDKEY," +
                    "f.RTABLENAME,f.RFIELDNAME,f.RCODELOOKUP,f.\"GROUP\",d.ISREQUARED,d.ISUNIQUE,d.TN FROM \"lookups\" a Inner join \"lookupsdetails\" d " +
                    "on a.\"Id\"=d.\"IdLookUp\" Inner join \"lookupsfield\" f on d.\"IdLookField\"=f.\"Id\" Where a.\"Id\"={0} order by f.\"GROUP\",d.SORTORDER",
                    id);
            List<TableField> list = new List<TableField>();
            list.Add(new TableField { DbField = "Id", Name = "Ключ", Id = 0, IsRequared = true, Length = 10, NameEng = "Id" });
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;

            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, command);
                while (dbman. DataReader.Read())
                {
                    list.Add(new TableField
                    {
                        Id = int.Parse(dbman. DataReader["Id"].ToString()),
                        Name = dbman. DataReader["Name"].ToString(),
                        NameEng = dbman. DataReader["NameEng"].ToString(),
                        DbField = dbman. DataReader["DbField"].ToString(),
                        IsRequared = int.Parse(dbman. DataReader["ISREQUARED"].ToString()) == 1 ? true : false,
                        IsUnique = int.Parse(dbman. DataReader["ISUNIQUE"].ToString()) == 1 ? true : false,
                        Length = int.Parse(dbman. DataReader["Length"].ToString()),
                        RFIELDKEY = dbman. DataReader["RFIELDKEY"].ToString(),
                        RFIELDNAME = dbman. DataReader["RFIELDNAME"].ToString(),
                        RTABLENAME = dbman. DataReader["RTABLENAME"].ToString(),
                        GROUP = int.Parse(dbman. DataReader["GROUP"].ToString()),
                        Tn = dbman. DataReader["TN"].ToString(),
                        //RCODELOOKUP = int.Parse(dbman. DataReader["RCODELOOKUP"] != null ? dbman. DataReader["RCODELOOKUP"].ToString() : "0")
                        
                    });
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message,"public static IEnumerable<TableField> GetLookUpFields(int id)");
            }

            finally
            {
                dbman. Dispose();
            }

            return list;
        } 
        public static LookupModel GetLookup(int numer)
        {
            //var lm = LookUpRepository.Instance.LookUp(numer);
            //if (lm!=null)
            //{
            //    return lm;
            //}
            List<TableField> lt = GetLookUpFields(numer).ToList();
            var meta = GetAllLookups("").FirstOrDefault(e => e.Id == numer);
            var lm=new LookupModel(lt,meta);
            //LookUpRepository.Instance.Add(numer,lm);
            return lm;
        }
        public static bool DeleteRow(List<string> row, LookupModel lookup)
        {
            bool rezult = true;

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Delete from \"{0}\" WHERE \"Id\"={1}", lookup.LookUpMetaData.Tablename, row[0]);
                dbman. ExecuteNonQuery(CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static bool DeleteRow(List<string> row, LookupModel lookup)");
                rezult = false;
            }

            finally
            {
                dbman. Dispose();
            }
            return rezult;
        }
        public static bool DeleteLookUp(LookUpMetaData lookup)
        {
            bool result = true;

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Delete from \"lookups\" WHERE \"Id\"={0}", lookup.Id);
                dbman.ExecuteNonQuery(CommandType.Text, sb.ToString());
                sb.Clear();
                sb.AppendFormat("Delete from \"lookupsdetails\" WHERE \"IdLookUp\"={0}", lookup.Id);
                dbman.ExecuteNonQuery(CommandType.Text, sb.ToString());
                sb.Clear();
                sb.AppendFormat("DROP TRIGGER \"triger{0}\"", lookup.Tablename);
                dbman.ExecuteNonQuery(CommandType.Text, sb.ToString());
                sb.Clear();
                sb.AppendFormat("DROP GENERATOR GENERATOR{0}", lookup.Tablename.ToUpper());
                dbman.ExecuteNonQuery(CommandType.Text, sb.ToString());
                sb.Clear();
                sb.AppendFormat("Drop table \"{0}\"", lookup.Tablename);
                dbman.ExecuteNonQuery(CommandType.Text, sb.ToString());
                dbman.CommitTransaction();

            }
            catch (Exception ex)
            {
                dbman. RollBackTransaction();
                Logger.Instance().WriteLogError(ex.Message, "public static bool DeleteLookUp(LookUpMetaData lookup)");
                result = false;
            }

            finally
            {
                dbman. Dispose();
            }
            return result;
        }
        public static bool UpdateLookup(LookupModel l)
        {
            bool result = true;

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(3);
                dbman.AddParameters(0, "@Id", l.LookUpMetaData.Id);
                dbman.AddParameters(1, "@Name", l.LookUpMetaData.Name);
                dbman.AddParameters(2, "@Description", l.LookUpMetaData.Description);
                StringBuilder sb = new StringBuilder();
                sb.Append("Update \"lookups\" SET \"Name\" = @Name,DESCRIPTION=@Description  WHERE \"Id\"=@Id");
                dbman. ExecuteNonQuery(CommandType.Text, sb.ToString());

                foreach (var field in l.Fields)
                {
                    string commands =
                        string.Format(
                            "Update \"lookupsdetails\" SET ISUNIQUE={0},ISREQUARED={1},TN='{4}' WHERE \"IdLookField\"={3} and \"IdLookUp\"={2}",
                             field.IsUnique ? 1 : 0, field.IsRequared ? 1 : 0, l.LookUpMetaData.Id, field.Id,field.Tn??"");
                    dbman. ExecuteNonQuery(CommandType.Text, commands);
                   
                }
                dbman. CommitTransaction();

            }
            catch (Exception ex)
            {
                dbman. RollBackTransaction();
                Logger.Instance().WriteLogError(ex.Message,"public static bool UpdateLookup(LookupModel l)");
                result = false;
            }

            finally
            {
                dbman. Dispose();
            }
            return result;
        }
        public static IEnumerable<LookUpMetaData> GetSystemLookups()
        {
            List<LookUpMetaData> list = new List<LookUpMetaData>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman. ExecuteReader(CommandType.Text, "select * from \"syslookups\"");
                while (dbman. DataReader.Read())
                {
                    list.Add(new LookUpMetaData
                    {
                        Id = (int)dbman. DataReader["Id"],
                        Name = dbman. DataReader["Name"].ToString(),
                        Description = dbman. DataReader["Description"].ToString(),
                        Tablename = dbman. DataReader["Tablename"].ToString()
                    });
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<LookUpMetaData> GetSystemLookups()");
            }

            finally
            {
                dbman. Dispose();
            }
            return list;
        }
    }
}
