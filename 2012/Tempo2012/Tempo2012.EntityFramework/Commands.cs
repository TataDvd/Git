using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework
{
    public static class Commands
    {
        public static string InsertSaldos
        {
            get
            {
                return
                    @"INSERT INTO SALDOS (SALDODEBIT, SALDOKREDIT, SALDOVALUTADEBIT, SALDOVALUTAKREDIT, ACCID, PARTIDID, ANALITICID) VALUES ({0},{1},{2},{3},{4},{5},{6})";
            }
        }

        public static string InsertFirm
        {
            get
            {
                //return "insert into \"firm\"(  \"Name\", \"Bulstad\", \"DDSnum\", \"City\", \"Country\", \"Address\", \"Telefon\", \"Presentor\", \"NameBoss\", \"EGN\", \"PresentorYN\", \"Names\", \"Tel\", \"FirstName\", \"SurName\", \"City2\", \"Address2\", \"LastName\")values(  @Name, @Bulstad, @DDSnum, @City, @Country, @Address, @Telefon, @Presentor, @NameBoss, @EGN, @PresentorYN, @Names, @Tel, @FirstName, @SurName, @City2, @Address2, @LastName)";
                return "IP_firm";
            }
        }

        public static string InsertLookup
        {
            get { return "IP_lookup"; }
        }

        public static string UpdateFirm
        {
            get
            {
                return
                    "update \"firm\" set  \"Name\" = @Name, \"Bulstad\" = @Bulstad, \"DDSnum\" = @DDSnum, \"City\" = @City, \"Country\" = @Country, \"Address\" = @Address, \"Telefon\" = @Telefon, \"Presentor\" = @Presentor, \"NameBoss\" = @NameBoss, \"EGN\" = @EGN, \"PresentorYN\" = @PresentorYN, \"Names\" = @Names, \"Tel\" = @Tel, \"FirstName\" = @FirstName, \"SurName\" = @SurName, \"City2\" = @City2, \"Address2\" = @Address2, \"LastName\" = @LastName, REGISERDDS=@REGISERDDS,NA=@NA where  \"Id\" = @Id";
            }
        }

        public static string InsertAccount
        {
            get
            {
                //return "insert into \"firm\"(  \"Name\", \"Bulstad\", \"DDSnum\", \"City\", \"Country\", \"Address\", \"Telefon\", \"Presentor\", \"NameBoss\", \"EGN\", \"PresentorYN\", \"Names\", \"Tel\", \"FirstName\", \"SurName\", \"City2\", \"Address2\", \"LastName\")values(  @Name, @Bulstad, @DDSnum, @City, @Country, @Address, @Telefon, @Presentor, @NameBoss, @EGN, @PresentorYN, @Names, @Tel, @FirstName, @SurName, @City2, @Address2, @LastName)";
                return "IP_ACCOUNTS";
            }
        }

        public static string UpdateAccount
        {
            get
            {
                return
                    "update \"account\"set \"Id\" = @Id, \"TypeSaldo\" = @TypeSaldo, \"TypeAccount\" = @TypeAccount, \"SubNum\" = @SubNum, \"PartidNum\" = @PartidNum, \"NameMain\" = @NameMain, \"TypeSaldo\" = @TypeSaldo, \"NameMainEng\" = @NameMainEng, \"Presentor\" = @Presentor, \"NameBoss\" = @NameBoss, \"EGN\" = @EGN, \"PresentorYN\" = @PresentorYN, \"Names\" = @Names, \"Tel\" = @Tel, \"FirstName\" = @FirstName, \"SurName\" = @SurName, \"City2\" = @City2, \"Address2\" = @Address2, \"LastName\" = @LastName where  \"Id\" = @Id";
            }
        }



        public static string insertAA
        {
            get { return "insert into \"analiticalaccount\"(\"Name\",\"TypeID\") values(@Name,@TypeId)"; }
        }

        public static string UpdateAA
        {
            get { return "update \"analiticalaccount\" set \"Name\"=@Name,\"TypeID\"=@TypeId where \"Id\"=@Id"; }
        }

        public static string DeleteAAConector
        {
            get { return "delete from \"conectoranaliticfield\" where \"AnaliticalNameID\"={0}"; }
        }

        public static string DeleteATConector
        {
            get { return "delete from MPATYPETOAFIELD where ATYPEID={0}"; }
        }

        public static string DeleteAA
        {
            get { return "delete from \"analiticalaccount\" where \"Id\"={0}"; }
        }

        public static string DeleteAT
        {
            get { return "delete from \"analiticalaccounttype\" where \"Id\"={0}"; }
        }

        public static string insertAT
        {
            get { return "insert into \"analiticalaccounttype\"(\"Name\",SL,SV,KOL) values(@Name,@Sl,@SV,@KOL)"; }
        }

        public static string UpdateAT
        {
            get { return "update \"analiticalaccounttype\" set \"Name\"=@Name,SV=@SV,SL=@SL,KOL=@KOL where \"Id\"=@Id"; }
        }

        public static string DeleteSaldos
        {
            get { return @"delete from SALDOS where ACCID={0} and PARTIDID={1}"; }
        }
        public static string InsertMovement
        {
            get
            {
                return "INSERT INTO MOVEMENT (ACCID,FIELDID,LOOKUPID,VALUE,VALUEINT,VALUEdecimal,VALUEDATE) VALUES (@ACCID,@FIELDID,@LOOKUPID,@VALUE,@VALUEINT,@VALUEdecimal,@VALUEDATE)";
            }
        }
        public static string DeleteMovement
        {
            get
            {
                return "DELETE FROM MOVEMENT WHERE (ID=@ID)";
            }
        }
        public static string UpdateMovement
        {
            get
            {
                return "UPDATE MOVEMENT SET VALUE=@VALUE,VALUE=@VALUEINT,VALUEdecimal=@VALUEdecimal,VALUEDATE=@VALUEDATE  WHERE (ID=@ID)";
            }
        }
    }
}
