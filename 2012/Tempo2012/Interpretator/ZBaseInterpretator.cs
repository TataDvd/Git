using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Interpretator
{
    public class SecNameReck
    {
        public string Name;
        public int Line; 
    }
    public class FunNameReck
    {
        public string Name;
        public int Line;
        public string Val;
    }
    public class ValReck {
        public string Name;
        public byte ValTyp;
        public bool Flag;
        public string SValue;
        public decimal RValue;
        public int IValue;
        public DateTime DValue;
        public bool BValue;
         }
    public class RezWType {
       public string  Name;
       public int  ParNum;
       public byte IncC;
       public byte DecC;
       public byte Flist;
       public byte Err;
       public byte Fpush;
    }
    public class IntConstReck {
        public string Name;
        public byte RetType;
        }
    public class FunType
    {
        public string Name;
        public int ParNum;
        public byte[] ParTyp;
        public byte RetTyp;
    }
    public class ZBaseInterpretator
    {
        public int WordC { get; set;}
        public int LineC { get; set;}
        public int MaxC { get; set;}
        public int CurrentLine { get; private set; }
        public string CurrentWord { get; private set; }
        public string GetWord { get; private set; }
        public string[] PAR = new string[] { "", "", "", "", "", "","","","","","",""};
        public int[] Counters=new int[] {0,0,0,0,0,0,0,0,0,0};
        private  readonly char[] OpenParentheses = { '(', '[' };
        private  readonly char[] CloseParentheses = { ')', ']'};

        public RezWType CurrtIrw { get; private set; }
        public Object FindPointer { get; private set; }
        public ValReck VV1=new ValReck();
        public ValReck VV2=new ValReck();
        public ValReck VV3=new ValReck();

        public List<string> TextLines = new List<string>();
        public List<SecNameReck> SectionRoot = new List<SecNameReck>();
        public List<SecNameReck> LabelRoot = new List<SecNameReck>();
        public List<FunNameReck> FunRoot = new List<FunNameReck>();
        public List<ValReck> VarRoot = new List<ValReck>();
        public List<string> OutPut = new List<string>();
        public  List<RezWType> Irw = new List<RezWType>
            {
                new RezWType {Name = "IF"            ,ParNum = 2,IncC=5, DecC=0, Flist=0,Err=0,Fpush=0},//1
                new RezWType {Name = "THEN"          ,ParNum =-2,IncC=0, DecC=0, Flist=0,Err=1,Fpush=0},//2
                new RezWType {Name = "ELSE"          ,ParNum =-3,IncC=0, DecC=0, Flist=0,Err=1,Fpush=0},//3
                new RezWType {Name = "ENDIF"         ,ParNum = 0,IncC=0, DecC=5, Flist=0,Err=1,Fpush=0},//4
                new RezWType {Name = "LOOP"          ,ParNum = 1,IncC=1, DecC=0, Flist=3,Err=3,Fpush=1},//5
                new RezWType {Name = "ENDLOOP"       ,ParNum = 0,IncC=0, DecC=0, Flist=0,Err=0,Fpush=0},//6
                new RezWType {Name = "WHILE"         ,ParNum = 2,IncC=2, DecC=0, Flist=0,Err=0,Fpush=1},//7
                new RezWType {Name = "DO"            ,ParNum =-2,IncC=0, DecC=5, Flist=0,Err=7,Fpush=0},//8
                new RezWType {Name = "ENDWHILE"      ,ParNum = 0,IncC=0, DecC=2, Flist=0,Err=7,Fpush=0},//9
                new RezWType {Name = "REPEAT"        ,ParNum = 0,IncC=4, DecC=0, Flist=0,Err=0,Fpush=1},//10
                new RezWType {Name = "UNTIL"         ,ParNum = 1,IncC=0, DecC=4, Flist=0,Err=10,Fpush=0},//11
                new RezWType {Name = "FOR"           ,ParNum = 5,IncC=3, DecC=0, Flist=3,Err=3,Fpush=1},//12
                new RezWType {Name = "FROM"          ,ParNum =-2,IncC=0, DecC=0, Flist=0,Err= 7,Fpush=0},//13
                new RezWType {Name = "TO"            ,ParNum =-4,IncC=0, DecC=0, Flist=0,Err= 7,Fpush=0},//14
                new RezWType {Name = "ENDFOR"        ,ParNum = 0,IncC=0, DecC=3, Flist=0,Err=12,Fpush=0},//15
                new RezWType {Name = "SECTION"       ,ParNum = 1,IncC=0, DecC=0, Flist=0,Err= 0,Fpush=0},//16
                new RezWType {Name = "ENDSECTION"    ,ParNum = 0,IncC=0, DecC=0, Flist=0,Err= 0,Fpush=0},//17
                new RezWType {Name = "PROCEDURE"     ,ParNum = 1,IncC=6, DecC=0, Flist=0,Err= 0,Fpush=0},//18
                new RezWType {Name = "ENDPROC"       ,ParNum = 0,IncC=0, DecC=6, Flist=0,Err=13,Fpush=0},//19
                new RezWType {Name = "LABEL"         ,ParNum = 1,IncC=0, DecC=0, Flist=0,Err= 0,Fpush=0},//20
                new RezWType {Name = "CALL"          ,ParNum = 1,IncC=6, DecC=0, Flist=2,Err= 7,Fpush=1},//21
                new RezWType {Name = "JUMP"          ,ParNum = 1,IncC=0, DecC=0, Flist=1,Err= 6,Fpush=0},//22
                new RezWType {Name = "DEFVAR"        ,ParNum = 1,IncC=0, DecC=0, Flist=3,Err= 0,Fpush=0},//23
                new RezWType {Name = "FSEEK"         ,ParNum = 4,IncC=7, DecC=0, Flist=4,Err= 3,Fpush=1},//24
                new RezWType {Name = "ENDSEEK"       ,ParNum = 1,IncC=0, DecC=7, Flist=0,Err=24,Fpush=0},//25
                new RezWType {Name = "APPLYSECTION"  ,ParNum = 1,IncC=0, DecC=0, Flist=0,Err= 0,Fpush=0},//26
                new RezWType {Name = "DFOR"          ,ParNum = 5,IncC=8, DecC=0, Flist=3,Err= 3,Fpush=1},//27
                new RezWType {Name = "DFROM"         ,ParNum =-2,IncC=0, DecC=0, Flist=0,Err= 7,Fpush=0},//28
                new RezWType {Name = "DTO"           ,ParNum =-4,IncC=0, DecC=0, Flist=0,Err= 7,Fpush=0},//29
                new RezWType {Name = "ENDDFOR"       ,ParNum = 0,IncC=0, DecC=8, Flist=0,Err=12,Fpush=0} //30
                
            };

        public List<IntConstReck> IntConst=new List<IntConstReck> {
                new IntConstReck{Name="CONFIG"           ,RetType=3},
                new IntConstReck{Name="CURENTDATE"       ,RetType=5},
                new IntConstReck{Name="SYSTEMDATE"       ,RetType=5},
                new IntConstReck{Name="MR_MODE"          ,RetType=1},
                new IntConstReck{Name="GROUPCOUNTER"     ,RetType=1},
                new IntConstReck{Name="CURENTTIME"       ,RetType=3},
                new IntConstReck{Name="IKONOMISTNUM"     ,RetType=1},
                new IntConstReck{Name="FIRMANAME"        ,RetType=3},
                new IntConstReck{Name="IKONOMISTNAME"    ,RetType=3},
                new IntConstReck{Name="DDSREG"           ,RetType=2},
                new IntConstReck{Name="SOUNDFLAG"        ,RetType=2},
                new IntConstReck{Name="PASWORDFLAG"      ,RetType=2},
                new IntConstReck{Name="SHOWIKONOM"       ,RetType=2},
                new IntConstReck{Name="NETSTRING"        ,RetType=3},
                new IntConstReck{Name="CURENTZFILE"      ,RetType=3},
                new IntConstReck{Name="CURENTBUFFER"     ,RetType=1},
                new IntConstReck{Name="CURENTPOLE"       ,RetType=1},
                new IntConstReck{Name="DATA_DIR"         ,RetType=3},
                new IntConstReck{Name="ZERROR"           ,RetType=1},
                new IntConstReck{Name="ZSTATUS"          ,RetType=1},
                new IntConstReck{Name="INTERROR"         ,RetType=1},
                new IntConstReck{Name="ENABLEEROR"       ,RetType=1},
                new IntConstReck{Name="CURENTOPER"       ,RetType=1},
                new IntConstReck{Name="CURENTPRAVILO"    ,RetType=1},
                new IntConstReck{Name="SPTR"             ,RetType=1},
                new IntConstReck{Name="MEMAVAIL"         ,RetType=1},
                new IntConstReck{Name="MAXAVAIL"         ,RetType=1},
                new IntConstReck{Name="MR_MODE"          ,RetType=1},
                new IntConstReck{Name="FSPOS"            ,RetType=1},
                new IntConstReck{Name="REGISTER1"        ,RetType=4},
                new IntConstReck{Name="REGISTER2"        ,RetType=4},
                new IntConstReck{Name="REGISTER3"        ,RetType=4},
                new IntConstReck{Name="REGISTER4"        ,RetType=4},
                new IntConstReck{Name="REGISTER5"        ,RetType=4},
                new IntConstReck{Name="REGISTER6"        ,RetType=1},
                new IntConstReck{Name="REGISTER7"        ,RetType=1},
                new IntConstReck{Name="REGISTER8"        ,RetType=1},
                new IntConstReck{Name="REGISTER9"        ,RetType=2},
                new IntConstReck{Name="REGISTER10"       ,RetType=2},
                new IntConstReck{Name="REGISTER11"       ,RetType=3},
                new IntConstReck{Name="ZAPREG1"          ,RetType=4},
                new IntConstReck{Name="ZAPREG2"          ,RetType=4},
                new IntConstReck{Name="ZAPREG3"          ,RetType=4},
                new IntConstReck{Name="ZAPREG4"          ,RetType=4},
                new IntConstReck{Name="ZAPREG5"          ,RetType=4},
                new IntConstReck{Name="ZAPREG6"          ,RetType=4},
                new IntConstReck{Name="ZAPREG7"          ,RetType=4},
                new IntConstReck{Name="ZAPREG8"          ,RetType=4},
                new IntConstReck{Name="ZAPREG9"          ,RetType=4},
                new IntConstReck{Name="ZAPREG10"         ,RetType=4},
                new IntConstReck{Name="SREGISTER1"       ,RetType=3},
                new IntConstReck{Name="SREGISTER2"       ,RetType=3},
                new IntConstReck{Name="SREGISTER3"       ,RetType=3},
                new IntConstReck{Name="SREGISTER4"       ,RetType=3},
                new IntConstReck{Name="SREGISTER5"       ,RetType=3},
                new IntConstReck{Name="SREGISTER6"       ,RetType=3},
                new IntConstReck{Name="SREGISTER7"       ,RetType=3},
                new IntConstReck{Name="SREGISTER7"       ,RetType=3},
                new IntConstReck{Name="SREGISTER8"       ,RetType=3},
                new IntConstReck{Name="SREGISTER9"       ,RetType=3},
                new IntConstReck{Name="PHONES"           ,RetType=3},
                new IntConstReck{Name="DDSMOL"           ,RetType=3},
                new IntConstReck{Name="DATEDDSREG"       ,RetType=3},
                new IntConstReck{Name="LASTDATEDDSREG"   ,RetType=3},
                new IntConstReck{Name="DDSPROCENT"       ,RetType=1},
                new IntConstReck{Name="LOOPDATE"         ,RetType=5 }
        };
        public List<FunType> Intfa = new List<FunType>{
               new FunType{Name="LINECHAR"         ,ParNum=2,ParTyp=new byte[]{1  ,3  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="TEXTSUMA"         ,ParNum=1,ParTyp=new byte[]{14,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="LEVASUMA"         ,ParNum=1,ParTyp=new byte[]{14,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="CENTER"           ,ParNum=2,ParTyp=new byte[]{13,11,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="COPY"             ,ParNum=3,ParTyp=new byte[]{13,11,11,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="DELETE"           ,ParNum=3,ParTyp=new byte[]{13,11,11,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="FC"               ,ParNum=3,ParTyp=new byte[]{14,11,11,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="FL"               ,ParNum=2,ParTyp=new byte[]{13,11,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="FR"               ,ParNum=2,ParTyp=new byte[]{13,11,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="FI"               ,ParNum=3,ParTyp=new byte[]{14,11,11,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="LENGTH"           ,ParNum=1,ParTyp=new byte[]{13,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="GETSYSTEMHELP"    ,ParNum=1,ParTyp=new byte[]{11,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="HELPMESSAGE"      ,ParNum=2,ParTyp=new byte[]{11,13,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="MESSAGE"          ,ParNum=1,ParTyp=new byte[]{13,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="ASK"              ,ParNum=1,ParTyp=new byte[]{13,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="INSERT"           ,ParNum=2,ParTyp=new byte[]{13,13,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="DELETESPACE"      ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="EQU"              ,ParNum=2,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="EQL"              ,ParNum=2,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="EQH"              ,ParNum=2,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="NOT"              ,ParNum=2,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="LOW"              ,ParNum=2,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="HIGH"             ,ParNum=2,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="CMP"              ,ParNum=2,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="CMPSQL"           ,ParNum=2,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="TESTOPEN"         ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="BUFFNUMER"        ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="VERSION"          ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="LOGICALNAME"      ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="POLENUMER"        ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="TABLENUMER"       ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="RECKNUMER"        ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="RECKLEN"          ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="POSITION"         ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="LASTDATE"         ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=5},
               new FunType{Name="LASTTIME"         ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="INTERNALDEL"      ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="LOCKFILE"         ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="UNLOCKFILE"       ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="READRECK"         ,ParNum=2,ParTyp=new byte[]{13,11,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="WRITERECK"        ,ParNum=2,ParTyp=new byte[]{13,11,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="OPENTEXTREPORT"   ,ParNum=0,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="READLINE"         ,ParNum=0,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="CREATETEXTREPORT" ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="CLOSETEXTREPORT"  ,ParNum=0,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="REPORTLINE"       ,ParNum=-1,ParTyp=new byte[]{0 ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="REPORT"           ,ParNum=-1,ParTyp=new byte[]{0 ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="TESTCLOSE"        ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="TABLEINDEX"       ,ParNum=5,ParTyp=new byte[]{3  ,3  ,13,13,11,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="TABLEREPORT"      ,ParNum=5,ParTyp=new byte[]{3  ,3  ,13,13,11,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="FINDRECK"         ,ParNum=2,ParTyp=new byte[]{13,3  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="INT"              ,ParNum=1,ParTyp=new byte[]{14,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="BOOL"             ,ParNum=1,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="STRING"           ,ParNum=1,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="REAL"             ,ParNum=1,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=4},
               new FunType{Name="CREATEFILE"       ,ParNum=2,ParTyp=new byte[]{3  ,11,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="DELETEFILE"       ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="ADDBLANK"         ,ParNum=2,ParTyp=new byte[]{13,11,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="DATE"             ,ParNum=1,ParTyp=new byte[]{13,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="FCR"              ,ParNum=1,ParTyp=new byte[]{14,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=4},
               new FunType{Name="FLL"              ,ParNum=2,ParTyp=new byte[]{14,11,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=4},
               new FunType{Name="BIP"              ,ParNum=0,ParTyp=new byte[]{0  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="DELAY"            ,ParNum=1,ParTyp=new byte[]{11,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="KONTO"            ,ParNum=1,ParTyp=new byte[]{13,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="EOF"              ,ParNum=0,ParTyp=new byte[]{  0,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="ADDRECK"          ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="COPYRECK"         ,ParNum=2,ParTyp=new byte[]{3  ,3  ,0  ,0  ,0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="ADDZAPREG"        ,ParNum=3,ParTyp=new byte[]{14,11,11,0  ,0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="SETRELATION"      ,ParNum=3,ParTyp=new byte[]{3  ,13,13,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="SELECTFROM"       ,ParNum=2,ParTyp=new byte[]{13,13,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="DOUBLEINSERT"     ,ParNum=1,ParTyp=new byte[]{11,0  ,0  ,0  ,0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="DTABLEREPORT"     ,ParNum=10,ParTyp=new byte[]{3  ,3  ,13,13,11,13,  11,13,  13,11},RetTyp=0},
               new FunType{Name="DISPOSELINES"     ,ParNum=1,ParTyp=new byte[]{  3,  0,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="INSERTLINES"      ,ParNum=2,ParTyp=new byte[]{  3,11,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="DISABLEFUNKEY"    ,ParNum=1,ParTyp=new byte[]{11,  0,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="FLR"              ,ParNum=1,ParTyp=new byte[]{14,  0,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=4},
               new FunType{Name="EXEC"             ,ParNum=1,ParTyp=new byte[]{13,  0,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="SUMRECK"          ,ParNum=1,ParTyp=new byte[]{  3,  0,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="FINDFIRST"        ,ParNum=2,ParTyp=new byte[]{  3,13,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="EXECSECTION"      ,ParNum=1,ParTyp=new byte[]{13,  0,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="DAYWEEK"          ,ParNum=1,ParTyp=new byte[]{13,  0,  0,  0,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=1},
               new FunType{Name="COPYEMPTYFILE"    ,ParNum=2,ParTyp=new byte[]{  3,  3,  0,  0,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=4},
               new FunType{Name="CLEARZAPREG"      ,ParNum=0,ParTyp=new byte[]{  0,  0,  0,  0,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=0},
               new FunType{Name="YEAR"             ,ParNum=1,ParTyp=new byte[]{13,  0,  0,  0,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=1},
               new FunType{Name="MONTH"            ,ParNum=1,ParTyp=new byte[]{13,  0,  0,  0,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=1},
               new FunType{Name="DAY"              ,ParNum=1,ParTyp=new byte[]{13,  0,  0,  0,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=1},
               new FunType{Name="INDEX"            ,ParNum=1,ParTyp=new byte[]{  3,  0,  0,  0,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=0},
               new FunType{Name="ADDINDEXRECK"     ,ParNum=1,ParTyp=new byte[]{  3,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="DL"               ,ParNum=1,ParTyp=new byte[]{13,  0,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="EXECSPR"          ,ParNum=1,ParTyp=new byte[]{13,  0,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="LASTMONTH"        ,ParNum=1,ParTyp=new byte[]{13,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="DELETEINDEXRECK"  ,ParNum=2,ParTyp=new byte[]{13,11,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="LD"               ,ParNum=1,ParTyp=new byte[]{11,  0,  0,  0,  0,  0,  0  ,0  ,  0  ,0  },RetTyp=5},
               new FunType{Name="HAVEFILE"         ,ParNum=1,ParTyp=new byte[]{13,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="GETATSTRING"      ,ParNum=2,ParTyp=new byte[]{  3,13,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="GETATLONG"        ,ParNum=2,ParTyp=new byte[]{  3,13,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="GETATREAL"        ,ParNum=2,ParTyp=new byte[]{  3,13,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=4},
               new FunType{Name="GETATBOOL"        ,ParNum=2,ParTyp=new byte[]{  3,13,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="RTABLEINDEX"      ,ParNum=5,ParTyp=new byte[]{3  ,3  ,13,13,11,0    ,0  ,0    ,0  ,0  },RetTyp=0},
               new FunType{Name="RTABLEREPORT"     ,ParNum=5,ParTyp=new byte[]{3  ,3  ,13,13,11,0    ,0  ,0    ,0  ,0  },RetTyp=0},
               new FunType{Name="STEPLEFT"         ,ParNum=0,ParTyp=new byte[]{0  ,0  ,0  ,0  ,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=0},
               new FunType{Name="STEPRIGHT"        ,ParNum=0,ParTyp=new byte[]{0  ,0  ,0  ,0  ,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=0},
               new FunType{Name="STEPUP"           ,ParNum=0,ParTyp=new byte[]{0  ,0  ,0  ,0  ,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=0},
               new FunType{Name="STEPDN"           ,ParNum=0,ParTyp=new byte[]{0  ,0  ,0  ,0  ,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=0},
               new FunType{Name="LOGICALDELETE"    ,ParNum=2,ParTyp=new byte[]{  3,1  ,0  ,0  ,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=0},
               new FunType{Name="ЃЋ‹Ќ€—Ќ€"         ,ParNum=4,ParTyp=new byte[]{13,13,13,11,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=1},
               new FunType{Name="GETDATE"          ,ParNum=3,ParTyp=new byte[]{11,11,11,0  ,0  ,0    ,0  ,0    ,0  ,0  },RetTyp=5},
               new FunType{Name="SAVEBUFFER"       ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="RESTOREBUFFER"    ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="IFDELETED"        ,ParNum=1,ParTyp=new byte[]{3  ,0  ,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="RECKSELECTFROM"   ,ParNum=2,ParTyp=new byte[]{13,13,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="FTABLEINDEX"      ,ParNum=9,ParTyp=new byte[]{3  ,3  ,13,13,11,13,  11,13,  11,0  },RetTyp=0},
               new FunType{Name="FTABLEREPORT"     ,ParNum=9,ParTyp=new byte[]{3  ,3  ,13,13,11,13,  11,13,  11,0  },RetTyp=0},
               new FunType{Name="STABLEREPORT"     ,ParNum=9,ParTyp=new byte[]{3  ,3  ,13,13,11,11,  11,11,  13,0  },RetTyp=0},
               new FunType{Name="RRTABLEINDEX"     ,ParNum=9,ParTyp=new byte[]{3  ,3  ,13,13,11,13,  11,13,  11,0  },RetTyp=0},
               new FunType{Name="RRTABLEREPORT"    ,ParNum=9,ParTyp=new byte[]{3  ,3  ,13,13,11,13,  11,13,  11,0  },RetTyp=0},
               new FunType{Name="POS"              ,ParNum=2,ParTyp=new byte[]{13,13,0  ,0  ,0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=1},
               new FunType{Name="SETATSTRING"      ,ParNum=3,ParTyp=new byte[]{  3,13,13,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="PRINTFILE"        ,ParNum=3,ParTyp=new byte[]{  3,11,11,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="PRINTBLANK"       ,ParNum=1,ParTyp=new byte[]{  3,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="SETDELETED"       ,ParNum=1,ParTyp=new byte[]{  3,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="CLEAR"            ,ParNum=1,ParTyp=new byte[]{  3,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="DELETERECK"       ,ParNum=2,ParTyp=new byte[]{13,11,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="LS0"              ,ParNum=2,ParTyp=new byte[]{11,11,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="BACKDAY"          ,ParNum=1,ParTyp=new byte[]{  5,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=5},
               new FunType{Name="READINDEXRECK"    ,ParNum=3,ParTyp=new byte[]{13,13,11,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="NEWINDEX"         ,ParNum=3,ParTyp=new byte[]{13,13,11,0,  0  ,0    ,0  ,0    ,0  ,0  },RetTyp=0},
               new FunType{Name="ACTIVYEARSUMA"    ,ParNum=8,ParTyp=new byte[]{13,14,11,11,15,15,11  ,11,  0  ,0  },RetTyp=0},
               new FunType{Name="DATEINDELTA"      ,ParNum=3,ParTyp=new byte[]{15,15,15,0  ,0  ,0  ,0    ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="SETDATADIR"       ,ParNum=1,ParTyp=new byte[]{13,0  ,0  ,0  ,0  ,0  ,0    ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="SORTFILE"         ,ParNum=3,ParTyp=new byte[]{13,13,11,0,  0  ,0    ,0  ,0    ,0  ,0  },RetTyp=0},
               new FunType{Name="MINRECK"          ,ParNum=10,ParTyp=new byte[]{14,14,14,14,14,14,14,14,14,14}   ,RetTyp=3},
               new FunType{Name="PROCENT"          ,ParNum=2,ParTyp=new byte[]{14,14,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  }   ,RetTyp=4},
               new FunType{Name="FCC"              ,ParNum=3,ParTyp=new byte[]{14,11,11,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="COPYFILE"         ,ParNum=2,ParTyp=new byte[]{13,13,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=2},
               new FunType{Name="INCSTR"           ,ParNum=1,ParTyp=new byte[]{13,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="DOSWIN"           ,ParNum=1,ParTyp=new byte[]{13,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="WINDOS"           ,ParNum=1,ParTyp=new byte[]{13,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="ADDESCAPE"        ,ParNum=2,ParTyp=new byte[]{13,  13,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="LOADRTV"           ,ParNum=1,ParTyp=new byte[]{13,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},//{filename=string},//moe da zarezda nekaw hedar
               new FunType{Name="SAVERTV"           ,ParNum=1,ParTyp=new byte[]{13,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},//{filename=string},//save za po natam polzing
               new FunType{Name="REPORTLINERTV"     ,ParNum=-1,ParTyp=new byte[]{0,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},//<=>ReportLine
               new FunType{Name="REPORTRTV"         ,ParNum=-1,ParTyp=new byte[]{0,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},//<=>Report
               new FunType{Name="SETFONTRTV"        ,ParNum=1,ParTyp=new byte[]{13,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},//{seting=string},
               new FunType{Name="SETSTILERTV"       ,ParNum=1,ParTyp=new byte[]{13,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},//{seting=string},
               new FunType{Name="REPORTIMAGE"       ,ParNum=1,ParTyp=new byte[]{13,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},//{imagefilename=string},
               new FunType{Name="REPORTIMAGEBG"     ,ParNum=2,ParTyp=new byte[]{13,  1,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="SHOWRTV"           ,ParNum=0,ParTyp=new byte[]{0,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0},
               new FunType{Name="INSERTPASSWORD"    ,ParNum=2,ParTyp=new byte[]{13,13,0  ,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=3},
               new FunType{Name="CLOSEACTIVEFORM"   ,ParNum=0,ParTyp=new byte[]{0,  0,  0,0,  0  ,0  ,  0  ,0  ,  0  ,0  },RetTyp=0}
        };

        public void LoadProgramLine(string FileName)
        {
           var lines = File.ReadAllLines(FileName);
            WordC = 1;LineC = 0;MaxC = -1;
            foreach (string s in lines)
            {
                if (!string.IsNullOrWhiteSpace(s) && !s.StartsWith("{"))
                {
                    TextLines.Add(s);
                    if (s.StartsWith(Irw[15].Name))
                    {
                        var ss = s.Split(' ');
                        SectionRoot.Add(new SecNameReck { Name = ss[1], Line = MaxC });
                    }
                    if (s.StartsWith(Irw[9].Name))
                    {
                        var ss = s.Split(' ');
                        LabelRoot.Add(new SecNameReck { Name = ss[1], Line = MaxC });
                    }
                    if (s.StartsWith(Irw[17].Name))
                    {
                        var ss = s.Split(' ');
                        if (ss[1].Contains('('))
                        {
                            var sss = ss[1].Substring(0, ss[1].IndexOf('('));
                            var ssV = ss[1].Substring(ss[1].IndexOf('('), ss[1].IndexOf(')'));
                            FunRoot.Add(new FunNameReck { Name = sss, Line = MaxC,Val=ssV});
                        }
                        else
                        {
                            FunRoot.Add(new FunNameReck { Name = ss[1], Line = MaxC });
                        }
                       
                    }
                    MaxC++;
                }
            }
        }
        public void Apline_Sors()
        {
            if (TextLines.Count>0)
            {
                CurrentWord = TextLines[0];
                _GetWord();
                do
                {
                    Apply_Struct(true);
                }
                while (NoMax());

            }
        }

        private void Apply_Struct(bool ignore)
        {
            var i0 = CurrtIrw;
            var s = GetWord;
            if (CurrtIrw != null && NoMax())
            {
                var i1 = CurrtIrw.ParNum;
                var i = 1;
                while (i <= i1 && NoMax())
                {
                    IncWC();
                    PAR[i] = GetWord;
                    i++;
                }
                Counters[i0.IncC]++;
                Counters[i0.DecC]--;
                IncWC();
                if (ignore)
                {
                    var i2 = i0.Flist;
                    var i3 = i0.Err;
                    var i4 = i0.Fpush;
                    if (i2 == 2)
                    {
                        var i5 = Pos('(', PAR[1]);
                        if (i5 < 0) throw new Exception(CurrentWord);
                        PAR[6]= Copy(PAR[1], 1, i5);
                        PAR[1]= Copy(PAR[1], i5 + 1);
                        
                    }
                    switch (i2)
                    {
                        case 0: i3 = 0;
                            break;
                        case 1:
                            FindPointer = LabelRoot.FirstOrDefault(e => e.Name == PAR[1]);
                            break;
                        case 2:
                            FindPointer = FunRoot.FirstOrDefault(e => e.Name == PAR[6]);
                            break;
                        case 3:
                            FindPointer = VarRoot.FirstOrDefault(e => e.Name == PAR[1].ToUpper());
                            break;
                        case 4:
                            //FindPointer = VarRoot.FirstOrDefault(e => e.Name == PAR[6]);
                            break;
                    }
                    if (FindPointer == null && i3!=0) { throw new Exception("Грешка" + CurrentWord);}
                }
                
            }
            if (i0 != null)
            {
                switch (i0.Name)
                {
                    case "IF": Apply_If(i0, ignore); break;
                    case "LOOP": Apply_Loop(i0, ignore); break;
                    case "WHILE": Apply_While(i0, ignore); break;
                    case "REPEAT": Apply_Repeat(i0, ignore); break;
                    case "FOR": Apply_For(i0, ignore); break;
                    case "ENDFOR":
                        if (Counters[3] < 0)
                        {
                            throw new Exception("Грешка" + CurrentWord);
                        } break;
                    case "DEFVAR":
                        {
                            if (FindPointer == null && ignore)
                            {
                                var v = new ValReck();
                                v.Name = PAR[1].ToUpper();
                                v.ValTyp = 1;
                                VarRoot.Add(v);
                            }
                        }
                        break;
                }
            }
            else
            {
                //calculy
                if (Pos('=', CurrentWord) > -1)
                {
                    IncWC();
                    PAR[1] = GetWord;
                    if (PAR[1] != "=") throw new Exception("Грешка" + CurrentWord);
                    IncWC();
                    PAR[2] = GetWord;
                    if (ignore)
                    {
                        
                        Calculi(PAR[2],ref VV3);
                        if (!VV3.Flag) throw new Exception("Грешка" + CurrentWord);
                        if (Pos('[', s) == -1 && Pos('[', s) == -1)
                        {
                            SetDefValue(s, VV3);
                        }
                        else
                        {
                            SetOnePole(s, VV3);
                        }
                        if (!VV3.Flag) SetOnePole(s, VV3);
                        if (!VV3.Flag) throw new Exception("Грешка" + CurrentWord);
                    }
                    IncWC();
                }
                else
                {
                    if (ignore)
                    {
                        
                        Calculi(CurrentWord, ref VV3);
                        if (!VV3.Flag) throw new Exception("Грешка" + CurrentWord);
                        IncWC();
                    }
                    else
                    {
                        IncWC();
                    }
                }
            }

            
        }

        private void SetDefValue(string s, ValReck vV3)
        {
            var v = VarRoot.FirstOrDefault(e => e.Name == s);
            if (v!=null)
            {
                v.IValue = vV3.IValue;
                v.BValue = vV3.BValue;
                v.DValue = vV3.DValue;
                v.RValue = vV3.RValue;
                v.ValTyp = vV3.ValTyp;
                v.SValue = vV3.SValue;
                v.Flag = true;
            }
        }

        private void SetOnePole(string s, ValReck vV3)
        {
            throw new NotImplementedException();
        }

        private void Calculi(string s,ref ValReck vV3)
        {
            Stack<object> IST1 = new Stack<object>();
            Stack<object> IST2 = new Stack<object>();
            while (s.Length>1 && s[1] == '(' && s[s.Length] == ')')
            {
                s=Copy(s, 1, s.Length-1);
            }
            Translate(IST1, IST2, s);
            CalculiZ(IST1, IST2, ref vV3);
        }

        private void CalculiZ(Stack<object> iST1, Stack<object> iST2, ref ValReck V3)
        {
            ValReck V1=new ValReck();
            if (iST1.Count > 0)
            {
                if (iST1.Count % 2 == 0) throw new Exception("Грешка" + CurrentWord);
                foreach (var item in iST1)
                {
                    V1 = item as ValReck;
                    if (!OpChar(V1.Name[0])) iST2.Push(V1);
                    else
                    {
                        if (OpChar(V1.Name[0]))
                        {
                            VV2 = iST2.Pop() as ValReck;
                            VV1 = iST2.Pop() as ValReck;
                            _AplineOper(ref VV1, ref VV2, ref VV3, V1.Name[0]);
                            iST2.Push(VV3);
                        }
                    }
                }
                V1 = iST2.Pop() as ValReck;
            }
            V3 = V1;
        }

        private void _AplineOper(ref ValReck V1,ref ValReck V2, ref ValReck V3, char v)
        {
            V3.Flag = false;
            if (V1.ValTyp != V2.ValTyp)
            {
                _ConvertTypes(ref V1, ref V2);

            }
            if (!V1.Flag && !V2.Flag)
            {
                throw new Exception("Variables invalid");
            }
            __AplineOper(ref V1, ref V2, ref V3, v);
        }

        private void __AplineOper(ref ValReck v1, ref ValReck v2, ref ValReck v3, char v)
        {
            v3.Flag = true;
            switch (v)
            {
                case '+':__ADD(ref v1,ref v2,ref v3);break;
                case '*':__MUL(ref v1,ref v2,ref v3);break;
                case '-':__SUB(ref v1,ref v2,ref v3);break;
                case '/':__DIV(ref v1,ref v2,ref v3);break;
                case '%':__PROCENT(ref v1,ref v2,ref v3);break;
                case '^':__STEPEN(ref v1,ref v2,ref v3);break;
                case '&':__MUL(ref v1,ref v2,ref v3);break;
                case '|':__ADD(ref v1,ref v2,ref v3);break;
            }
        }

        private void __STEPEN(ref ValReck v1, ref ValReck v2, ref ValReck v3)
        {
            switch (v1.ValTyp)
            {
                case 1: v3.RValue =(decimal)Math.Pow(v1.IValue,v2.IValue);v3.ValTyp = 1; break;
                case 4: v3.RValue = (decimal)Math.Pow(v1.IValue,v2.IValue);v3.ValTyp = 4; break;
                default: v3.Flag = false; break;
            }
        }

        private void __PROCENT(ref ValReck v1, ref ValReck v2, ref ValReck v3)
        {
            v3.RValue = (v1.RValue / v2.RValue)*100;v3.ValTyp = 4;
        }

        private void __DIV(ref ValReck v1, ref ValReck v2, ref ValReck v3)
        {
            switch (v1.ValTyp)
            {
                case 1:
                    v3.RValue = v1.IValue / v2.IValue;
                    v3.ValTyp = 4;
                    v1.ValTyp = 4;
                    v2.ValTyp = 4;
                    break;
                case 4: v3.RValue = v1.RValue / v2.RValue; v3.ValTyp = 4; break;
                case 3: v3.SValue = v1.SValue + v2.SValue; v3.ValTyp = 3; break;
                case 2: v3.BValue = v1.BValue || v2.BValue;v3.ValTyp = 2; break;
                default: v3.Flag = false; break;
            }
        }

        private void __SUB(ref ValReck v1, ref ValReck v2, ref ValReck v3)
        {
            switch (v1.ValTyp)
            {
                case 1: v3.IValue = v1.IValue - v2.IValue;v3.ValTyp = 1; break;
                case 4: v3.RValue = v1.RValue - v2.RValue;v3.ValTyp = 4; break;
                default: v3.Flag = false; break;
            }
        }

        private void __MUL(ref ValReck v1, ref ValReck v2, ref ValReck v3)
        {
            switch (v1.ValTyp)
            {
                case 1: v3.IValue = v1.IValue * v2.IValue;v3.ValTyp = 1; break;
                case 4: v3.RValue = v1.RValue * v2.RValue;v3.ValTyp = 4; break;
                case 3: v3.SValue = v1.SValue + v2.SValue;v3.ValTyp = 3; break;
                case 2: v3.BValue = v1.BValue && v2.BValue;v3.ValTyp = 2; break;
                default: v3.Flag = false; break;
            }
        }

        private void __ADD(ref ValReck v1, ref ValReck v2, ref ValReck v3)
        {
            switch (v1.ValTyp)
            {
                case 1: v3.IValue = v1.IValue + v2.IValue;v3.ValTyp = 1; break;
                case 4: v3.RValue = v1.RValue + v2.RValue;v3.ValTyp = 4; break;
                case 3: v3.SValue = v1.SValue + v2.SValue;v3.ValTyp = 3; break;
                case 2: v3.BValue = v1.BValue || v2.BValue;v3.ValTyp = 2; break;
                default: v3.Flag = false;break;  
            }
        }

        private void _ConvertTypes(ref ValReck v1, ref ValReck v2)
        {
            //__ConvertTypes(v1, v2.ValTyp);
            __ConvertTypes(v2, v1.ValTyp);
        }

        private void __ConvertTypes(ValReck v1, byte valTyp)
        {
            if (valTyp == 1)
            {
                v1.Flag = true;
                int v;
                switch (v1.ValTyp)
                {
                    case 1:
                        break;
                    case 3:
                        v1.IValue = int.TryParse(v1.SValue, out v)?int.Parse(v1.SValue):0;
                    break;
                    case 4:
                        v1.IValue = (int)v1.RValue; break;
                    case 2:
                        v1.IValue = v1.BValue ? 1 : 0; break;
                    default:
                        v1.Flag = false;
                        break;
                }
            }
            if (valTyp == 2)
            {

                switch (v1.ValTyp)
                {
                    case 1: v1.BValue =v1.IValue>0;break;
                    case 3:
                        v1.BValue = v1.SValue=="ДА" || v1.SValue == "Дa" || v1.SValue == "дa";
                        break;
                    case 4:
                        v1.BValue = v1.RValue>0; break;
                    case 5:
                        v1.BValue = false; break;
                    default:
                        v1.Flag = false;
                        break;
                }
            }
            if (valTyp == 3)
            {
                v1.Flag = true;
                switch (v1.ValTyp)
                {
                    case 1:v1.SValue = v1.IValue.ToString(); break;
                    case 2:v1.SValue = v1.BValue.ToString(); break;
                    case 4:v1.SValue = v1.RValue.ToString(); break;
                    case 5:v1.SValue = v1.DValue.ToShortDateString(); break;
                }
            }
            if (valTyp ==4)
            {
                v1.Flag = true;
                switch (v1.ValTyp)
                {

                    case 1:
                        v1.RValue = v1.IValue;
                        break;
                    case 4:
                        break;
                    case 3:
                        v1.RValue = decimal.Parse(v1.SValue); break;
                    default:
                        v1.Flag = false;
                        break;
                }
            }
            if (valTyp == 5)
            {
                v1.Flag = true;
                switch (v1.ValTyp)
                {

                    case 3:
                        v1.DValue = DateTime.Parse(v1.SValue);
                        break;
                    case 5:
                        break;
                    default:
                        v1.Flag = false;
                        break;
                }
            }
            if (v1.Flag == true) v1.ValTyp = valTyp;
        }

        private void Translate(Stack<object> iST1, Stack<object> iST2, string s)
        {
            iST1.Clear();iST2.Clear();
            ValReck VV1 = new ValReck();
            if (!string.IsNullOrWhiteSpace(s))
            {
                string ch = "(";
                string d = "";
                iST1.Push(ch);
                s=VerIzraz(s);
                ch ="";
                if (OpChar(s[0])) throw new Exception("Грешка" + CurrentWord);
                var i = 0;
                var j = s.Length-1;
                while (i<=j)
                {
                    ch = s[i].ToString();
                    if (NChar(s[i])) ReadName(iST1, iST2, s, ref i, j);
                    else if (s[i] == '\'') ReadConstant(iST1, iST2, s, ref i, j);
                        else if (s[i] == '(') iST1.Push(ch);
                            else if (s[i] == ')') Reform(iST1,iST2,ref ch,ref d);
                                else if (OpChar(s[i]))
                                      {
                                          Reform1(iST1,iST2,ref ch,ref d);
                                          iST1.Push(d);
                                          iST1.Push(ch);
                                      }
                    i++;
                }
                Reform(iST1, iST2, ref ch,ref d);
                if (iST2.Count > 0)
                {
                    iST1.Clear();
                    foreach (var item in iST2)
                    {
                        VV1 = new ValReck {Name=" "};
                        if (OpChar(item.ToString()[0]))
                        {
                            VV1.Name = item.ToString();
                        }
                        else
                        {
                            CalculIzraz(item.ToString(),ref VV1);
                        }
                        iST1.Push(VV1);
                    }
                } 
            }

        }

        private void CalculIzraz(string s,ref ValReck V3)
        {
            s = VerIzraz(s);
            string FN="", FV="";
            AnaliseFun(s,ref FN,ref FV);
            if (FN == "")
            {
                bool f = false;
                if (Pos('\'', FV) != -1) f = true;
                if (Pos('[', FV) != -1) GetOnePole(ref FV, ref V3);
                if (!V3.Flag) GetDefValue(FV, ref V3);
                //if (!V3.Flag) GetOnePole(ref FV, ref V3);
                if (!V3.Flag) ReturnValue(FV, ref V3);
                if (!V3.Flag)
                {
                    if (Pos('\'', s) != -1 || Pos('+', s) != -1
                        || Pos('-', s) != -1 || Pos('*', s) != -1
                        || Pos('/', s) != -1 || Pos('^', s) != -1
                        || Pos('%', s) != -1)
                    {
                        Calculi(FN, ref V3);
                    }
                    else
                    {
                        ControlString(FV, f, V3);
                        V3.ValTyp = 3;
                        V3.Flag = true;
                        V3.SValue = FV;
                    }
                }
                if (!V3.Flag) throw new Exception("Генерал ерор");
            }
            else
            {
                List<ValReck> param = new List<ValReck>();
                var par = FV.Split(',');
                foreach (var item in par)
                    {
                        CalculIzraz(item,ref V3);
                        param.Add(V3);
                    }
                CallFunction(FN,param,ref V3);
            }
        }

        private void CallFunction(string fN, List<ValReck> param,ref ValReck v3)
        {
            var fun = Intfa.FirstOrDefault(e => e.Name == fN.ToUpper());
            if (fun != null)
            {
                //Control var params
                _AplineFunction(fun, param,ref v3);
            }
        }

        private void _AplineFunction(FunType fun, List<ValReck> param, ref ValReck v3)
        {
            switch(fun.Name)
            {
                case "MESSAGE":
                    __ConvertTypes(param[0], 3);
                    OutPut.Add(param[0].SValue);
                    break;
            }
        }

        private void ControlString(string fV, bool f, ValReck v3)
        {
            var fN = fV.TrimStart().ToUpper();
            var fun = Intfa.FirstOrDefault(e => e.Name == fN.ToUpper());
            if (fun == null)
            {
                var con = IntConst.FirstOrDefault(e => e.Name == fN);
                if (con == null)
                {
                    var v = VarRoot.FirstOrDefault(e => e.Name == fN);
                    if (v == null)
                    {
                        v3.Flag = true;
                        v3.SValue = fN;
                        v3.ValTyp = 3;
                    }
                    //filehandle
                }
                else
                {
                    _GetInternalConstant(con, ref v3);
                }    
            }
        }

        private void _GetInternalConstant(IntConstReck con, ref ValReck v3)
        {
            v3.Flag = true;
            switch (con.Name)
            {
                case "CURENTDATE":v3.DValue = DateTime.Now;break;
            }
        }

        private void ReturnValue(string s,ref ValReck V3)
        {
            if (s == "ДА" || s == "'ДА'")
            {
                V3.ValTyp = 2;
                V3.Flag = true;
                V3.BValue = true;
                return;
            }
            if (s == "НЕ" || s == "'НЕ'")
            {
                V3.ValTyp = 2;
                V3.Flag = true;
                V3.BValue = false;
                return;
            }
            int i;
            if (int.TryParse(s,out i))
            {
                V3.ValTyp = 1;
                V3.Flag = true;
                V3.IValue = i;
                return;
            }
            decimal d;
            if (decimal.TryParse(s, out d))
            {
                V3.ValTyp = 1;
                V3.Flag = true;
                V3.RValue = d;
                return;
            }
            DateTime dat;
            if (DateTime.TryParse(s,out dat))
            {
                V3.ValTyp = 5;
                V3.Flag = true;
                V3.DValue = dat;
                return;
            }
            var v = IntConst.FirstOrDefault(e => e.Name == s);
            if (v != null)
            {
                GetInternalConstant(v.Name, ref V3);
            }
        }

        private void GetInternalConstant(string name, ref ValReck v3)
        {
            v3.Flag = false;
            switch (name)
            {
                case "CONFIG"     :v3.SValue = "CONFIG";v3.Flag = true; break;
                case "CURENTDATE" :v3.DValue = DateTime.Now;v3.Flag = true; break;
                case "DDSPROCENT" :v3.IValue = 20;v3.Flag = true;break;
            }
        }

        private void GetDefValue(string fV, ref ValReck v3)
        {
            v3.Flag = false;
            var v = VarRoot.FirstOrDefault(e => e.Name == fV);
            if (v != null)
            {
                v3 = v;
            }

        }

        private void GetOnePole(ref string fV, ref ValReck v3)
        {
            v3.Flag = false;
            //v3.SValue = fV;
            //v3.IValue = 10;
            //v3.RValue = 10;
        }

        private void AnaliseFun(string s, ref string fn, ref string fV)
        {
            int i = 0,m=0,l=0,j = s.Length-1;
            bool F = true;
            if (s[0]!='\'')
            {
                while (F && i <= j)
                {
                    if (s[i] == '(') m++;
                    if (s[i] == '[') l++;
                    if (s[i] == ')') m--;
                    if (s[i] == ']') l--;
                    if (l == 0)
                    {
                        if (m == 0)
                        {
                            fn += s[i];
                            i++;
                        }
                        else
                        {
                            F = false;
                        }
                    }
                    else
                    {
                        fn += s[i];
                        i++;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(s))
            {
                if (i <= s.Length - 1)
                {
                    fV = Copy(s, i, s.Length);
                }
            }
            if (string.IsNullOrWhiteSpace(fV))
            {
                fV = s;
                fn = "";
            }
            while (fV[0] == '(' && fV[fV.Length-1] == ')')
            {
                fV = fV.Substring(1, fV.Length - 2);
            }
        }

        private void Reform1(Stack<object> iST1, Stack<object> iST2, ref string ch,ref string d)
        {
            d = iST1.Pop().ToString();
            while (d != "(" && Teglo(d)<=Teglo(ch))
            {
                iST2.Push(d);
                d = iST1.Pop().ToString();
            }
        }

        private int Teglo(string s)
        {
            int i= 0;
            switch (s[0]) {
                case '+':
                case '-': i= 3;break;
                case '*':
                case '/': i= 2;break;
                case '^':
                case '%': i= 1;break;
            }
            return i;
        }

        private void Reform(Stack<object> iST1, Stack<object> iST2, ref string ch,ref string d)
        {
            d = iST1.Pop().ToString();
            while (d!="(" )
            {
                iST2.Push(d);
                d = iST1.Pop().ToString();
            }
        }

        private void ReadConstant(Stack<object> iST1, Stack<object> iST2, string s,ref int i, int j)
        {
            string c = "";
            i++;
            while (s[i]!='\'' && i <= j)
            {
                c += s[i];
                i++;
            }
            //if (s[i] == '\'') c += s[i];
            //else throw new Exception("Грешна константа");
            if (!string.IsNullOrEmpty(c)) iST2.Push(c);
        }

        private void ReadName(Stack<object> iST1, Stack<object> iST2, string s, ref int i, int j)
        {
            string c = "";
            int m, l;
            while (i<=j && NChar(s[i]))
            {
                c += s[i];
                i++;
            }
            if (i <= j && !OpChar(s[i]))
            {
                c += s[i]; m = 0; l = 0;
                if (s[i] == '(') m++;
                if (s[i] == '[') l++;
                i++;
                while (!((m == 0) && (l == 0)) && i <= j)
                {
                    c += s[i];
                    if (s[i] == '(') m++;
                    if (s[i] == '[') l++;
                    if (s[i] == ')') m--;
                    if (s[i] == ']') l--;
                    i++;
                }
                if (m > 0 || l > 0) throw new Exception("Грешен израз");
                i--;
            }
            else
            {
                i--;
            }
            if (!string.IsNullOrEmpty(c)) iST2.Push(c);
        }

        private FunType GetIf(string c)
        {
            return Intfa.FirstOrDefault(e => e.Name == c);
        }

        private bool NChar(char ch)
        {
            return (char.IsLetterOrDigit(ch)||ch=='_');
        }

        private bool OpChar(char v)
        {
            char[] vv = new char[] { '+', '-', '/', '*', '%', '^', '&', '|' };
            return vv.Contains(v);
        }

        private string VerIzraz(string s)
        {
           
            // Indices of the currently open parentheses:
            Stack<int> parentheses = new Stack<int>();
            var ss = s;
            while (ss.Length>1 && ss[0]=='(' && ss[ss.Length-1]==')')
            {
                ss = ss.Substring(1, ss.Length - 2);
            }
            foreach (char chr in ss)
            {
                int index;

                // Check if the 'chr' is an open parenthesis, and get its index:
                if ((index = Array.IndexOf(OpenParentheses, chr)) != -1)
                {
                    parentheses.Push(index);  // Add index to stach
                }
                // Check if the 'chr' is a close parenthesis, and get its index:
                else if ((index = Array.IndexOf(CloseParentheses, chr)) != -1)
                {
                    // Return 'false' if the stack is empty or if the currently
                    // open parenthesis is not paired with the 'chr':
                    if (parentheses.Count == 0 || parentheses.Pop() != index)
                    {
                        throw new Exception("Грешка" + CurrentWord);
                    }
                }
            }
            // Return 'true' if there is no open parentheses, and 'false' - otherwise:
            if (parentheses.Count>0)  throw new Exception("Грешка" + CurrentWord);
            return ss;
    }

        private void Apply_For(RezWType i0, bool ignore)
        {
            string s = PAR[1];string v = PAR[3];string w = PAR[5];
            byte k = i0.IncC;int j = 0;int i = 0; bool f = true;
            int olddb = Counters[k] - 1;
            if (ignore)
            {
                ValReck v1=new ValReck(), v2=new ValReck();
                Calculi(v, ref v1);
                Calculi(w, ref v2);
                __ConvertTypes(v1, 1);
                __ConvertTypes(v2, 1);
                i = v1.IValue;j = v2.IValue;
                if (i==0)
                {
                    f = false;
                }
                for (var l = i; l <= j; l++)
                {
                    v1.IValue = l;
                    v1.ValTyp = 1;
                    SetDefValue(s, v1);
                    do
                    {
                        var oldc = LineC;var oldwc = WordC;
                        Apply_Struct(ignore);
                        LineC = oldc;WordC = oldc;
                    } while (!NoMax() || olddb == Counters[k]);
                }
            }
        }

        private void Apply_Repeat(RezWType i0, bool ignore)
        {
            throw new NotImplementedException();
        }

        private void Apply_While(RezWType i0, bool ignore)
        {
            throw new NotImplementedException();
        }

        private void Apply_Loop(RezWType i0, bool ignore)
        {
            throw new NotImplementedException();
        }

        private void Apply_If(RezWType i0, bool ignore)
        {
            throw new NotImplementedException();
        }

        private void _GetWord()
        {
            var s = CurrentWord;
            if (!string.IsNullOrWhiteSpace(s))
            {
                int k = 0, l = 0, j = 0;
                bool ff = true,f = true;
                if (s[0] != '=')
                {
                    do
                    {
                        if (s[k] == '\'') f = !f;
                        if (s[k] == '[') l++;
                        if (s[k] == ']') l--;
                        if (s[k] == '(') j++;
                        if (s[k] == ')') j--;
                        if (f && s[k] == '=')
                        {
                            ff = false;
                            k--;
                        }
                        if (f && s[k] == ' ' && l == 0 && j == 0)
                        {
                            ff = false;
                            k--;
                        }
                        k++;
                    }
                    while (k < s.Length && ff);
                }
                else
                {
                    k = 1;
                }
               if (k > s.Length) k = s.Length;
               GetWord = Copy(CurrentWord,k);
               CurrtIrw = GetIrw(GetWord);
               CurrentWord = CurrentWord.TrimStart();
            } 
        }

        private RezWType GetIrw(string getWord)
        {
            var r = Irw.FirstOrDefault(e => e.Name == getWord.ToUpper());
            if (r != null)
            {
                return r;
            }
            return null;
        }

        private string Copy(string s, int begin)
        {
            return s.Substring(0,begin);
        }
        private string Copy(string s, int begin,int end)
        {
            var len = end - begin;
            if (len < 0)
            {
                return "";
            }
            return s.Substring(begin,len);
        }
        private int Pos(char test, string s)
        {
            return s.IndexOf(test);
        }
        private bool NoMax()
        {
            return LineC <= MaxC;
        }
        private void IncWC()
        {
            do
            {
                if (NoMax())
                {
                    if (String.IsNullOrWhiteSpace(CurrentWord))
                    {
                        LineC++;
                        if (!NoMax()) return;
                        CurrentWord = TextLines[LineC];
                        WordC = 1;
                       
                    }
                    else
                    {
                        WordC++;
                    }
                }
            }
            while (!NoMax() || string.IsNullOrWhiteSpace(CurrentWord));
            if (NoMax())
            {
                if (WordC != 1)
                {
                    var s = GetWord;
                    if (CurrentWord.Contains(s))
                    {
                        CurrentWord = CurrentWord.Replace(s, "");
                        CurrentWord = CurrentWord.TrimStart();
                    }
                    if (String.IsNullOrWhiteSpace(CurrentWord))
                    {
                        LineC++;
                        if (!NoMax()) return;
                        CurrentWord = TextLines[LineC];
                        WordC = 1;
                    }
                }
            }
            _GetWord();
        }
    }
}
