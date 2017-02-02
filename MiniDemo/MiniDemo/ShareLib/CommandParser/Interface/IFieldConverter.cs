namespace ShareLib.CommandParser.Interface
{
    public interface IFieldConverter
    {
        object Convert(string value, string invalid);
        string ConvertBack(object value, string format, string invalid);
    }
}
