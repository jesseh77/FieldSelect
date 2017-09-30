using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace FieldSelector
{
    public static class SelectFields
    {
        public static object FromObject<T>(T item, IEnumerable<string> fieldNames) where T : class
        {
            var fieldNameSegments = fieldNames.Select(x => x.Split('.'));
            return FromObject(item, fieldNameSegments);
        }

        private static object FromObject<T>(T item, IEnumerable<string[]> fieldNames) where T : class
        {
            var fieldNameSegments = fieldNames.GroupBy(x => x.First());
            dynamic obj = new DynamicFields();
            var fields = item.GetType().GetRuntimeProperties();
            
            foreach (var field in fieldNameSegments)
            {
                if (field.Count() == 1)
                {
                    var value = typeof(T).GetRuntimeProperty(field.Key).GetValue(item);
                    obj.Add(field.Key, value);
                }
                else
                {
                    var value = typeof(T).GetRuntimeProperty(field.Key).GetValue(item);
                    var relativeFieldNames = field.Skip(1);
                    
                    obj.Add(field.Key, FromObject(value, relativeFieldNames));
                }
            }

            return obj;
        }
    }
}
