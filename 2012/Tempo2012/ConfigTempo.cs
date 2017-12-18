using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;
using System.IO;

namespace Tempo2012.EntityFramework
{
    [Serializable]
    public class ConfigTempo
    {
        public virtual DateTime WorkData { get; set;}
        public virtual FirmModel CurrentFirma { get; set;}
        public virtual string Shema { get; set; }
        public virtual string ConectionString { get; set;}
        public virtual byte UIMode { get; set;}
        public virtual DeclarConfigModel DeclarConfig { get; set; }
    }
    [Serializable]
    public class ConfigTempoSinglenton
    {
        private const string Configfilename = "config.ini";
        private ConfigTempo currentConfig=new ConfigTempo();
        private static ConfigTempoSinglenton  _instance;
        private Tempo2012DataBaseContext contex=new Tempo2012DataBaseContext();
        private ConfigTempoSinglenton()
        {
            if (File.Exists(Configfilename))
            {
                LoadConfiguration();
            }
            else
            {
                currentConfig.CurrentFirma = contex.GetLastFirm();
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
                return currentConfig.CurrentFirma;
            }
            set
            {
                currentConfig.CurrentFirma = value;
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
        public void  SaveConfiguration()
        {
            SerializeUtil.SerializeToXML<ConfigTempo>(Configfilename, currentConfig);
        }
        private void LoadConfiguration()
        {
            currentConfig = SerializeUtil.DeserializeFromXML<ConfigTempo>(Configfilename);
        }

        public DeclarConfigModel DeclarConfig { get { return currentConfig.DeclarConfig; } set { currentConfig.DeclarConfig = value;} }
    }
}
