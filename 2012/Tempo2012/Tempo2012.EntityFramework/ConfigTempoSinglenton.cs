using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.EntityFramework
{
    [Serializable]
    public class ConfigTempoSinglenton
    {
        private const string Configfilename = "config.ini";
        private ConfigTempo currentConfig=new ConfigTempo();
        private static ConfigTempoSinglenton  _instance;
        private TempoDataBaseContext contex=new TempoDataBaseContext();
        private ConfigTempoSinglenton()
        {
            if (File.Exists(Configfilename))
            {
                LoadConfiguration();
                Entrence.ConnectionString  = string.Format(currentConfig.ConectionString);
                Entrence.CurrentFirma = contex.GetAllFirma().FirstOrDefault(e => e.Id == ActiveFirma);
                if (Entrence.CurrentFirma != null)
                    currentConfig.CurrentFirma =
                        Entrence.CurrentFirma.Id;
                
            }
            else
            {
                Entrence.ConnectionString  = string.Format(Entrence.ConectionStringTemplate,"localhost","D:\\TempoData\\Data","H1");
                Entrence.CurrentFirma=contex.GetLastFirm();
                currentConfig.CurrentFirma = Entrence.CurrentFirma.Id;
                currentConfig.WorkData = DateTime.Now;
                currentConfig.DeclarConfig = new DeclarConfigModel();
            }
        }
        public static ConfigTempoSinglenton GetInstance()
        {
            return _instance ?? (_instance = new ConfigTempoSinglenton());
        }
        public FirmModel CurrentFirma 
        {
            get
            {
                return Entrence.CurrentFirma;
            }
            set
            {
                Entrence.CurrentFirma = value;
                currentConfig.CurrentFirma = value.Id;
            }
        }
        public DateTime WorkDate
        {
            get { return currentConfig.WorkData; }
            set { currentConfig.WorkData = value; }
        }
        public byte ModeUI { get { return currentConfig.UIMode; } set { currentConfig.UIMode = value;} }
    
        public string Shema
        {
            get
            {
                return currentConfig.Shema;
            }
            set 
            {
                currentConfig.Shema = value;
            }
        }
        public string ConectionString
        {
            get
            {
                return currentConfig.ConectionString;
            }
            set
            {
                currentConfig.ConectionString = value;
            }
        }
        public string BaseDbPath
        {
            get
            {
                return currentConfig.BaseDbPath;
            }
            set
            {
                currentConfig.BaseDbPath = value;
            }
        }

        public string BaseArhivePath
        {
            get
            {
                return currentConfig.BaseArhivePath;
            }
            set
            {
                currentConfig.BaseArhivePath = value;
            }
        }
        public int ActiveHolding
        {
            get
            {
                return currentConfig.ActiveHolding;
            }
            set
            {
                currentConfig.ActiveHolding = value;
            }
        }
        public int ActiveFirma
        {
            get
            {
                return currentConfig.CurrentFirma;
            }
            set
            {
                currentConfig.CurrentFirma = value;
            }
        }

        public string BaseTemplatePath
        {
            get
            {
                return currentConfig.BaseTemplatePath;
            }
            set
            {
                currentConfig.BaseTemplatePath = value;
            }
        }

        public List<DeclarConfigModel> DeclarConfigs
        {
            get
            {
                return currentConfig.DeclarConfigs;
            }
            set
            {
                currentConfig.DeclarConfigs = value;
            }
        }
        public void  SaveConfiguration()
        {
            SerializeUtil.SerializeToXML<ConfigTempo>(Configfilename, currentConfig);
        }
        private void LoadConfiguration()
        {
            currentConfig = SerializeUtil.DeserializeFromXML<ConfigTempo>(Configfilename);
        }

        public DeclarConfigModel DeclarConfig { get { return currentConfig.DeclarConfig; } set { currentConfig.DeclarConfig = value;} }
        public List<HoldingModel> Holdings {
            get
            {
                return currentConfig.Holdings;
            }
            set
            {
                currentConfig.Holdings = value;
            }
        }
        public List<FirmSettingModel> FirmSettings
        {
            get
            {
                return currentConfig.FirmSettings;
            }
            set
            {
                currentConfig.FirmSettings = value;
            }
        }
        public List<string> ConfigNames
        {
            get
            {
                return currentConfig.ConfigNames;
            }
            set
            {
                currentConfig.ConfigNames = value;
            }
        }
        public List<PeriodModel> Periods
        {
            get
            {
                return currentConfig.Periods;
            }
            set
            {
                currentConfig.Periods = value;
            }
        }
     
    }
}