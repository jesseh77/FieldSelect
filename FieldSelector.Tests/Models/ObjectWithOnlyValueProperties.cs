using System;
using System.Collections.Generic;
using System.Text;

namespace FieldSelector.Tests.Models
{
    internal class ObjectWithOnlyValueProperties
    {
        public int intProperty { get; set; }
        public double doubleProperty { get; set; }
        public DateTime datetimeProperty { get; set; }
        public string stringProperty { get; set; }
    }
}
