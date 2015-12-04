using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synvata.ORM
{
    public class DynamicEntity<T>
    {
        public T Entity { get; set; }
         
        Dictionary<string, object> _propValues = new Dictionary<string, object>();

        private DynamicEntity() { }

        public DynamicEntity(T entity)
        {
            Entity = entity;
        }

        public T GetProperty<T>(string propName)
        {
            if (_propValues.ContainsKey(propName))
                return (T)_propValues[propName]  ;
            var pinfo = Entity.GetType().GetProperty(propName);
            if (pinfo == null)
                throw new ApplicationException("Cannot find property [" +propName +"] for class " + typeof(T));
            object value = pinfo.GetValue(Entity);
            _propValues.Add(propName, value);
            return (T)value;
        }
    }
}
