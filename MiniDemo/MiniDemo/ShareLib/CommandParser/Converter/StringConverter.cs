using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ShareLib.CommandParser.Interface;

namespace ShareLib.CommandParser.Converter
{
    public class StringConverter : IFieldConverter
    {
        public object Convert(string value, string invalid)
        {
            return (value.Trim());
        }

        public string ConvertBack(object value, string format, string invalid)
        {
            string ret = value as string;

            if (value == null)
            {
                ret = invalid;
            }
            else if (!string.IsNullOrEmpty(format))
            {
                ret = String.Format(CultureInfo.InvariantCulture, format, value);
            }

            return (ret);
        }
    }
}
