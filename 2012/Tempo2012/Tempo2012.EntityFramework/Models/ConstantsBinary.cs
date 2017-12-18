using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework.Models
{
    public class ConstantsBinary
    {
        public const uint UpdateAcc = 1;
        public const uint UpdateSaldo = 2;
        public const uint DeleteConto = 4;
        public const uint UpdateConto = 8;
        public const uint OborotReport = 16;
        public const uint FinishMonth = 32;
        public const uint BalansReport = 64;
        public const uint ReportPeriodi = 128;
        public const uint Classes = 256; 
        public const uint NewCurrency =256;
        public const uint AddCurrencyRates = 512;
        public const uint FinishYear = 1024;
        public const uint AddStore = 2048;
        public const uint StoreReports = 4096;
        public const uint HronologicalReport =8192;
        public const uint AnaliticalReport = 16384;
        public const uint All = 32767;
    }
}
