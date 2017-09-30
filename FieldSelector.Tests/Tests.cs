using FieldSelector.Tests.Models;
using FieldSelector;
using System;
using System.Collections.Generic;
using Xunit;

namespace FieldSelectorTests
{
    public class SimpleObjectTests
    {
        [Fact]
        public void should_return_first_level_fields()
        {
            var obj = new ObjectWithReferenceAndValueProperties
            {
                intProperty = 7,
                doubleProperty = 11,
                stringProperty = "a property",
                datetimeProperty = DateTime.Now
            };
            var fields = new List<string> { "doubleProperty" };

            dynamic result = SelectFields.FromObject(obj, fields);

            Assert.Equal(result.doubleProperty, obj.doubleProperty);
        }

        [Fact]
        public void should_return_properties_from_fielddynamic()
        {
            var propName = "anIntProperty";
            var propValue = 7;

            dynamic dyn = new DynamicFields();
            dyn.Add(propName, propValue);

            var result = dyn.anIntProperty;
            Assert.Equal(result, propValue);
        }

        [Fact]
        public void should_return_properties_from_child_object()
        {
            var propValue = 13;
            var propName = "ObjectProperty.doubleProperty";
            var obj = new ObjectWithReferenceAndValueProperties
            {
                ObjectProperty = new ObjectWithOnlyValueProperties
                {
                    doubleProperty = propValue
                }
            };

            dynamic result = SelectFields.FromObject(obj, new[] { propName });

            Assert.Equal(result.ObjectProperty.doubleProperty, propValue);
        }
    }
}
