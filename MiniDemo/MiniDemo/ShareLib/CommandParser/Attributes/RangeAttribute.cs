using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareLib.CommandParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class RangeAttribute : Attribute
    {
        public RangeAttribute()
        {
            Format = null;
            InvalidValue = null;
        }

        public virtual bool CheckRange(object value)
        {
            return (true);
        }

        public string Format { set; get; }
        public string InvalidValue { set; get; }
    }
}
