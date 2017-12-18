using System;

namespace Tempo2012.EntityFramework
{
    [Serializable]
    public class FirmSettingModel
    {
        public int HoldingId { get; set;}
        public int FirmaId { get; set;}
        public string Key { get; set;}
        public string Name { get; set; }
        public string Value { get; set;}
        public FirmSettingModel Clone()
        {
            return (FirmSettingModel)this.MemberwiseClone();
        }
    }
}