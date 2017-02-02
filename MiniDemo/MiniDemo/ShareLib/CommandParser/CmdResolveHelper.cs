using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ShareLib.CommandParser.Attributes;
using ShareLib.CommandParser.Interface;

namespace ShareLib.CommandParser
{
    public class CmdResolveHelper
    {
        public static bool ParseStructByType(IResolvableCmd data, Type type, string command, char split)
        {
            var fields = command.Split(new char[] {split}).ToList<string>();

            var count = type.GetProperties().Length;
            var fieldsCnt = fields.Count;

            //set the ommision field as empty string
            for (var i = 0; i < count - fieldsCnt; i++)
            {
                fields.Add("");
            }

            var ret = (fields.Count == count) ? ParseStructByType(data, type, fields) : false;

            return (ret);
        }

        public static bool ParseStructByType(IResolvableCmd target, Type type, List<string> fields)
        {
            var ret = true;

            var index = 0;

            EnumProperty(target, (t, info) =>
            {
                var result = SetPropertyValue(t, info, fields, ref index);

                if (!result)
                {
                    ret = false;
                }

                return (result);
            });

            return (ret);
        }

        public delegate bool PropertyCallback(IResolvableCmd data, StructPropertyInfo info);

        private static Attribute GetCustomAttribute(PropertyInfo info, Type type)
        {
            var attributes = info.GetCustomAttributes(type, false);
            return (Attribute) ((attributes.Length > 0) ? attributes[0] : null);
        }

        public static void EnumProperty(IResolvableCmd data, PropertyCallback callback)
        {
            var props = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var info in props)
            {
                var internalClass = (InternalClassAttribute) GetCustomAttribute(info, typeof (InternalClassAttribute));

                //when inner class is array
                if (info.PropertyType.IsArray && internalClass != null)
                {
                    //Array array = info.GetValue(data) as Array; //.Net4.5
                    Array array = info.GetValue(data, null) as Array;
                    foreach (var item in array)
                    {
                        EnumProperty(item as IResolvableCmd, callback);
                    }
                }
                else
                {
                    if (internalClass != null)
                    {
                        EnumProperty(info.GetValue(data, null) as IResolvableCmd, callback);
                    }
                    else 
                    {
                        //var attrib = (ConstraintAttribute)info.GetCustomAttribute(typeof(ConstraintAttribute)); //.Net4.5
                        var attrib = (ConstraintAttribute) GetCustomAttribute(info, typeof (ConstraintAttribute));
                        var range = (RangeAttribute) GetCustomAttribute(info, typeof (RangeAttribute));

                        if (callback != null)
                        {
                            var ret =
                                callback(data, new StructPropertyInfo
                                {
                                    Count = props.Length,
                                    Property = info,
                                    Constraint = attrib,
                                    Range = range
                                });

                            if (!ret)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static bool SetValue(bool isArray, IResolvableCmd data, StructPropertyInfo info, string field, int index)
        {
            bool ret = false;

            var invalid = info.Range == null ? null : info.Range.InvalidValue;

            var value = info.Constraint.ValueConverter.Convert(field, invalid);

            if (info.Constraint.MaxLength == -1)
            {
                ret = true;
            }
            else
            {
                if (field.Length <= info.Constraint.MaxLength)
                {
                    if (info.Range != null)
                    {
                        if (value != null)
                        {
                            if (info.Range.CheckRange(value))
                            {
                                ret = true;
                            }
                        }
                    }
                    else
                    {
                        ret = true;
                    }
                }
            }

            if (ret)
            {
                if (isArray)
                {
                    var array = info.Property.GetValue(data, null) as Array;
                    if (array != null) array.SetValue(value, index);
                }
                else
                {
                    info.Property.SetValue(data, value, null);
                }
            }

            return (ret);
        }

        public static bool SetPropertyValue(IResolvableCmd data, StructPropertyInfo info, List<string> field,
            ref int index)
        {
            var ret = true;
            var isArray = info.Property.PropertyType.IsArray;

            try
            {
                var count = 1;

                if (isArray)
                {
                    var array = info.Property.GetValue(data, null) as Array;
                    count = array.Length;
                }

                for (var i = 0; i < count; i++)
                {
                    var attrib = (ConstraintAttribute) GetCustomAttribute(info.Property, typeof (ConstraintAttribute));

                    if (attrib.Packaged)
                    {
                        SetValue(isArray, data, info, field[index].Substring(attrib.StartPos, attrib.MaxLength), i);
                        if (attrib.Last)
                        {
                            index++;
                        }
                    }
                    else
                    {

                        if (index < field.Count) 
                        {
                            SetValue(isArray, data, info, field[index], i);
                            index++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                ret = false;
            }
            catch (FormatException)
            {
                ret = false;
            }
            catch (OverflowException)
            {
                ret = false;
            }

            return (ret);
        }

        public static string GetPropertyString(IResolvableCmd data, StructPropertyInfo info, string split)
        {
            var value = info.Property.GetValue(data, null);
            var sb = new StringBuilder();

            int count = 1;
            Array array = null;
            var isArray = info.Property.PropertyType.IsArray;

            if (isArray)
            {
                array = info.Property.GetValue(data, null) as Array;
                count = array.Length;
            }

            for (var i = 0; i < count; i++)
            {
                if (isArray)
                {
                    value = array.GetValue(i);
                }

                string field;
                if (info.Range != null)
                {
                    if (value != null)
                    {
                        if (!info.Range.CheckRange(value))
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }

                    field = info.Constraint.ValueConverter.ConvertBack(value, info.Range.Format, info.Range.InvalidValue);
                }
                else
                {
                    field = info.Constraint.ValueConverter.ConvertBack(value, null, null);
                }

                sb.Append(field + split);
            }

            return (sb.ToString());
        }

        public static string ToCommandString(IResolvableCmd data, char split, bool addEndSplit = false,
            string specialHead = null)
        {
            StringBuilder builder = new StringBuilder();

            int index = 0;

            EnumProperty(data, (d, info) =>
            {
                string result = GetPropertyString(d, info, split.ToString());

                if (index == info.Count - 1)
                {
                    if (!addEndSplit)
                    {
                        //delete the last split
                        result = result.Replace(split.ToString(), "");
                    }
                    builder.Append(result);
                }
                else
                {
                    builder.Append(result);
                }

                index++;

                return (true);
            });

            return (builder.ToString());
        }
    }
}
