using System;
using ShareLib.CommandParser.Interface;

namespace ShareLib.CommandParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ConstraintAttribute : Attribute
    {
        public ConstraintAttribute(Type converter)
        {
            ValueConverter = (IFieldConverter)Activator.CreateInstance(converter);
            StartPos = 0;
            Packaged = false;
            Last = false;
            TrimFirst = false;
        }

        public int MaxLength { set; get; }
        public IFieldConverter ValueConverter { private set; get; }
        public int StartPos { set; get; }
        public bool Packaged { set; get; }
        public bool Last { set; get; }
        public bool TrimFirst { set; get; }
    }
}
