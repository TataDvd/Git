using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Tempo2012.EntityFramework.Models
{
    public interface ITransportObject
    {
        string GetTableName();
        Dictionary<string, object> GetFields();
        void SetFields(Dictionary<string, object> list);
    }
    public abstract class BaseModel : ITransportObject 
    {
       

        public virtual Dictionary<string,object> GetFields()
        {
            var result = new Dictionary<string, object>();
            Type myObjectType = this.GetType();
            PropertyInfo[] propertyInfo =myObjectType.GetProperties();

            foreach (System.Reflection.PropertyInfo info in propertyInfo)
            {
                result.Add(info.Name,info.GetValue(this,null));
            }
            return result;
        }

        public virtual void SetFields(Dictionary<string, object> list)
        {
            var result = new Dictionary<string, object>();
            Type myObjectType = this.GetType();
            PropertyInfo[] propertyInfo = myObjectType.GetProperties();

            foreach (System.Reflection.PropertyInfo info in propertyInfo)
            {
                info.SetValue(null, list[info.Name], null);
            }
        }

        public virtual string GetTableName()
        {
            return "";

        }
    }
}
