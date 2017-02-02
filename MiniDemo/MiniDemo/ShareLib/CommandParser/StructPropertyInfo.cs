using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ShareLib.CommandParser.Attributes;

namespace ShareLib.CommandParser
{
    public class StructPropertyInfo
    {
        public ConstraintAttribute Constraint;
        public int Count;
        public PropertyInfo Property;
        public RangeAttribute Range;
    }
}
