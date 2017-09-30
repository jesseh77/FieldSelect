using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

namespace FieldSelector
{
    public class SelectFields : ISelectFields
    {
        /// <summary>
        /// Takes an object and a string list of field names. Returns a dynamic object with only the specified
        /// fields from the object. Variable declaration for return value must use the dynamic keyword. ie:
        ///   dynamic newObject = Select(oldObject, fieldList);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="fieldNames"></param>
        /// <returns>object</returns>
        public object Select<T>(T item, IEnumerable<string> fieldNames) where T : class
        {
            return FromObject(item, fieldNames);
        }

        /// <summary>
        /// Takes an object and a string list of field names. Returns a dynamic object with only the specified
        /// fields from the object. Variable declaration for return value must use the dynamic keyword. ie:
        ///   dynamic newObject = SelectFields.FromObject(oldObject, fieldList);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="fieldNames"></param>
        /// <returns>object</returns>
        public static object FromObject<T>(T item, IEnumerable<string> fieldNames) where T : class
        {
            if (item == null) throw new NullReferenceException();
            var fieldNameSegments = fieldNames.Select(x => x.Split('.'));
            return FromObject(item, fieldNameSegments);
        }

        private static object FromObject<T>(T item, IEnumerable<string[]> fieldNames) where T : class
        {
            var fieldNameSegments = fieldNames.GroupBy(x => x.First());
            dynamic obj = new DynamicFields();
            
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
