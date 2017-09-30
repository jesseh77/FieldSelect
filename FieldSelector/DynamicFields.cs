using System.Collections.Generic;
using System.Dynamic;

namespace FieldSelector
{
    public class DynamicFields : DynamicObject
    {
        private Dictionary<string, object> values;

        public DynamicFields()
        {
            values = new Dictionary<string, object>();
        }
        public void Add(string propertyName, object value)
        {
            values.Add(propertyName, value);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (values.ContainsKey(binder.Name))
            {
                result = values[binder.Name];
                return true;
            }
            result = null;
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if(values.ContainsKey(binder.Name))
            {
                return false;
            }
            values.Add(binder.Name, value);
            return true;
        }
    }
}
