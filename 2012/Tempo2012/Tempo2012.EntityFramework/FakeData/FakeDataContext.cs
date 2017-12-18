using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;
using System.IO;

namespace Tempo2012.EntityFramework.FakeData
{
    public static class FakeDataContext
    {
        private const string FIRMAFILENAME = "Firma.xml";
        private const string CITYFILENAME = "Cityfile.xml";
        private const string COUNTRYFILENAME = "Countryfile.xml";
        private const string ACCOUNTFILENAME = "Accountfile.xml";
        private const string ANALITICALACCOUNT = "AnaliticalAccount.xml";
        private const string ANALITICALACCOUNTTYPE = "AnaliticalAccountType.xml";
        private const string ANALITICALFIELDS = "AnaliticalFields.xml";
        private const string MapAnanaliticAccToAnaliticField = "MapAnanaliticAccToAnaliticField.xml";
        private const string CONTO = "Conto.xml";
        private const string CONTODEBITS = "Contodebit.xml";
        private const string CONTOCREDITS = "Contocredit.xml";
        private const string NATIONALCONTO = "NationalAccouts.xml";
        private const string NOMENCLATURENOMENCLATURE = "NomenNomen.xml";
        private const string NOMENCLATUREFIELDS="NomenFields.xml";
        private const string NOMENKLATURECONECTIONS = "NomenConections.xml";
        #region FakeFirms
        public static IEnumerable<FirmModel> GetAllFirma()
        {
            List<FirmModel> AllFirmaList=new List<FirmModel>();
            if (File.Exists(FIRMAFILENAME))
            {
                AllFirmaList=SerializeUtil.DeserializeFromXML<List<FirmModel>>(FIRMAFILENAME);
            }
            else
            {
                AllFirmaList.AddRange(new List<FirmModel>
                {
                    new FirmModel
                    {
                        Address="Tran",
                        Address2="Pernik",
                        Bulstad="12121221",
                        DDSnum="433433334",
                        EGN="7310153783",
                        FirstName="Антон",
                        City=1,
                        Id=1,
                        LastName="Таков",
                        Name="Антон ЕООД",
                        NameBoss="Антон Таков",
                        Names="Станка Такич",
                        Presentor="Милен Таков",
                        PresentorYN=1,
                        SurName="Недялков",
                        Tel="0898706435",
                        Telefon="343322333",
                        Country=2,
                        City1 = 1,
                    },
                    new FirmModel
                    {
                        Address="Tran",
                        Address2="Pernik",
                        Bulstad="12121221",
                        DDSnum="433433334",
                        EGN="7310153783",
                        FirstName="Антон",
                        City=1,
                        City1=2,
                        Id=2,
                        LastName="Таков",
                        Name="Stanka ЕООД",
                        NameBoss="Антон Таков",
                        Names="Станка Такич",
                        Presentor="Милен Таков",
                        PresentorYN=1,
                        SurName="Недялков",
                        Tel="0898706435",
                        Telefon="343322333",
                        
                    },
                    new FirmModel
                    {
                        Address="Tran",
                        Address2="Pernik",
                        Bulstad="12121221",
                        DDSnum="433433334",
                        EGN="7310153783",
                        FirstName="Антон",
                        City=1,
                        City1=2,
                        Id=1,
                        LastName="Таков",
                        Name="Бастун ЕООД",
                        NameBoss="Антон Таков",
                        Names="Станка Такич",
                        Presentor="Милен Таков",
                        PresentorYN=1,
                        SurName="Недялков",
                        Tel="0898706435",
                        Telefon="343322333",
                       
                    }
                }
                );
                SerializeUtil.SerializeToXML<List<FirmModel>>(FIRMAFILENAME,AllFirmaList);
            }
            return AllFirmaList;
        }
        public static IEnumerable<City> GetAllSity()
        {
            List<City> allsityes=new List<City>();
            if (File.Exists(CITYFILENAME))
            {
                allsityes = SerializeUtil.DeserializeFromXML<List<City>>(CITYFILENAME);
            }
            else
            {
                allsityes.AddRange(
                new List<City>
                    {
                        new City{CountryId = 1,Id=1,Name = "Трън",Zip = "2460"},
                        new City{CountryId = 1,Id=2,Name = "Перник",Zip = "2300"},
                        new City{CountryId = 1,Id=3,Name = "София",Zip = "1000"},
                        new City{CountryId = 1,Id=4,Name = "Пловдив",Zip = "2000"},
                        new City{CountryId = 1,Id=5,Name = "Русе",Zip = "3000"},
                    });
                SerializeUtil.SerializeToXML<List<City>>(CITYFILENAME, allsityes);
            }
            return allsityes;
        }
        public static IEnumerable<Country> GetAllCountry()
        {
            List<Country> allcountry = new List<Country>();
            if (File.Exists(COUNTRYFILENAME))
            {
                allcountry = SerializeUtil.DeserializeFromXML<List<Country>>(COUNTRYFILENAME);
            }
            else
            {
                allcountry.AddRange(
                new List<Country>
                    {
                        new Country{Id=1,Name = "България"},
                        new Country{Id=2,Name = "Англия"},
                        new Country{Id=3,Name = "САЩ"},
                        new Country{Id=4,Name = "ЮАР"}
                    });
                SerializeUtil.SerializeToXML<List<Country>>(COUNTRYFILENAME,allcountry);
            }
            return allcountry;
        }
        #endregion
        #region FakeAccounts
        public static IEnumerable<AnaliticalAccount> GetAllAnaliticalAccount()
        {
            List<AnaliticalAccount> aa=new List<AnaliticalAccount>();
            if (File.Exists(ANALITICALACCOUNT))
            {
                aa=SerializeUtil.DeserializeFromXML<List<AnaliticalAccount>>(ANALITICALACCOUNT);
            }
            else
            {
                aa.AddRange(new List<AnaliticalAccount>
                {
                    new AnaliticalAccount{Id=1,Name="Аналитична сметка",TypeID=1},
                    new AnaliticalAccount{Id=2,Name="Аналитична сметка с валута",TypeID=2},
                    new AnaliticalAccount{Id=3,Name="Дълготрайни активи",TypeID=1},
                    new AnaliticalAccount{Id=4,Name="Материали",TypeID=5},
                    new AnaliticalAccount{Id=5,Name="Материаали с валута",TypeID=6},
                    new AnaliticalAccount{Id=6,Name="Контрагенти",TypeID=1},
                    new AnaliticalAccount{Id=7,Name="Контрагенти с валута",TypeID=2},
                    new AnaliticalAccount{Id=8,Name="Контрагенти с падеж",TypeID=3},
                    new AnaliticalAccount{Id=9,Name="Контрагенти с валута и с падеж",TypeID=4}
                });
                SerializeUtil.SerializeToXML<List<AnaliticalAccount>>(ANALITICALACCOUNT, aa);
            }
            return aa;
        }
        public static IEnumerable<AnaliticalAccountType> GetAllAnaliticalAccountType()
        {
            List<AnaliticalAccountType> aat = new List<AnaliticalAccountType>();
            if (File.Exists(ANALITICALACCOUNTTYPE))
            {
                aat = SerializeUtil.DeserializeFromXML<List<AnaliticalAccountType>>(ANALITICALACCOUNTTYPE);
            }
            else
            {
                aat.AddRange(new List<AnaliticalAccountType>
                {
                    new AnaliticalAccountType{Id=1,Name="Стойностна"},
                    new AnaliticalAccountType{Id=2,Name="Валутна"},
                    new AnaliticalAccountType{Id=3,Name="Стойностна с падеж"},
                    new AnaliticalAccountType{Id=4,Name="Валутна с падеж"},
                    new AnaliticalAccountType{Id=5,Name="Материална"},
                    new AnaliticalAccountType{Id=6,Name="Материална с валута"},
                    new AnaliticalAccountType{Id=7,Name="Дълготрайни активи"}
                });
                SerializeUtil.SerializeToXML<List<AnaliticalAccountType>>(ANALITICALACCOUNTTYPE, aat);
            };
            return aat;
        }
        public static IEnumerable<AnaliticalFields> GetAllAnaliticalFields()
        {
            List<AnaliticalFields> allanal = new List<AnaliticalFields>();
            if (File.Exists(ANALITICALFIELDS))
            {
                allanal = SerializeUtil.DeserializeFromXML<List<AnaliticalFields>>(ANALITICALFIELDS);
            }
            else
            {
                allanal.AddRange(new List<AnaliticalFields>
                {
                
                    new AnaliticalFields{Id=1,Name="Сума лева",FieldType="money"},
                    new AnaliticalFields{Id=2,Name="Сума валута",FieldType="money"},
                    new AnaliticalFields{Id=3,Name="Дата падеж",FieldType="data"},
                    new AnaliticalFields{Id=4,Name="Количество",FieldType="int"},
                    new AnaliticalFields{Id=5,Name="Мярка",FieldType="string"}
                });
                SerializeUtil.SerializeToXML<List<AnaliticalFields>>(ANALITICALFIELDS, allanal);
            }
            return allanal;
        }
        public static IEnumerable<MapAnanaliticAccToAnaliticField> GetAllConnectorAnaliticField()
        {
            List<MapAnanaliticAccToAnaliticField> conn = new List<MapAnanaliticAccToAnaliticField>();
            if (File.Exists(MapAnanaliticAccToAnaliticField))
            {
                conn = SerializeUtil.DeserializeFromXML<List<MapAnanaliticAccToAnaliticField>>(MapAnanaliticAccToAnaliticField);
            }
            else
            {
                conn.AddRange(new List<MapAnanaliticAccToAnaliticField>
                {
                    new MapAnanaliticAccToAnaliticField{Id=1,AnaliticalFieldId=1,AnaliticalNameID=1},
                    new MapAnanaliticAccToAnaliticField{Id=2,AnaliticalFieldId=2,AnaliticalNameID=1},
                    new MapAnanaliticAccToAnaliticField{Id=3,AnaliticalFieldId=2,AnaliticalNameID=2},
                    new MapAnanaliticAccToAnaliticField{Id=4,AnaliticalFieldId=3,AnaliticalNameID=1},
                    new MapAnanaliticAccToAnaliticField{Id=5,AnaliticalFieldId=3,AnaliticalNameID=3},
                    new MapAnanaliticAccToAnaliticField{Id=6,AnaliticalFieldId=4,AnaliticalNameID=1},
                    new MapAnanaliticAccToAnaliticField{Id=7,AnaliticalFieldId=4,AnaliticalNameID=2},
                    new MapAnanaliticAccToAnaliticField{Id=8,AnaliticalFieldId=4,AnaliticalNameID=3},
                    new MapAnanaliticAccToAnaliticField{Id=9,AnaliticalFieldId=5,AnaliticalNameID=1},
                    new MapAnanaliticAccToAnaliticField{Id=10,AnaliticalFieldId=5,AnaliticalNameID=4},
                    new MapAnanaliticAccToAnaliticField{Id=11,AnaliticalFieldId=5,AnaliticalNameID=5},
                    new MapAnanaliticAccToAnaliticField{Id=12,AnaliticalFieldId=6,AnaliticalNameID=1},
                    new MapAnanaliticAccToAnaliticField{Id=13,AnaliticalFieldId=6,AnaliticalNameID=2},
                    new MapAnanaliticAccToAnaliticField{Id=14,AnaliticalFieldId=6,AnaliticalNameID=4},
                    new MapAnanaliticAccToAnaliticField{Id=15,AnaliticalFieldId=6,AnaliticalNameID=5},
                    new MapAnanaliticAccToAnaliticField{Id=16,AnaliticalFieldId=7,AnaliticalNameID=1}
                });
                SerializeUtil.SerializeToXML<List<MapAnanaliticAccToAnaliticField>>(MapAnanaliticAccToAnaliticField, conn);
            }
            return conn;
        }
        public static IEnumerable<AccountsModel> GetAllAccounts()
        {
            List<AccountsModel> allacc = new List<AccountsModel>();
            if (File.Exists(ACCOUNTFILENAME))
            {
                allacc = SerializeUtil.DeserializeFromXML<List<AccountsModel>>(ACCOUNTFILENAME);
            }
            else
            {
                allacc.AddRange(new List<AccountsModel>
            {
                new AccountsModel
                {
                    Id=1,
                    Num=101,
                    SubNum=1,
                    AnaliticalNum=0,
                    PartidNum=0,
                    NameMain="Собствен капитал",
                    NameMainEng="Own capital",
                    FirmaId=1,
                    LevelAccount=1,
                    NameSub="Магазин",
                    NameSubEng="For Sale",
                    TypeSaldo=1,
                    TypeAccount=1,
                    TypeAccountEx = 1
                },
                new AccountsModel
                {
                    Id=1,
                    Num=101,
                    SubNum=2,
                    AnaliticalNum=0,
                    PartidNum=0,
                    NameMain="Собствен капитал",
                    NameMainEng="Own capital",
                    FirmaId=1,
                    LevelAccount=2,
                    NameSub="Магазин",
                    NameSubEng="For Sale",
                    TypeSaldo=1,
                    TypeAccount=1,
                    TypeAccountEx = 1
                },
                new AccountsModel
                {
                    Id=1,
                    Num=101,
                    SubNum=3,
                    AnaliticalNum=0,
                    PartidNum=0,
                    NameMain="Собствен капитал",
                    NameMainEng="Own capital",
                    FirmaId=1,
                    LevelAccount=2,
                    NameSub="Магазин",
                    NameSubEng="For Sale",
                    TypeSaldo=2,
                    TypeAccount=4,
                    TypeAccountEx = 1
                },
            });
                SerializeUtil.SerializeToXML<List<AccountsModel>>(ACCOUNTFILENAME,allacc);
            }
            return allacc;
        }
        #endregion
        #region FakeUsers
        public static IEnumerable<User> GetAllUsers()
        {
            return new List<User>
            {
                new User{Id=1,Name="Антон Таков", PassWord="12345", Rights=ConstantsBinary.All, UserName="delioff",CanUpdateConto = true},
                new User{Id=6,Name="Антон Таков", PassWord="offdeli", Rights=ConstantsBinary.All, UserName="delioff",CanDeleteConto = true},
                new User{Id=2,Name="Денка Марина", PassWord="12345", Rights=ConstantsBinary.All, UserName="denka",CanFinishMonth = true},
            };
        }
        #endregion
        #region FakeLookUpSpecific
        public static IEnumerable<LookUpSpecific> GetKindDocuments()
        {
            List<LookUpSpecific> result=new List<LookUpSpecific>();
            if (File.Exists("kinddocsfordebit.xml"))
            {
                result = SerializeUtil.DeserializeFromXML<List<LookUpSpecific>>("kinddocsfordebit.xml");
            }
            else
            {
                result.AddRange(new List<LookUpSpecific>
                {
                    new LookUpSpecific{Id=1,Name="Фактура", CodetId="01"},
                    new LookUpSpecific{Id=2,Name="Дебитно известие", CodetId="02"},
                    new LookUpSpecific{Id=3,Name="Кредитно известие", CodetId="03"},
                    new LookUpSpecific{Id=4,Name="Митническа декларация", CodetId="07"},
                    new LookUpSpecific{Id=5,Name="Фактура", CodetId="19"},
                    new LookUpSpecific{Id=6,Name="Фактура", CodetId="50"},
                    new LookUpSpecific{Id=7,Name="Фактура", CodetId="51"},
                    new LookUpSpecific{Id=8,Name="Фактура", CodetId="52"},

                });
                SerializeUtil.SerializeToXML<List<LookUpSpecific>>("kinddocsfordebit.xml",result);
            }
            return result;
        }
        public static IEnumerable<LookUpSpecific> GetNationalAccounts()
        {
            List<LookUpSpecific> result = new List<LookUpSpecific>();
            if (File.Exists(NATIONALCONTO))
            {
                result = SerializeUtil.DeserializeFromXML<List<LookUpSpecific>>(NATIONALCONTO);
            }
            else
            {
                loadFromOldFile(@"D:\tempo\SYSTEM\SMETKI.DTA", result);
                //result.AddRange(new List<LookUpSpecific>
                //{
                //    new LookUpSpecific{Id=1,Name="Фактура", CodetId="01"},
                //    new LookUpSpecific{Id=2,Name="Дебитно известие", CodetId="02"},
                //    new LookUpSpecific{Id=3,Name="Кредитно известие", CodetId="03"},
                //    new LookUpSpecific{Id=4,Name="Митническа декларация", CodetId="07"},
                //    new LookUpSpecific{Id=5,Name="Фактура", CodetId="19"},
                //    new LookUpSpecific{Id=6,Name="Фактура", CodetId="50"},
                //    new LookUpSpecific{Id=7,Name="Фактура", CodetId="51"},
                //    new LookUpSpecific{Id=8,Name="Фактура", CodetId="52"},

                //});
                SerializeUtil.SerializeToXML<List<LookUpSpecific>>(NATIONALCONTO, result);
            }
            return result;
        }
        public static string convertBgdostoDos866(string fileName)
        {
            byte[] bufer = null;
            if (File.Exists(fileName))
            {
                bufer = File.ReadAllBytes(fileName);
                for (int i = 0; i < bufer.Length; i++)
                {
                    if ((bufer[i] >= 176) && (bufer[i] <= 191))
                    {
                        bufer[i] += 48;
                    }
                }
            }
            return Encoding.GetEncoding(866).GetString(bufer);
        }
        public static void loadFromOldFile(string fileName, List<LookUpSpecific> allacounts)
        {
            if (File.Exists(fileName))
            {
                // Read in lines from file.
                allacounts.Clear();
                string text = convertBgdostoDos866(fileName);
                using (StringReader reader = new StringReader(text))
                {
                    string line;
                    int id=0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Length > 3)
                        {
                            string fields = line.Substring(0, 3);
                            int k = line.IndexOf('|');
                            string fields1 = line.Substring(4, k);
                            k = fields1.IndexOf('|');
                            fields1 = line.Substring(4, k);
                            allacounts.Add(new LookUpSpecific {Id=id,Name=fields1,CodetId=fields});
                        }
                    }
                }
            }
        }
        
        #endregion
        public static void Save<T>(T list)
        {
            string path=COUNTRYFILENAME;
            if (list is List<FirmModel>) path=FIRMAFILENAME;
            if (list is List<City>) path=CITYFILENAME;
            if (list is List<AccountsModel>) path=ACCOUNTFILENAME;
            if (list is List<Conto>) path = CONTO;
            SerializeUtil.SerializeToXML<T>(path,list);
        }
        #region Conto
        public static IEnumerable<Conto> GetAllConto()
        {
            List<Conto> allConto = new List<Conto>();
            if (File.Exists(CONTO))
            {
                allConto = SerializeUtil.DeserializeFromXML<List<Conto>>(CONTO);
            }
            else
            {
                allConto.Add(new Conto
                {
                    CartotecaCredit=0,
                    CartotekaDebit=1,
                    CreditAccount=1,
                    Data=new DateTime(2011,8,7),
                    DataInvoise=new DateTime(2011,7,7),
                    DebitAccount=0,
                    DocumentId=1,
                    FirmId=1,
                    Folder="8",
                    Id=1,
                    Note="Плащане по фактура",
                    NumberObject=1,
                    Oborot=100.5m,
                    Reason="Фактура N 500",
                    UserId = Config.CurrentUser.Id
                });
                allConto.Add(new Conto
                {
                    CartotecaCredit = 1,
                    CartotekaDebit = 0,
                    CreditAccount = 2,
                    Data = new DateTime(2011, 8, 8),
                    DataInvoise = new DateTime(2011, 7, 8),
                    DebitAccount = 0,
                    DocumentId = 1,
                    FirmId = 1,
                    Folder = "8",
                    Id = 1,
                    Note = "Плащане по фактура",
                    NumberObject = 1,
                    Oborot = 500.5m,
                    Reason = "Фактура N 501",
                    UserId = Config.CurrentUser.Id
                });
                SerializeUtil.SerializeToXML<List<Conto>>(CONTO, allConto);
            }
            return allConto;
        }
        public static IEnumerable<CartotecaCredit> GetAllCartotecaCredit()
        {
            List < CartotecaCredit> Cc= new List<CartotecaCredit>();
            if (File.Exists(CONTOCREDITS))
            {
                Cc = SerializeUtil.DeserializeFromXML<List<CartotecaCredit>>(CONTOCREDITS);
            }
            else
            {
                Cc.Add(new CartotecaCredit {Id=1,ContoId=1,TitleValue="Сума лева",TypeValue="money",Value="100.50"});
                Cc.Add(new CartotecaCredit { Id = 2, ContoId = 2, TitleValue = "Сума лева", TypeValue = "money", Value = "200.50" });
                SerializeUtil.SerializeToXML<List<CartotecaCredit>>(CONTOCREDITS,Cc);
            }
            return Cc;
        }
        public static IEnumerable<CartotecaDebit> GetAllCartotecaDebit()
        {
            List<CartotecaDebit> Cc = new List<CartotecaDebit>();
            if (File.Exists(CONTODEBITS))
            {
                Cc = SerializeUtil.DeserializeFromXML<List<CartotecaDebit>>(CONTODEBITS);
            }
            else
            {
                Cc.Add(new CartotecaDebit { Id = 1, ContoId = 1, TitleValue = "Сума лева", TypeValue = "money", Value = "50.50" });
                Cc.Add(new CartotecaDebit { Id = 2, ContoId = 2, TitleValue = "Сума лева", TypeValue = "money", Value = "150.50" });
                SerializeUtil.SerializeToXML<List<CartotecaDebit>>(CONTODEBITS, Cc);
            }
            return Cc;
        }
        #endregion
        #region LookUpSpecific
        public static IEnumerable<IEnumerable<string>> GetNomenclatureContent(int idnomen)
        {
            List<List<string>> worker=new List<List<string>>();
            LookUpMetaData nomenclatureHedar = GetAllLookUpSpecific().Where(e => e.Id == idnomen).FirstOrDefault();
            List<LookUpMetaData> metadata=new List<LookUpMetaData>(GetNomenlatureFields(idnomen));
            List<string> fields = new List<string>();
            foreach (LookUpMetaData m in metadata)
            {
                fields.Add(m.Name);
            }
            worker.Add(fields);
            if (nomenclatureHedar != null)
            {
                List<string> content = new List<string>(LoadContentinlist(nomenclatureHedar.Name+"nomen.xml"));
                List<string> row=new List<string>();
                int i = 0;
                foreach (var current in content)
                {
                    row.Add(current);
                    i++;
                    if (i==metadata.Count)
                    {
                        worker.Add(row);
                        row=new List<string>();
                        i = 0;
                    }
                }
            }
            return worker;
        }
        public static IEnumerable<string> LoadContentinlist(string nomenname)
        {
            List<string> list=new List<string>();
            if (File.Exists(nomenname))
            {
                list=SerializeUtil.DeserializeFromXML<List<string>>(nomenname);
            }
            else
            {
                list.AddRange(new List<string>{
                "1","Microsofr","Tran",
                "2","Test","Tran",
                "3","Koce","Burgas"
                });
                SerializeUtil.SerializeToXML<List<string>>(nomenname,list);
            }
            return list;
        }

        public static IEnumerable<LookUpMetaData> GetAllLookUpSpecific()
        {
            List<LookUpMetaData> list=new List<LookUpMetaData>();
            if (File.Exists(NOMENCLATURENOMENCLATURE))
            {
                list = SerializeUtil.DeserializeFromXML<List<LookUpMetaData>>(NOMENCLATURENOMENCLATURE);
            }
            else
            {
                list = new List<LookUpMetaData>
                           {
                               new LookUpMetaData {Description = "Клиенти", Id = 1, Name = "Клиенти"},
                               new LookUpMetaData {Description = "Контрагенти", Id = 2, Name = "Контрагенти"},
                               new LookUpMetaData {Description = "Доставчици", Id = 3, Name = "Доставчици"},
                               new LookUpMetaData {Description = "Валута", Id = 4, Name = "Валута"},
                               new LookUpMetaData {Description = "Материали", Id = 5, Name = "Материали"},
                           };
                SerializeUtil.SerializeToXML<List<LookUpMetaData>>(NOMENCLATURENOMENCLATURE, list);
            }
            return list;
        }
        public static IEnumerable<LookUpMetaData> GetAllNomenclatureFields()
        { 
            List<LookUpMetaData> list=new List<LookUpMetaData>();
            if (File.Exists(NOMENCLATUREFIELDS))
            {
                list = SerializeUtil.DeserializeFromXML<List<LookUpMetaData>>(NOMENCLATUREFIELDS);
            }
            else
            {
               list = new List<LookUpMetaData>{
                                                         new LookUpMetaData
                                                             {
                                                                 Id = 1,
                                                                 Description = "Булстат",
                                                                 Name = "Булстат",
                                                                 Type = "string"
                                                             },
                                                         new LookUpMetaData
                                                             {
                                                                 Id = 2,
                                                                 Description = "Сума лева",
                                                                 Name = "Сума лева",
                                                                 Type = "money"
                                                             },
                                                         new LookUpMetaData
                                                             {
                                                                 Id = 3,
                                                                 Description = "Сума валута",
                                                                 Name = "Сума валута",
                                                                 Type = "money"
                                                             },
                                                         new LookUpMetaData
                                                             {
                                                                 Id = 4,
                                                                 Description = "ДДС Номер",
                                                                 Name = "ДДС Номер",
                                                                 Type = "string"
                                                             },
                                                         new LookUpMetaData
                                                             {
                                                                 Id =5,
                                                                 Description = "Град",
                                                                 Name = "Град",
                                                                 Type = "string"
                                                             },
                                                         new LookUpMetaData
                                                             {
                                                                 Id = 6,
                                                                 Description = "Банкова сметка",
                                                                 Name = "Банкова сметка",
                                                                 Type = "money"
                                                             },
                                                         new LookUpMetaData
                                                             {
                                                                 Id =7,
                                                                 Description = "Номер",
                                                                 Name = "Номер",
                                                                 Type = "int"
                                                             },
                                                         new LookUpMetaData
                                                             {
                                                                 Id = 8,
                                                                 Description = "Адрес",
                                                                 Name = "Адрес",
                                                                 Type = "string"
                                                             },
                                                         new LookUpMetaData
                                                             {
                                                                 Id = 9,
                                                                 Description = "Единична цена лева",
                                                                 Name = "Единична цена лева",
                                                                 Type = "string"
                                                             },
                                                         new LookUpMetaData
                                                             {
                                                                 Id = 10,
                                                                 Description = "Единична цена валута",
                                                                 Name = "Единична цена валута",
                                                                 Type = "string"
                                                             },
                                                     };
                SerializeUtil.SerializeToXML<List<LookUpMetaData>>(NOMENCLATUREFIELDS,list);
                
            }
            return list;
        }
        public static IEnumerable<LookUpToFields> GetAllConections()
        {
            List<LookUpToFields> list = new List<LookUpToFields>();
            if (File.Exists(NOMENKLATURECONECTIONS))
            {
                list = SerializeUtil.DeserializeFromXML<List<LookUpToFields>>(NOMENKLATURECONECTIONS);
            }
            else
            {
                list = new List<LookUpToFields>
                           {
                               new LookUpToFields {IdLookUpField = 1, IdLookUp = 1},
                               new LookUpToFields {IdLookUpField = 1, IdLookUp = 2},
                               new LookUpToFields {IdLookUpField = 1, IdLookUp = 3},
                               new LookUpToFields {IdLookUpField = 1, IdLookUp = 4},
                               new LookUpToFields {IdLookUpField = 1, IdLookUp = 5},
                               new LookUpToFields {IdLookUpField = 2, IdLookUp = 1},
                               new LookUpToFields {IdLookUpField = 2, IdLookUp = 2},
                               new LookUpToFields {IdLookUpField = 2, IdLookUp = 3},
                               new LookUpToFields {IdLookUpField = 3, IdLookUp = 1},
                               new LookUpToFields {IdLookUpField = 3, IdLookUp = 2},
                               new LookUpToFields {IdLookUpField = 4, IdLookUp = 1},
                               new LookUpToFields {IdLookUpField = 4, IdLookUp = 2},
                               new LookUpToFields {IdLookUpField = 4, IdLookUp = 3},
                               new LookUpToFields {IdLookUpField = 4, IdLookUp = 4},
                               new LookUpToFields {IdLookUpField = 4, IdLookUp = 5},
                               new LookUpToFields {IdLookUpField = 5, IdLookUp = 1},
                               new LookUpToFields {IdLookUpField = 5, IdLookUp = 2},
                               new LookUpToFields {IdLookUpField = 5, IdLookUp = 3},
                               new LookUpToFields {IdLookUpField = 5, IdLookUp = 4},
                               new LookUpToFields {IdLookUpField = 5, IdLookUp = 5}
                           };
                SerializeUtil.SerializeToXML<List<LookUpToFields>>(NOMENKLATURECONECTIONS,list);
            }
            return list;
        }
        public static void SaveConectors(List<LookUpToFields> list)
        {
            List<LookUpToFields> rez =new List<LookUpToFields>(GetAllConections()); 
            foreach (var conector in list)
            {
                rez.Add(conector);
            }
            SerializeUtil.SerializeToXML<List<LookUpToFields>>(NOMENKLATURECONECTIONS, rez);
        }

        public static void SaveNomenclatureHeader(LookUpMetaData nomenclatureHedar)
        {
            List<LookUpMetaData> rez = new List<LookUpMetaData>(GetAllLookUpSpecific());
            rez.Add(nomenclatureHedar);
            SerializeUtil.SerializeToXML<List<LookUpMetaData>>(NOMENCLATURENOMENCLATURE, rez);
        }

        public static IEnumerable<LookUpMetaData> GetNomenlatureFields(int numer)
        {
            List<LookUpMetaData> list = GetAllNomenclatureFields().ToList();
            List<LookUpMetaData> result=new List<LookUpMetaData>();
            List<LookUpToFields> conector = GetAllConections().Where(e => e.IdLookUpField == numer).ToList();
            foreach (var curconector in conector)
            {
                LookUpMetaData newmetada = list.Where(e => e.Id == curconector.IdLookUp).FirstOrDefault();
                result.Add(newmetada);
            }
            return result;
        }

        #endregion
    }
}
