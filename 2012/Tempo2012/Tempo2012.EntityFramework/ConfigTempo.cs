using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.EntityFramework
{
    [Serializable]
    public class ConfigTempo
    {
        public virtual DateTime WorkData { get; set;}
        public virtual int CurrentFirma { get; set;}
        public virtual string Shema { get; set; }
        public virtual string ConectionString { get; set;}
        public virtual byte UIMode { get; set;}
        public virtual DeclarConfigModel DeclarConfig { get; set; }
        [XmlArray("DeclarConfigModels"), XmlArrayItem(typeof(DeclarConfigModel), ElementName = "DeclarConfigModel")]
        public virtual List<DeclarConfigModel> DeclarConfigs { get; set; }
        public virtual string BaseDbPath { get; set;}
        public virtual string BaseArhivePath { get; set; }
        public virtual string BaseTemplatePath { get; set; }
        public virtual int ActiveHolding { get; set;}
        [XmlArray("Holdings"), XmlArrayItem(typeof(HoldingModel), ElementName = "Holding")]
        public virtual List<HoldingModel> Holdings { get; set; }
        [XmlArray("FirmSettings"), XmlArrayItem(typeof(FirmSettingModel), ElementName = "FirmSettingModel")]
        public virtual List<FirmSettingModel> FirmSettings { get; set; }
        [XmlArray("ConfigNames"), XmlArrayItem(typeof(string), ElementName = "ConfigName")]
        public virtual List<string> ConfigNames { get; set; }

        [XmlArray("Periods"), XmlArrayItem(typeof(PeriodModel), ElementName = "Period")]
        public virtual List<PeriodModel> Periods { get; set; }
    }
}
