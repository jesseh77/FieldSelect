using FieldSelector.Tests.Models;
using FieldSelector;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.CSharp.RuntimeBinder;

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

            Assert.Equal(dyn.anIntProperty, propValue);
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

        [Fact]
        public void should_return_only_requested_fields()
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
            Assert.Throws(typeof(RuntimeBinderException), () => result.intProperty);
            Assert.Throws(typeof(RuntimeBinderException), () => result.stringProperty);
            Assert.Throws(typeof(RuntimeBinderException), () => result.datetimeProperty);
        }

        [Fact]
        public void should_throw_null_reference_exception_when_item_is_null()
        {
            object obj = null;
            var fields = new List<string> { "doubleProperty" };

            Action action = () =>
            {
                dynamic result = SelectFields.FromObject(obj, fields);
            };

            Assert.Throws(typeof(NullReferenceException), action);
        }

        [Fact]
        public void should_throw_exception_when_invalid_field_name_provided()
        {
            var obj = new ObjectWithReferenceAndValueProperties
            {
                intProperty = 7,
                doubleProperty = 11,
                stringProperty = "a property",
                datetimeProperty = DateTime.Now
            };
            var fields = new List<string> { "notAValidProperty" };

            Action action = () => SelectFields.FromObject(obj, fields);
            Assert.Throws(typeof(NullReferenceException), action);
        }
    }
}
