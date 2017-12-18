using System;

namespace Tempo2012.EntityFramework.Models
{
    public class DdsItemModel
    {
       
        private decimal _ddsSuma;
        public bool IsNotComputed { get; set;}
        public virtual decimal DdsSuma
        {
            get { return _ddsSuma; }
            set
            {
                _ddsSuma = value;
                if (IsNotComputed) return;
                _ddsTotal =Math.Round( _ddsSuma + _ddsSuma * _ddsPercent / 100,2);
                _dds = _ddsTotal - _ddsSuma;
            }
        }

        private decimal _ddsPercent;
        public virtual decimal DdsPercent
        {
            get { return _ddsPercent; }
            set
            {
                _ddsPercent = value;
                if (IsNotComputed) return;
                _ddsTotal = Math.Round(_ddsSuma + _ddsSuma * _ddsPercent / 100,2);
                _dds = _ddsTotal - _ddsSuma;
            }
        }

        public virtual string Name { get; set;}
        private decimal _ddsTotal;
        

        public virtual decimal DdsTotal
        {
            get { return _ddsTotal; }
            set
            {
                _ddsTotal = value;
                if (IsNotComputed) return;
                _ddsSuma =Math.Round(_ddsTotal * 100 / (_ddsPercent + 100),2);
                _dds = _ddsTotal - _ddsSuma;
            }
        }
        private decimal _dds;
        public virtual decimal Dds
        {
            get { return _dds; }
            set 
            {
                _dds = value;
                if (IsNotComputed) return;
                _ddsTotal = _dds + _ddsSuma;
                if (_ddsSuma != 0) _ddsPercent = Math.Round(_dds * 100 / _ddsSuma,2);
            }
        }
        private bool _in;
        public virtual bool In
        {
            get { return _in; }
            set
            {
                _in = value;
                
            }
        }
        public virtual int Id { get; set; }
        public virtual string Code { get; set;}
        public override string ToString()
        {
            return string.Format("{0,4}-{1}", Code, Name);
        }
        public virtual string NameCode {
            get
            {
                return ToString();
            } 
        }
    }
}