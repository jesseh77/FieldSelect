using System.Collections.Generic;

namespace FieldSelector
{
    public interface ISelectFields
    {
        object Select<T>(T item, IEnumerable<string> fieldNames) where T : class;
    }
}